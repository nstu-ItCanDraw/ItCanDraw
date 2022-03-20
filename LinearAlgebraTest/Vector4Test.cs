using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Vector4Test
    {
        //public TestContext TestContext { get; set; }
        /////////////////////////////////dot/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_231m1and32m93_0returned()
        {
            //arrange
            Vector4 VectX = new Vector4(2, 3, 1, -1);
            Vector4 VectY = new Vector4(3, 2, -9, 3);

            double expectedf = 0;

            //act
            double actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);

        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора неперпендикулярны
        [TestMethod]
        public void dot_231m1and31m93_m3returned()
        {
            //arrange
            Vector4 VectX = new Vector4(2, 3, 1, -1);
            Vector4 VectY = new Vector4(3, 1, -9, 3);

            double expectedf = -3;

            //act
            double actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_231m1and0000_0returned()
        {
            //arrange
            Vector4 VectX = new Vector4(2, 3, 1, -1);
            Vector4 VectY = new Vector4(0, 0, 0, 0);

            double expectedf = 0;

            //act
            double actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        /////////////////////////////////projectOnVector/////////////////


        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка лежит на прямой
        [TestMethod]
        public void projectOnVector_6666and3333_3333returned()
        {
            //arrange
            Vector4 VectX = new Vector4(6, 6, 6, 6);
            Vector4 VectY = new Vector4(3, 3, 3, 3);

            Vector4 expected = new Vector4(3, 3, 3, 3);

            //act
            Vector4 actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка не лежит на прямой
        [TestMethod]
        public void projectOnVector_6666and3223_25252525returned()
        {
            //arrange
            Vector4 VectX = new Vector4(6, 6, 6, 6);
            Vector4 VectY = new Vector4(3, 2, 2, 3);

            Vector4 expected = new Vector4(2.5, 2.5, 2.5, 2.5);

            //act
            Vector4 actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////projectOnFlat/////////////////

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_1m1m22and3333_3333returned()
        {
            //arrange
            Vector4 VectX = new Vector4(1, -1, -2, 2);
            Vector4 VectY = new Vector4(3, 3, 3, 3);

            Vector4 expected = new Vector4(3, 3, 3, 3);

            //act
            Vector4 actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_1m1m22and2332_23272426returned()
        {
            //arrange
            Vector4 VectX = new Vector4(1, -1, -2, 2);
            Vector4 VectY = new Vector4(2, 3, 3, 2);

            Vector4 expected = new Vector4(2.3, 2.7, 2.4, 2.6);

            //act
            Vector4 actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
