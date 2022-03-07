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

namespace GUI
{
    /// <summary>
    /// Interaction logic for RenderControl.xaml
    /// </summary>
    public partial class RenderControl : UserControl
    {

        private int dummyVAO = 0;
        private bool initialized = false;
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
            AssetsManager.LoadPipeline("CurveToTexture", "shaders/fullscreenQuad.vsh", "shaders/curveToTexture.fsh");

            dummyVAO = GL.GenVertexArray();

            initialized = true;
        }
        private void OpenTKControl_Update(TimeSpan deltaTime)
        {
            if (!initialized)
                return;

            Render(deltaTime);
        }
        private void Render(TimeSpan deltaTime)
        {
            GL.ClearColor(Color4.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Pipeline curveToTexture = AssetsManager.Pipelines["CurveToTexture"];
            curveToTexture.Use();
            curveToTexture.Uniform1("coeffs", new float[6] { 0f, 2f, 0f, 0f, 0f, -0.5f });

            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!IsFocused)
                Focus();
        }
    }
}
