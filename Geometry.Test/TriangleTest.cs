using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinearAlgebra;
using Geometry;
using System.Collections.Generic;

namespace Geometry.Test
{
    [TestClass]
    public class TriangleTest

    //////////////////////////////////CreateTriangle//////////////////////////////////
    {
        //Создается обычный треугольник 
        [TestMethod]
        public void CreateTriangle_5_7_00_creturn()
        {
            var expected = "Geometry.Triangle";
            var actual = FigureFactory.CreateTriangle(5, 7, new Vector2(0, 0)).ToString();
            Assert.AreEqual(expected, actual);
        }

        //Создается треугольник с минимальными данными
        [TestMethod]
        public void CreateTriangle_1E5_1E5_00_creturn()
        {
            var expected = "Geometry.Triangle";
            var actual = FigureFactory.CreateTriangle(1E-5, 1E-5, new Vector2(0, 0)).ToString();
            Assert.AreEqual(expected, actual);
        }

        //Исключение на width
        [ExpectedException(typeof(ArgumentException), "Triangle width must be greater or equal 1E-5.")]
        [TestMethod]
        public void CreateTriangle_1E6_1E5_00_ereturn()
        {
            var instance = FigureFactory.CreateTriangle(1E-6, 1E-5, new Vector2(0, 0));
        }

        //Исключение на height
        [ExpectedException(typeof(ArgumentException), "Triangle height must be greater or equal 1E-5.")]
        [TestMethod]
        public void CreateTriangle_1E5_1E6_00_ereturn()
        {
            var instance = FigureFactory.CreateTriangle(1E-5, 1E-6, new Vector2(0, 0));
        }

        //////////////////////////////////GetParameters//////////////////////////////////
        //width
        [TestMethod]
        public void GetParameters_10_7_00_widthreturn()
        {
            var expected = "10";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var actual = rect.GetParameters()["width"].ToString();
            Assert.AreEqual(expected, actual);
        }

        //height
        [TestMethod]
        public void GetParameters_5_7_00_heightreturn()
        {
            var expected = "7";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var actual = rect.GetParameters()["height"].ToString();
            Assert.AreEqual(expected, actual);
        }

        //name
        [TestMethod]
        public void GetTriangle_5_7_00_namereturn()
        {
            var expected = "triangle";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var actual = rect.GetParameters()["name"].ToString();
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////SetParameters//////////////////////////////////
        //width
        [TestMethod]
        public void SetParameters_10on5_7_00_0return()
        {
            var expected = "0";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var parametrs = new Dictionary<string, object>
            {
                { "width", 5 }
            };
            var actual = rect.SetParameters(parametrs).ToString();
            Assert.AreEqual(expected, actual);
        }

        //height
        [TestMethod]
        public void SetParameters_10_7on5_00_0return()
        {
            var expected = "0";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var parametrs = new Dictionary<string, object>
            {
                { "height", 5 }
            };
            var actual = rect.SetParameters(parametrs).ToString();
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////SetParameter//////////////////////////////////
        //width
        [TestMethod]
        public void SetParameter_10on5_7_00_0return()
        {
            var expected = "0";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var actual = rect.SetParameter("width", 5).ToString();
            Assert.AreEqual(expected, actual);
        }

        //height
        [TestMethod]
        public void SetParameter_10_7on5_00_0return()
        {
            var expected = "0";
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var actual = rect.SetParameter("height", 5).ToString();
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////TrySetParameter//////////////////////////////////
        //width
        [TestMethod]
        public void TrySetParameter_10on5_7_00_treturn()
        {
            bool expected = true;
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            bool actual = rect.TrySetParameter("width", 5);
            Assert.AreEqual(expected, actual);
        }

        //height
        [TestMethod]
        public void TrySetParameter_10_7on5_00_treturn()
        {
            bool expected = true;
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            bool actual = rect.TrySetParameter("height", 5);
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////TrySetParameters//////////////////////////////////
        //width
        [TestMethod]
        public void TrySetParameters_10on5_7_5_00_treturn()
        {
            bool expected = true;
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var parametrs = new Dictionary<string, object>
            {
                { "width", 5 }
            };
            bool actual = rect.TrySetParameters(parametrs);
            Assert.AreEqual(expected, actual);
        }

        //height
        [TestMethod]
        public void TrySetParameters_10_7on5_00_treturn()
        {
            bool expected = true;
            IGeometry rect = FigureFactory.CreateTriangle(10, 7, new Vector2(0, 0));
            var parametrs = new Dictionary<string, object>
            {
                { "height", 5 }
            };
            bool actual = rect.TrySetParameters(parametrs);
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////IsPointInFigure//////////////////////////////////
        [TestMethod]
        public void IsPointInFigure_10_5_00_12_treturn()
        {
            Vector2 point = new Vector2(1, 1);
            bool expected = true;
            double eps = 1E-5;
            IGeometry rect = FigureFactory.CreateTriangle(10, 5, new Vector2(0, 0));
            bool actual = rect.IsPointInFigure(point, eps);
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////UpdateAABB//////////////////////////////////
        //left_bottom
        [TestMethod]
        public void AABB_lb_return()
        {
            var rect = FigureFactory.CreateTriangle(10, 5, new Vector2(10, 10));
            rect.Transform.RotationDegrees = 90;
            var AABB = rect.AABB;
            var expected = new Vector2(7.5, 5);
            Assert.AreEqual(expected, AABB.left_bottom);
        }

        //right_top
        [TestMethod]
        public void AABB_rt_return()
        {
            var rect = FigureFactory.CreateTriangle(10, 5, new Vector2(10, 10));
            rect.Transform.RotationDegrees = 90;
            var AABB = rect.AABB;
            var expected = new Vector2(12.5, 15);
            Assert.AreEqual(expected, AABB.right_top);
        }

        //////////////////////////////////UpdateOBB//////////////////////////////////

        //left_bottom
        [TestMethod]
        public void OBB_lb_return()
        {
            var rect = FigureFactory.CreateTriangle(10, 5, new Vector2(10, 10));
            rect.Transform.RotationDegrees = 90;
            var OBB = rect.OBB;
            var expected = new Vector2(-5, -2.5);
            Assert.AreEqual(expected, OBB.left_bottom);
        }

        //right_top
        [TestMethod]
        public void OBB_rt_return()
        {
            var rect = FigureFactory.CreateTriangle(10, 5, new Vector2(10, 10));
            rect.Transform.RotationDegrees = 90;
            var OBB = rect.OBB;
            var expected = new Vector2(5, 2.5);
            Assert.AreEqual(expected, OBB.right_top);
        }
    }
}
