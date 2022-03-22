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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI
{
    /// <summary>
    /// Interaction logic for RenderControl.xaml
    /// </summary>
    public partial class RenderControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty;
        
        private Camera camera;
        private int dummyVAO = 0;
        private bool initialized = false;
        private readonly double ZoomDelta = 1.1;
        public Logic.Color SpaceBackgroundColor { get; set; } = new Logic.Color(60, 60, 60);
        internal DocumentViewModel ViewModel
        {
            get
            {
                return (DocumentViewModel) GetValue(ViewModelProperty);
            }

            private set
            {
                SetValue(ViewModelProperty, value);
            }
        }
        private FrameBufferPool FBP;

        #region click and drag control states
        private bool isMouseWheelDown = false;
        private LinearAlgebra.Vector2 mouseDragVirtualBase;
        #endregion

        static RenderControl()
        {
            ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(DocumentViewModel), typeof(RenderControl));
        }

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
            GL.LineWidth(2);

            ViewModel = new DocumentViewModel();
            /*ViewModel.CreateDocument();
            ViewModel.CreateDocument();

            IGeometry triangle = FigureFactory.CreateTriangle(100, 100, LinearAlgebra.Vector2.Zero);
            triangle.Transform.Position = new LinearAlgebra.Vector2(20, 40);
            triangle.Transform.RotationDegrees = 20;
            ViewModel.CurrentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(triangle));*/
        }
        private void OpenTKControl_Loaded(object sender, RoutedEventArgs e)
        {
            camera = new Camera((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight, ViewModel.CurrentDocument != null ? ViewModel.CurrentDocument.Height * 1.2 : OpenTKControl.ActualHeight);
            FBP = new FrameBufferPool(8, camera.ScreenWidth, camera.ScreenHeight, TextureType.FloatValue);

            AssetsManager.LoadPipeline("CurveToTexture", "shaders/documentQuad.vsh", "shaders/curveToTexture.fsh");
            AssetsManager.LoadPipeline("TextureUnion", "shaders/documentQuad.vsh", "shaders/textureUnion.fsh");
            AssetsManager.LoadPipeline("Coloring", "shaders/documentQuad.vsh", "shaders/floatTextureColoring.fsh");
            AssetsManager.LoadPipeline("DocumentBackground", "shaders/documentQuad.vsh", "shaders/flatColoring.fsh");
            AssetsManager.LoadPipeline("OBBquad", "shaders/OBBquad.vsh", "shaders/flatColoring.fsh");

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
            GL.ClearColor(SpaceBackgroundColor.r / 255f, SpaceBackgroundColor.g / 255f, SpaceBackgroundColor.b / 255f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (ViewModel.CurrentDocument != null)
                renderDocumentViewModel(ViewModel);
        }
        private void renderDocumentViewModel(DocumentViewModel document)
        {
            renderDocumentBackground(document.CurrentDocument);

            renderVisualGeometries(ViewModel.CurrentDocument.VisualGeometries);

            renderOBBs(ViewModel.SelectedVisualGeometries);
        }
        private void renderVisualGeometries(IReadOnlyList<IVisualGeometry> visualGeometries)
        {
            foreach (IVisualGeometry vg in visualGeometries)
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
        private void renderOBBs(IReadOnlyList<IVisualGeometry> visualGeometries)
        {
            GL.Disable(EnableCap.Blend);

            foreach (IVisualGeometry vg in visualGeometries)
                renderGeometryOBBto(null, vg.Geometry);
        }
        private void renderGeometryOBBto(FrameBuffer fbo, IGeometry geometry)
        {
            if (fbo == null)
                FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);
            else
                fbo.Use();

            Pipeline obbQuad = AssetsManager.Pipelines["OBBquad"];
            obbQuad.Use();
            obbQuad.Uniform2("bottomLeft", (Vector2f)geometry.OBB.left_bottom);
            obbQuad.Uniform2("topRight", (Vector2f)geometry.OBB.right_top);
            obbQuad.UniformMatrix3x3("view", (Matrix3x3f)camera.View);
            obbQuad.UniformMatrix3x3("model", (Matrix3x3f)geometry.Transform.Model);
            obbQuad.Uniform4("color", 1.0f, 0.0f, 0.0f, 1.0f);

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
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
            coloring.Uniform1("documentWidth", (float)ViewModel.CurrentDocument.Width);
            coloring.Uniform1("documentHeight", (float)ViewModel.CurrentDocument.Height);
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
            union.Uniform1("documentWidth", (float)ViewModel.CurrentDocument.Width);
            union.Uniform1("documentHeight", (float)ViewModel.CurrentDocument.Height);

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
            curveToTexture.Uniform1("documentWidth", (float)ViewModel.CurrentDocument.Width);
            curveToTexture.Uniform1("documentHeight", (float)ViewModel.CurrentDocument.Height);
            curveToTexture.UniformMatrix3x3("curveView", (Matrix3x3f)view);
            curveToTexture.UniformMatrix3x3("view", (Matrix3x3f)camera.View);

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

            FBP.Dispose();
            FBP = new FrameBufferPool(8, camera.ScreenWidth, camera.ScreenHeight, TextureType.FloatValue);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            if (isMouseWheelDown)
            {
                Point mousePosPoint = e.GetPosition(this);
                camera.Position += mouseDragVirtualBase - camera.ScreenToWorld(new LinearAlgebra.Vector2(mousePosPoint.X, mousePosPoint.Y));
            }
        }
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!IsFocused)
                Focus();

            if (e.ChangedButton == MouseButton.Middle)
            {
                isMouseWheelDown = true;
                Point mousePos = e.GetPosition(this);
                mouseDragVirtualBase = camera.ScreenToWorld(new LinearAlgebra.Vector2(mousePos.X, mousePos.Y));
            }
        }
        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            switch (e.ChangedButton)
            {
                case MouseButton.Middle:
                    isMouseWheelDown = false;
                    break;
                case MouseButton.Left:
                    Point mousePosPoint = e.GetPosition(this);
                    LinearAlgebra.Vector2 mousePos = camera.ScreenToWorld(new LinearAlgebra.Vector2(mousePosPoint.X, mousePosPoint.Y));
                    bool nothingHit = true;
                    foreach (IVisualGeometry vg in ViewModel.CurrentDocument.VisualGeometries)
                        if (vg.Geometry.IsPointInFigure(mousePos, 1e-2))
                        {
                            if (Keyboard.GetKeyStates(Key.LeftShift).HasFlag(KeyStates.Down))
                            {
                                if (ViewModel.IsVisualGeometrySelected(vg))
                                    ViewModel.DeselectVisualGeometry(vg);
                                else
                                    ViewModel.SelectVisualGeometry(vg);
                            }
                            else
                            {
                                ViewModel.ClearSelectedVisualGeometries();
                                ViewModel.SelectVisualGeometry(vg);
                            }
                            nothingHit = false;
                        }
                    if (nothingHit)
                        ViewModel.ClearSelectedVisualGeometries();
                    break;
            }
        }
        public void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            bool ctrlPressed = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control);
            bool ctrlShiftPresses = ctrlPressed && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift);

            if(ctrlShiftPresses)
            {
                switch(e.Key)
                {
                    case Key.S:
                        ViewModel.SaveAsCurrentDocument();
                        break;
                }

                return;
            }

            if(ctrlPressed)
            {
                switch (e.Key)
                {
                    case Key.N:
                        ViewModel.CreateDocument();
                        break;
                    case Key.O:
                        ViewModel.OpenDocument();
                        break;
                    case Key.S:
                        ViewModel.SaveCurrentDocument();
                        break;
                }

                return;
            }

            switch (e.Key)
            {
                case Key.Delete:
                    ViewModel.DeleteSelectedVisualGeometries();
                    break;
            }
        }
        public void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
