using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Matrix3x3fTest
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23_12and21_12__78_45returned()
        {
            //arrange
            Matrix3x3f MatX = new Matrix3x3f(1f, 2f, 3f,
                                            3f, 2f, 1f,
                                            2f, 1f, 3f);
            Matrix3x3f MatY = new Matrix3x3f(1f, 3f, 4f,
                                           2f, 1f, 2f,
                                           4f, 3f, 1f);

            Matrix3x3f expected = new Matrix3x3f(17f, 14f, 11f,
                                               11f, 14f, 17f,
                                               16f, 16f, 13f);
            //act
            Matrix3x3 actual = MatX * MatY;

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_inverse_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void inverse_23_12__74_53returned()
        {
            Matrix3x3f MatX = new Matrix3x3f(2f, 5f, 7f,
                                           6f, 3f, 4f,
                                           5f, -2f, -3f);

            Matrix3x3f expected = new Matrix3x3f(1f, -1f, 1f,
                                              -38f, 41f, -34f,
                                               27f, -29f, 24f);
            //act
            Matrix3x3f actual = MatX.inverse();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_invert_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void invert_23_12___returned()
        {
            //arrange
            Matrix3x3f MatX = new Matrix3x3f(2f, 5f, 7f,
                                           6f, 3f, 4f,
                                           5f, -2f, -3f);

            Matrix3x3f expected = new Matrix3x3f(1f, -1f, 1f,
                                              -38f, 41f, -34f,
                                               27f, -29f, 24f);
            //act

            MatX.invert();
            Matrix3x3f actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transposed_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transposed_23_12__74_53returned()
        {
            //arrange
            Matrix3x3f MatX = new Matrix3x3f(2f, 5f, 7f,
                                           6f, 3f, 4f,
                                           5f, -2f, -3f);


            Matrix3x3f expected = new Matrix3x3f(2f, 6f, 5f,
                                               5f, 3f, -2f,
                                               7f, 4f, -3f);
            //act
            Matrix3x3f actual = MatX.transposed();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_transpose_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transpose_23_12__74_53returned()
        {
            //arrange
            Matrix3x3f MatX = new Matrix3x3f(2f, 5f, 7f,
                                           6f, 3f, 4f,
                                           5f, -2f, -3f);


            Matrix3x3f expected = new Matrix3x3f(2f, 6f, 5f,
                                               5f, 3f, -2f,
                                               7f, 4f, -3f);
            //act
            MatX.transpose();
            Matrix3x3f actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void and21_12__78_45returned()
        {
            //arrange
            Vector3f MatX = new Vector3f(1f, 2f, 3f);
            Matrix3x3f MatY = new Matrix3x3f(1f, 3f, 4f,
                                           2f, 1f, 2f,
                                           4f, 3f, 1f);

            Vector3f expected = new Vector3f(17f, 14f, 11f);
            //act
            Vector3f actual = MatX * MatY;

            //assert
            Assert.AreEqual(expected, actual);

        }

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void and21___78_45returned()
        {
            //arrange
            Vector3f MatX = new Vector3f(1f, 2f, 3f);
            Matrix3x3f MatY = new Matrix3x3f(1f, 3f, 4f,
                                           2f, 1f, 2f,
                                           4f, 3f, 1f);

            Vector3f expected = new Vector3f(17f, 14f, 11f);
            //act
            Vector3f actual = MatY * MatX;

            //assert
            Assert.AreEqual(expected, actual);

        }
    }
}

