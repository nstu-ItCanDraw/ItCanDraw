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

        #region click and drag control states
        private bool isMouseWheelDown = false;
        private bool isDragging = false;
        private LinearAlgebra.Vector2 mouseDragBase;
        private LinearAlgebra.Vector2 mouseDragVirtualBase;
        private const double mouseDragDelta = 5;
        #endregion
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
        private void OpenTKControl_Loaded(object sender, RoutedEventArgs e)
        {
            camera = new Camera((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight, ViewModel.CurrentDocument.Height * 1.2);
            FBP = new FrameBufferPool(8, viewModel.CurrentDocument.Width, viewModel.CurrentDocument.Height, TextureType.FloatValue);

            AssetsManager.LoadPipeline("CurveToTexture", "shaders/fullscreenQuad.vsh", "shaders/curveToTexture.fsh");
            AssetsManager.LoadPipeline("TextureUnion", "shaders/fullscreenQuad.vsh", "shaders/textureUnion.fsh");
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
            renderDocumentBackground(document.CurrentDocument);

            foreach (IVisualGeometry vg in viewModel.CurrentDocument.VisualGeometries)
            {
                if (vg.Geometry is IFigure)
                {
                    GL.Disable(EnableCap.Blend);

                    FrameBuffer fbo = FBP.Get();
                    renderFigureTo(fbo, vg.Geometry as IFigure);

                    GL.Enable(EnableCap.Blend);
                    renderFloatTextureWithBrushTo(null, fbo.ColorTexture, vg.BackgroundBrush);
                    FBP.Release(fbo);
                }
                else
                    throw new NotImplementedException("Non-figures are not implemented yet.");
            }
        }
        private void renderDocumentBackground(IDocument document)
        {
            Pipeline background = AssetsManager.Pipelines["DocumentBackground"];
            background.Use();
            background.Uniform1("documentWidth", (float)document.Width);
            background.Uniform1("documentHeight", (float)document.Height);
            background.UniformMatrix3x3("view", (Matrix3x3f)camera.View);

            background.Uniform4("color", document.BackgroundColor.r / 255f, document.BackgroundColor.g / 255f, document.BackgroundColor.b / 255f, 1.0f);

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        private void renderFloatTextureWithBrushTo(FrameBuffer fbo, Texture2D tex, IBrush brush)
        {
            if (fbo == null)
                FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);
            else
                fbo.Use();

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
        private void renderFigureTo(FrameBuffer fbo, IFigure figure)
        {
            int figuresCount = figure.Curves.Count;

            if (figuresCount == 1)
            {
                renderCurvesTo(fbo, figure.Curves.ElementAt(0), figure.Transform.View);
                return;
            }

            Stack<FrameBuffer> stack = new Stack<FrameBuffer>();

            for (int i = 0; i < figuresCount; i++)
            {
                FrameBuffer frameBuffer = FBP.Get();

                renderCurvesTo(frameBuffer, figure.Curves.ElementAt(i), figure.Transform.View);

                stack.Push(frameBuffer);
            }

            while (stack.Count > 1)
            {
                FrameBuffer result;
                if (stack.Count > 2)
                    result = FBP.Get();
                else
                    result = null;

                FrameBuffer source1 = stack.Pop();
                FrameBuffer source2 = stack.Pop();

                unionTexturesTo(result, source1.ColorTexture, source2.ColorTexture);

                FBP.Release(source1);
                FBP.Release(source2);

                stack.Push(result);
            }
        }
        private void unionTexturesTo(FrameBuffer fbo, Texture2D tex1, Texture2D tex2)
        {
            if (fbo == null)
                FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);
            else
                fbo.Use();

            Pipeline union = AssetsManager.Pipelines["TextureUnion"];
            union.Use();
            union.Uniform1("quadWidth", (float)viewModel.CurrentDocument.Width);
            union.Uniform1("quadHeight", (float)viewModel.CurrentDocument.Height);

            tex1.Bind("tex1");
            tex2.Bind("tex2");

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        private void renderCurvesTo(FrameBuffer fbo, IReadOnlyCollection<double[]> curves, Matrix3x3 view)
        {
            if (fbo == null)
                FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);
            else
                fbo.Use();

            Pipeline curveToTexture = AssetsManager.Pipelines["CurveToTexture"];
            curveToTexture.Use();
            curveToTexture.Uniform1("quadWidth", (float)viewModel.CurrentDocument.Width);
            curveToTexture.Uniform1("quadHeight", (float)viewModel.CurrentDocument.Height);
            curveToTexture.UniformMatrix3x3("curveView", (Matrix3x3f)view);

            int curvesCount = curves.Count;
            for (int i = 0; i < curvesCount; i++)
                curveToTexture.Uniform1($"curves[{i}].coeffs", Array.ConvertAll(curves.ElementAt(i), new Converter<double, float>(val => (float)val)));
            curveToTexture.Uniform1("curvesCount", curvesCount);

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
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

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            if (isMouseWheelDown)
            {
                Point mousePosPoint = e.GetPosition(this);
                LinearAlgebra.Vector2 mousePos = new LinearAlgebra.Vector2(mousePosPoint.X, mousePosPoint.Y);
                if (isDragging)
                    camera.Position += mouseDragVirtualBase - camera.ScreenToWorld(mousePos);
                else
                    if ((mouseDragBase - mousePos).squaredLength() >= mouseDragDelta * mouseDragDelta)
                        isDragging = true;
            }
        }
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            //if (!IsFocused)
            //    Focus();

            if (e.ChangedButton == MouseButton.Middle)
            {
                isMouseWheelDown = true;
                Point mousePos = e.GetPosition(this);
                mouseDragBase = new LinearAlgebra.Vector2(mousePos.X, mousePos.Y);
                mouseDragVirtualBase = camera.ScreenToWorld(mouseDragBase);
            }
        }
        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.ChangedButton == MouseButton.Middle)
            {
                isDragging = false;
                isMouseWheelDown = false;
            }
        }
        public void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        public void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
