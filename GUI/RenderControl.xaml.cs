using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Wpf;

using LinearAlgebra;
using Geometry;
using Logic;

namespace GUI
{
    /// <summary>
    /// Interaction logic for RenderControl.xaml
    /// </summary>
    public partial class RenderControl : UserControl
    {
        private Camera camera;
        private int dummyVAO = 0;
        private bool initialized = false;
        private readonly double ZoomDelta = 1.1;
        private DocumentViewModel viewModel;
        internal DocumentViewModel ViewModel { get => viewModel; }
        private FrameBufferPool FBP;
        public RenderControl()
        {
            InitializeComponent();
            GLWpfControlSettings settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 0
            };
            OpenTKControl.Start(settings);

            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            viewModel = new DocumentViewModel();
            viewModel.CurrentDocument = DocumentFactory.CreateDocument("Untitled", 480, 640);
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void OpenTKControl_Loaded(object sender, RoutedEventArgs e)
        {
            camera = new Camera((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight, ViewModel.CurrentDocument.Height * 1.2);
            FBP = new FrameBufferPool(8, viewModel.CurrentDocument.Width, viewModel.CurrentDocument.Height, TextureType.FloatValue);

            AssetsManager.LoadPipeline("CurveToTexture", "shaders/fullscreenQuad.vsh", "shaders/curveToTexture.fsh");
            AssetsManager.LoadPipeline("Coloring", "shaders/documentQuad.vsh", "shaders/floatTextureColoring.fsh");
            AssetsManager.LoadPipeline("DocumentBackground", "shaders/documentQuad.vsh", "shaders/fullscreenColoring.fsh");

            dummyVAO = GL.GenVertexArray();

            initialized = true;
        }
        private void OpenTKControl_Update(TimeSpan deltaTime)
        {
            if (!initialized)
                return;

            render(deltaTime);
        }
        private void render(TimeSpan deltaTime)
        {
            FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);
            GL.ClearColor(Color4.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            renderDocumentViewModel(viewModel);
        }
        private void renderDocumentViewModel(DocumentViewModel document)
        {
            renderDocumentBackground(document.CurrentDocument, Logic.Color.White);

            foreach (IVisualGeometry vg in viewModel.CurrentDocument.VisualGeometries)
            {
                if (vg.Geometry is IFigure)
                {
                    FrameBuffer fbo = FBP.Get();
                    fbo.Use();

                    GL.Disable(EnableCap.Blend);
                    renderFigure(vg.Geometry as IFigure);

                    GL.Enable(EnableCap.Blend);
                    FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);
                    renderFloatTextureWithBrush(fbo.ColorTexture, vg.BackgroundBrush);
                    FBP.Release(fbo);
                }
                else
                    throw new NotImplementedException("Non-figures are not implemented yet.");
            }
        }
        private void renderDocumentBackground(IDocument document, Logic.Color color)
        {
            Pipeline background = AssetsManager.Pipelines["DocumentBackground"];
            background.Use();
            background.Uniform1("documentWidth", (float)document.Width);
            background.Uniform1("documentHeight", (float)document.Height);
            background.UniformMatrix3x3("view", (Matrix3x3f)camera.View);

            background.Uniform4("color", color.r / 255f, color.g / 255f, color.b / 255f, 1.0f);

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        private void renderFloatTextureWithBrush(Texture2D tex, IBrush brush)
        {
            Pipeline coloring = AssetsManager.Pipelines["Coloring"];
            coloring.Use();
            coloring.Uniform1("documentWidth", (float)viewModel.CurrentDocument.Width);
            coloring.Uniform1("documentHeight", (float)viewModel.CurrentDocument.Height);
            if (brush is Logic.SolidColorBrush)
            {
                Logic.SolidColorBrush scb = (Logic.SolidColorBrush)brush;
                coloring.Uniform4("color", scb.Color.r / 255f, scb.Color.g / 255f, scb.Color.b / 255f, (float)scb.Opacity);
            }
            else
                throw new NotImplementedException("Types other than SolidColorBrush are not implemented yet.");

            coloring.UniformMatrix3x3("view", (Matrix3x3f)camera.View);
            tex.Bind("tex");

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        private void renderFigure(IFigure figure)
        {
            Pipeline curveToTexture = AssetsManager.Pipelines["CurveToTexture"];
            curveToTexture.Use();
            curveToTexture.Uniform1("quadWidth", (float)viewModel.CurrentDocument.Width);
            curveToTexture.Uniform1("quadHeight", (float)viewModel.CurrentDocument.Height);

            int curvesCount = figure.Curves.ElementAt(0).Count;
            for (int i = 0; i < curvesCount; i++)
                curveToTexture.Uniform1($"curves[{i}].coeffs", Array.ConvertAll(figure.Curves.ElementAt(0).ElementAt(i), new Converter<double, float>(val => (float)val)));
            curveToTexture.Uniform1("curvesCount", curvesCount);
            curveToTexture.UniformMatrix3x3("curveView", (Matrix3x3f)figure.Transform.View);

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!IsFocused)
                Focus();
        }
        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point position = e.GetPosition(this);
            LinearAlgebra.Vector2 point = camera.ScreenToWorld(new LinearAlgebra.Vector2(position.X, position.Y));
            double delta = e.Delta > 0 ? this.ZoomDelta : 1 / this.ZoomDelta;

            camera.Zoom(point, delta);
        }

        private void OpenTKControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!initialized)
                return;

            camera.ScreenWidth = (int)e.NewSize.Width;
            camera.ScreenHeight = (int)e.NewSize.Height;
        }
    }
}
