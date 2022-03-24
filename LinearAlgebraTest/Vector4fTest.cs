using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Vector4fTest
    {
        /////////////////////////////////dot/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_231m1and32m93_0returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(2f, 3f, 1f, -1f);
            Vector4f VectY = new Vector4f(3f, 2f, -9f, 3f);

            float expected = 0;

            //act
            float actual = VectX.dotMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);

        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора неперпендикулярны
        [TestMethod]
        public void dot_231m1and31m93_m3returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(2, 3, 1, -1);
            Vector4f VectY = new Vector4f(3, 1, -9, 3);

            float expected = -3;

            //act
            float actual = VectX.dotMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_231m1and0000_0returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(2f, 3f, 1f, -1f);
            Vector4f VectY = new Vector4f(0f, 0f, 0f, 0f);

            float expectedf = 0f;

            //act
            float actualf = VectX.dotMul(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        /////////////////////////////////projectOnVector////////////////////////////////////


        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка лежит на прямой
        [TestMethod]
        public void projectOnVector_6666and3333_3333returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(6f, 6f, 6f, 6f);
            Vector4f VectY = new Vector4f(3f, 3f, 3f, 3f);

            Vector4f expected = new Vector4f(3f, 3f, 3f, 3f);

            //act
            Vector4f actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка не лежит на прямой
        [TestMethod]
        public void projectOnVector_6666and3223_252525returned()
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

        /////////////////////////////////projectOnFlat///////////////////////

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_1m1m22and3333_3333returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(1f, -1f, -2f, 2f);
            Vector4f VectY = new Vector4f(3f, 3f, 3f, 3f);

            Vector4f expected = new Vector4f(3f, 3f, 3f, 3f);

            //act
            Vector4f actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_1m1m22and2332_23272426returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(1f, -1f, -2f, 2f);
            Vector4f VectY = new Vector4f(2f, 3f, 3f, 2f);

            Vector4f expected = new Vector4f(2.3f, 2.7f, 2.4f, 2.6f);

            //act
            Vector4f actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
