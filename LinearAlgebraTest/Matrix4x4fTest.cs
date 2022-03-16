using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Matrix4x4fTest
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_1222_3112_3112_4251and1234_4321_3223_1122__17141516_12131720_12131720_28252835returned()
        {
            //arrange
            Matrix4x4f MatX = new Matrix4x4f(1f, 2f, 2f, 2f,
                                             3f, 1f, 1f, 2f,
                                             3f, 1f, 1f, 2f,
                                             4f, 2f, 5f, 1f);
            Matrix4x4f MatY = new Matrix4x4f(1f, 2f, 3f, 4f,
                                             4f, 3f, 2f, 1f,
                                             3f, 2f, 2f, 3f,
                                             1f, 1f, 2f, 2f);

            Matrix4x4f expected = new Matrix4x4f(17f, 14f, 15f, 16f,
                                                 12f, 13f, 17f, 20f,
                                                 12f, 13f, 17f, 20f,
                                                 28f, 25f, 28f, 35f);
            //act
            Matrix4x4 actual = MatX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Матрица и вектор не нулевые, все их компоненты не нулевыее
        [TestMethod]
        public void mult_1222and1234_4321_3223_1122__17141516returned()
        {
            //arrange
            Vector4f VectX = new Vector4f(1f, 2f, 2f, 2f);
            Matrix4x4f MatY = new Matrix4x4f(1f, 2f, 3f, 4f,
                                             4f, 3f, 2f, 1f,
                                             3f, 2f, 2f, 3f,
                                             1f, 1f, 2f, 2f);

            Vector4f expected = new Vector4f(17f, 14f, 15f, 16f);
            //act


            Vector4f actual = VectX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_1234_4321_3223_1122and1222__17141516returned()
        {
            //arrange
            Matrix4x4f MatX = new Matrix4x4f(1f, 2f, 3f, 4f,
                                             4f, 3f, 2f, 1f,
                                             3f, 2f, 2f, 3f,
                                             1f, 1f, 2f, 2f);
            Vector4f VectY = new Vector4f(1f, 2f, 2f, 2f);

            Vector4f expected = new Vector4f(19f, 16f, 17f, 11f);

            //act
            Vector4f actual = MatX * VectY;

            //assert
            Assert.AreEqual(expected, actual);
        }
        /////////////////////////////////_inverse_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void inverse_1232_2312_111m1_1m1m2m8__m115m252465_9525m2055_m05m052m05_m25m055m15returned()
        {
            Matrix4x4f MatX = new Matrix4x4f(1f, 2f, 3f, 2f,
                                             2f, 3f, 1f, 2f,
                                             1f, 1f, 1f, -1f,
                                             1f, -1f, -2f, -8f);

            Matrix4x4f expected = new Matrix4x4f(-11.5f, -2.5f, 24f, -6.5f,
                                                  9.5f, 2.5f, -20f, 5.5f,
                                                 -0.5f, -0.5f, 2f, -0.5f,
                                                 -2.5f, -0.5f, 5f, -1.5f);
            //act
            Matrix4x4f actual = MatX.inverse();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_invert_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void invert__1232_2312_111m1_1m1m2m8__m115m252465_9525m2055_m05m052m05_m25m055m15returned()
        {
            //arrange
            Matrix4x4f MatX = new Matrix4x4f(1f, 2f, 3f, 2f,
                                             2f, 3f, 1f, 2f,
                                             1f, 1f, 1f, -1f,
                                             1f, -1f, -2f, -8f);

            Matrix4x4f expected = new Matrix4x4f(-11.5f, -2.5f, 24f, -6.5f,
                                                  9.5f, 2.5f, -20f, 5.5f,
                                                 -0.5f, -0.5f, 2f, -0.5f,
                                                 -2.5f, -0.5f, 5f, -1.5f);
            //act

            MatX.invert();
            Matrix4x4f actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transposed_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transposed_1232_2312_111m1_1m1m2m8__1211_231m1_311m2_22m1m8returned()
        {
            //arrange
            Matrix4x4f MatX = new Matrix4x4f(1f, 2f, 3f, 2f,
                                             2f, 3f, 1f, 2f,
                                             1f, 1f, 1f, -1f,
                                             1f, -1f, -2f, -8f);

            Matrix4x4f expected = new Matrix4x4f(1f, 2f, 1f, 1f,
                                                 2f, 3f, 1f, -1f,
                                                 3f, 1f, 1f, -2f,
                                                 2f, 2f, -1f, -8f);
            //act
            Matrix4x4f actual = MatX.transposed();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_transpose_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transpose_1232_2312_111m1_1m1m2m8__1211_231m1_311m2_22m1m8returned()
        {
            //arrange
            Matrix4x4f MatX = new Matrix4x4f(1f, 2f, 3f, 2f,
                                             2f, 3f, 1f, 2f,
                                             1f, 1f, 1f, -1f,
                                             1f, -1f, -2f, -8f);

            Matrix4x4f expected = new Matrix4x4f(1f, 2f, 1f, 1f,
                                                 2f, 3f, 1f, -1f,
                                                 3f, 1f, 1f, -2f,
                                                 2f, 2f, -1f, -8f);
            //act
            MatX.transpose();
            Matrix4x4f actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);

        }
    }
}

