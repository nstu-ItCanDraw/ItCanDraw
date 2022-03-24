using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinearAlgebra;
using Geometry;
using System.Collections.Generic;

namespace Geometry.Test
{
    [TestClass]
    public class PolylineTets

    //////////////////////////////////CreatePolyline//////////////////////////////////
    {
        //Создается обычная ломаная 
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void CreatePolyline_00_11_creturn()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            var expected = "Geometry.Polyline";
            var actual = FigureFactory.CreatePolyline(points).ToString();
            Assert.AreEqual(expected, actual);
        }

        //Исключение
        [ExpectedException(typeof(ArgumentException), "Polyline must have 2 or more points.")]
        [TestMethod]
        public void CreatePolyline_11_ereturn()
        {
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { Y };
            var instance = FigureFactory.CreatePolyline(points);
        }

        //////////////////////////////////GetParameters//////////////////////////////////
        //points
        [TestMethod]
        public void GetParameters_00_11_pointsreturn()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            var expected = points.ToString();
            var rect = FigureFactory.CreatePolyline(points);
            var actual = rect.GetParameters()["points"].ToString();
            Assert.AreEqual(expected, actual);
            TestContext.WriteLine("Test {0}", actual);
        }

        //name
        [TestMethod]
        public void GetParameters_5_7_00_namereturn()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            var expected = "polyline";
            IGeometry rect = FigureFactory.CreatePolyline(points);
            var actual = rect.GetParameters()["name"].ToString();
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////SetParameters//////////////////////////////////
        //points
        [TestMethod]
        public void SetParameters_00_11_0return()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            var expected = "0";
            IGeometry rect = FigureFactory.CreatePolyline(points);
            var parametrs = new Dictionary<string, object>
            {
                { "points", points }
            };
            var actual = rect.SetParameters(parametrs).ToString();
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////SetParameter//////////////////////////////////
        //points
        [TestMethod]
        public void SetParameter_00_11_0return()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            var expected = "0";
            IGeometry rect = FigureFactory.CreatePolyline(points);
            var actual = rect.SetParameter("points", points).ToString();
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////TrySetParameter//////////////////////////////////
        //points
        [TestMethod]
        public void TrySetParameter_00_11_treturn()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            bool expected = true;
            IGeometry rect = FigureFactory.CreatePolyline(points);
            bool actual = rect.TrySetParameter("points", points);
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////TrySetParameters//////////////////////////////////
        //points
        [TestMethod]
        public void TrySetParameters_00_11_treturn()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(1, 1);
            var points = new List<Vector2>() { X, Y };
            bool expected = true;
            IGeometry rect = FigureFactory.CreatePolyline(points);
            var parametrs = new Dictionary<string, object>
            {
                { "points", points }
            };
            bool actual = rect.TrySetParameters(parametrs);
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////IsPointInFigure//////////////////////////////////
        [TestMethod]
        public void IsPointInFigure_00_22_11_treturn()
        {
            Vector2 X = new Vector2(0, 0);
            Vector2 Y = new Vector2(2, 2);
            var points = new List<Vector2>() { X, Y };
            Vector2 point = new Vector2(1, 1);
            bool expected = true;
            double eps = 1E-5;
            IGeometry rect = FigureFactory.CreatePolyline(points);
            bool actual = rect.IsPointInFigure(point, eps);
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////UpdateOBB//////////////////////////////////
        //left_bottom
        [TestMethod]
        public void AABB_lb_return()
        {
            Vector2 X = new Vector2(10, 10);
            Vector2 Y = new Vector2(12, 12);
            var points = new List<Vector2>() { X, Y };
            var rect = FigureFactory.CreatePolyline(points);
            rect.Transform.RotationDegrees = 90;
            var AABB = rect.AABB;
            var expected = new Vector2(10, 10);
            Assert.AreEqual(expected, AABB.left_bottom);
        }

        //right_top
        [TestMethod]
        public void AABB_rt_return()
        {
            Vector2 X = new Vector2(10, 10);
            Vector2 Y = new Vector2(12, 12);
            var points = new List<Vector2>() { X, Y };
            var rect = FigureFactory.CreatePolyline(points);
            rect.Transform.RotationDegrees = 90;
            var AABB = rect.AABB;
            var expected = new Vector2(12, 12);
            Assert.AreEqual(expected, AABB.right_top);
        }

        //////////////////////////////////UpdateOBB//////////////////////////////////

        //left_bottom
        [TestMethod]
        public void OBB_lb_return()
        {
            Vector2 X = new Vector2(10, 10);
            Vector2 Y = new Vector2(12, 12);
            var points = new List<Vector2>() { X, Y };
            var rect = FigureFactory.CreatePolyline(points);
            rect.Transform.RotationDegrees = 90;
            var OBB = rect.OBB;
            var expected = new Vector2(-1, -1);
            Assert.AreEqual(expected, OBB.left_bottom);
        }

        //right_top
        [TestMethod]
        public void OBB_rt_return()
        {
            Vector2 X = new Vector2(10, 10);
            Vector2 Y = new Vector2(12, 12);
            var points = new List<Vector2>() { X, Y };
            var rect = FigureFactory.CreatePolyline(points);
            rect.Transform.RotationDegrees = 90;
            var OBB = rect.OBB;
            var expected = new Vector2(1, 1);
            Assert.AreEqual(expected, OBB.right_top);
        }
    }
}
