﻿using System;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for RenderControl.xaml
    /// </summary>
    public partial class RenderControl : UserControl
    {
        private IFigure polygon;

        private FrameBuffer FBO;
        private Camera camera;
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
            camera = new Camera((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight, 300);
            FBO = new FrameBuffer(new Texture2D(200, 200, TextureType.FloatValue));

            var list = new List<LinearAlgebra.Vector2>() { new LinearAlgebra.Vector2(-0.4, 0.1), new LinearAlgebra.Vector2(0.4, 0.5), new LinearAlgebra.Vector2(-0.4, 0.5) };

            polygon = FigureFactory.CreatePolygon(list);
            list.Add(new LinearAlgebra.Vector2(0, -0.4));
            polygon.BasicPoints = list;
            list = polygon.BasicPoints.ToList();
            list.Add(new LinearAlgebra.Vector2(0.4, 0.1));
            polygon.BasicPoints = list;
            list = polygon.BasicPoints.ToList();
            list.Add(new LinearAlgebra.Vector2(0, 0.3));
            polygon.BasicPoints = list;

            AssetsManager.LoadPipeline("CurveToTexture", "shaders/fullscreenQuad.vsh", "shaders/curveToTexture.fsh");
            AssetsManager.LoadPipeline("Coloring", "shaders/documentQuad.vsh", "shaders/coloring.fsh");

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
            FBO.Use();
            renderFigure(polygon);

            FrameBuffer.UseDefault((int)OpenTKControl.ActualWidth, (int)OpenTKControl.ActualHeight);

            GL.ClearColor(Color4.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Pipeline coloring = AssetsManager.Pipelines["Coloring"];
            coloring.Use();
            coloring.Uniform1("documentWidth", 200.0f);
            coloring.Uniform1("documentHeight", 200.0f);
            coloring.Uniform4("color", 0.0f, 0.0f, 0.0f, 1.0f);
            coloring.Uniform4("backgroundColor", 1.0f, 1.0f, 1.0f, 1.0f);
            coloring.UniformMatrix3x3("view", (Matrix3x3f)camera.View);
            FBO.ColorTexture.Bind("tex");
            
            GL.BindVertexArray(dummyVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        private void renderFigure(IFigure figure)
        {
            Pipeline curveToTexture = AssetsManager.Pipelines["CurveToTexture"];
            curveToTexture.Use();
            curveToTexture.Uniform1("quadWidth", 200.0f);
            curveToTexture.Uniform1("quadHeight", 200.0f);
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
    }
}
