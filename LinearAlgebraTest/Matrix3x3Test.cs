using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Matrix3x3Test
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23_12and21_12__78_45returned()
        {
            //arrange
            Matrix3x3 MatX = new Matrix3x3(1, 2, 3,
                                            3, 2, 1,
                                            2, 1, 3);
            Matrix3x3 MatY = new Matrix3x3(1, 3, 4,
                                           2, 1, 2,
                                           4, 3, 1);

            Matrix3x3 expected = new Matrix3x3(17, 14, 11,
                                               11, 14, 17,
                                               16, 16, 13);
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
            Matrix3x3 MatX = new Matrix3x3(2, 5, 7,
                                           6, 3, 4,
                                           5, -2, -3);

            Matrix3x3 expected = new Matrix3x3(1, -1, 1,
                                              -38, 41, -34,
                                               27, -29, 24);
            //act
            Matrix3x3 actual = MatX.inverse();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_invert_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void invert_23_12___returned()
        {
            //arrange
            Matrix3x3 MatX = new Matrix3x3(2, 5, 7,
                                           6, 3, 4,
                                           5, -2, -3);

            Matrix3x3 expected = new Matrix3x3(1, -1, 1,
                                              -38, 41, -34,
                                               27, -29, 24);
            //act

            MatX.invert();
            Matrix3x3 actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transposed_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transposed_23_12__74_53returned()
        {
            //arrange
            Matrix3x3 MatX = new Matrix3x3(2, 5, 7,
                                           6, 3, 4,
                                           5, -2, -3);


            Matrix3x3 expected = new Matrix3x3(2, 6, 5,
                                               5, 3, -2,
                                               7, 4, -3);
            //act
            Matrix3x3 actual = MatX.transposed();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_transpose_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transpose_23_12__74_53returned()
        {
            //arrange
            Matrix3x3 MatX = new Matrix3x3(2, 5, 7,
                                           6, 3, 4,
                                           5, -2, -3);


            Matrix3x3 expected = new Matrix3x3(2, 6, 5,
                                               5, 3, -2,
                                               7, 4, -3);
            //act
            MatX.transpose();
            Matrix3x3 actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void and21_12__78_45returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 2, 3);
            Matrix3x3 MatY = new Matrix3x3(1, 3, 4,
                                           2, 1, 2,
                                           4, 3, 1);

            Vector3 expected = new Vector3(17, 14, 11);
            //act
            Vector3 actual = VectX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void and21_2__78_45returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 2, 3);
            Matrix3x3 MatY = new Matrix3x3(1, 3, 4,
                                           2, 1, 2,
                                           4, 3, 1);

            Vector3 expected = new Vector3(17, 14, 11);
            //act
            Vector3 actual = MatY * VectX;

            //assert
            Assert.AreEqual(expected, actual);

        }
    }
}
