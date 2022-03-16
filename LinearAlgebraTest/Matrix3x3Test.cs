using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Matrix3x3Test
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_123_321_213and134_212_431__171411_111417_161613returned()
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

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_123and134_212_731__17_14_11returned()
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

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_134_212_731and123__17_14_11returned()
        {
            //arrange
            Matrix3x3 MatX = new Matrix3x3(1, 3, 4,
                                           2, 1, 2,
                                           4, 3, 1);
            Vector3 VectY = new Vector3(1, 2, 3);

            Vector3 expected = new Vector3(19, 10, 13);

            //act
            Vector3 actual = MatX * VectY;

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_inverse_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void inverse_257_634_5m2m3__1m11_m3841m34_27m2924returned()
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
        public void invert_257_634_5m2m3__1m11_m3841m34_27m2924returned()
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
        public void transposed_257_634_5m2m3__265_53m2_74m3returned()
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
        public void transpose_257_634_5m2m3__265_53m2_74m3returned()
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
    }
}
