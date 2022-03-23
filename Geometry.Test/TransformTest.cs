using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinearAlgebra;
using Geometry;
using System.Collections.Generic;

namespace Geometry.Test
{

    [TestClass]
    public class TransformTests

    //////////////////////////////////LocalPosition//////////////////////////////////
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void LocalPositin_return()
        {
            var expected = new Vector2(4, 6);
            Vector2 Local = new Vector2(2, 3);
            Transform a = new Transform();
            Transform b = new Transform();
            a.LocalPosition = Local;
            b.LocalPosition = Local;
            a.Parent = b;
            var actual = a.Position;
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////LocalRotation//////////////////////////////////
        [TestMethod]
        public void LocalRotation_return()
        {
            double expected = 45;
            double u = 0.7854;
            var eps = 1E-3;
            Transform a = new Transform();
            a.LocalRotation = u;
            var actual = a.LocalRotationDegrees;
            Assert.AreEqual(expected, actual, eps);
        }

        //////////////////////////////////LocalRotationDegrees//////////////////////////////////
        [TestMethod]
        public void LocalRotationDegrees_return()
        {
            double expected = 45;
            double u = 45;
            Transform a = new Transform();
            a.LocalRotation = u;
            var actual = a.LocalRotation;
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////LocalScale//////////////////////////////////
        [TestMethod]
        public void LocalScale_return()
        {
            Vector2 expected = new Vector2(1, 1);
            Vector2 Local = new Vector2(1, 1);
            Transform a = new Transform();
            a.LocalScale = Local;
            var actual = a.LocalScale;
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////Rotation//////////////////////////////////
        [TestMethod]
        public void Rotation_return()
        {
            double expected = 1.5708;
            double u = 1.5708;
            Transform a = new Transform();
            Transform b = new Transform();
            a.Rotation = u;
            b.Rotation = u;
            a.Parent = b;
            var actual = b.Rotation;
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////RotationDegrees//////////////////////////////////
        [TestMethod]
        public void RotationDegrees_return()
        {
            double expected = 90;
            double u = 0.7854;
            var eps = 1E-3;
            Transform a = new Transform();
            Transform b = new Transform();
            a.Rotation = u;
            b.Rotation = u;
            a.Parent = b;
            var actual = a.RotationDegrees;
            Assert.AreEqual(expected, actual, eps);
        }

        //////////////////////////////////LocalModel//////////////////////////////////
        [TestMethod]
        public void LocalModel_return()
        {
            var expected = new Matrix3x3(1, 0, 0,
                                         0, 1, 0,
                                         0, 0, 1);
            Transform a = new Transform();
            Matrix3x3 actual = a.LocalModel;
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////LocalView//////////////////////////////////
        [TestMethod]
        public void LocalView_return()
        {
            var expected = new Matrix3x3(1, 0, 0,
                                         0, 1, 0,
                                         0, 0, 1);
            Transform a = new Transform();
            Matrix3x3 actual = a.LocalView;
            Assert.AreEqual(expected, actual);
        }

        //////////////////////////////////Model//////////////////////////////////
        [TestMethod]
        public void Model_return()
        {
            var expected = new Matrix3x3(1, 0, 0,
                                         0, 1, 0,
                                         0, 0, 1);
            Transform a = new Transform();
            Matrix3x3 actual = a.Model;
            Assert.AreEqual(expected, actual);
            TestContext.WriteLine("Test {0}", actual);
        }

        //////////////////////////////////View//////////////////////////////////
        [TestMethod]
        public void View_return()
        {
            var expected = new Matrix3x3(1, 0, 0,
                                         0, 1, 0,
                                         0, 0, 1);
            Transform a = new Transform();
            Matrix3x3 actual = a.View;
            Assert.AreEqual(expected, actual);
            TestContext.WriteLine("Test {0}", actual);
        }

    }
}
