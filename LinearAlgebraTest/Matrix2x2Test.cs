using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Matrix2x2Test
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23_12and21_12__78_45returned()
        {
            //arrange
            Matrix2x2 MatX = new Matrix2x2(2, 3,
                                           1, 2);
            Matrix2x2 MatY = new Matrix2x2(2, 1,
                                           1, 2);

            Matrix2x2 expected = new Matrix2x2(7, 8,
                                               4, 5);
            //act
            Matrix2x2 actual = MatX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23and21_12__78returned()
        {
            //arrange
            Vector2 VectX = new Vector2(2, 3);
            Matrix2x2 MatY = new Matrix2x2(2, 3,
                                           1, 2);

            Vector2 expected = new Vector2(7, 12);
            //act
            Vector2 actual = VectX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23_12and23_78returned()
        {
            //arrange
            Matrix2x2 MatX = new Matrix2x2(2, 3,
                                           1, 2);
            Vector2 VectY = new Vector2(2, 3);

            Vector2 expected = new Vector2(13, 8);
            //act
            Vector2 actual = MatX * VectY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_inverse_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void inverse_34_57__7m4_m53returned()
        {
            //arrange
            Matrix2x2 MatX = new Matrix2x2(3, 4,
                                           5, 7);

            Matrix2x2 expected = new Matrix2x2(7, -4,
                                              -5, 3);
            //act
            Matrix2x2 actual = MatX.inverse();

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_invert_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void invert_34_57__7m4_m53returned()
        {
            //arrange
            Matrix2x2 MatX = new Matrix2x2(3, 4,
                                           5, 7);

            Matrix2x2 expected = new Matrix2x2(7, -4,
                                              -5, 3);
            //act
            MatX.invert();
            Matrix2x2 actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transposed_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transposed_34_57__35_47returned()
        {
            //arrange
            Matrix2x2 MatX = new Matrix2x2(3, 4,
                                           5, 7);


            Matrix2x2 expected = new Matrix2x2(3, 5,
                                               4, 7);
            //act
            Matrix2x2 actual = MatX.transposed();

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transpose_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transpose_34_57__35_47returned()
        {
            //arrange
            Matrix2x2 MatX = new Matrix2x2(3, 4,
                                           5, 7);


            Matrix2x2 expected = new Matrix2x2(3, 5,
                                               4, 7);
            //act
            MatX.transpose();
            Matrix2x2 actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
