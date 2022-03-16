using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Matrix2x2fTest
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23_12and21_12__78_45returned()
        {
            //arrange
            Matrix2x2f MatX = new Matrix2x2f(2f, 3f,
                                             1f, 2f);
            Matrix2x2f MatY = new Matrix2x2f(2f, 1f,
                                             1f, 2f);

            Matrix2x2f expected = new Matrix2x2f(7f, 8f,
                                                 4f, 5f);
            //act
            Matrix2x2f actual = MatX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_23and23_12__78_45returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(2f, 3f);
            Matrix2x2f MatY = new Matrix2x2f(2f, 3f,
                                             1f, 2f);

            Vector2f expected = new Vector2f(7f, 12f);
            //act
            Vector2f actual = VectX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Матрица и вектор не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult_21_12and23__78_45returned()
        {
            //arrange
            
            Matrix2x2f MatX = new Matrix2x2f(2f, 3f,
                                             1f, 2f);
            Vector2f VectY = new Vector2f(2f, 3f);

            Vector2f expected = new Vector2f(13f, 8f);
            //act
            Vector2f actual = MatX * VectY;

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_inverse_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void inverse_34_57___7m4_m53returned()
        {
            //arrange
            Matrix2x2f MatX = new Matrix2x2f(3f, 4f,
                                             5f, 7f);

            Matrix2x2f expected = new Matrix2x2f(7f, -4f,
                                                -5f, 3f);
            //act
            Matrix2x2f actual = MatX.inverse();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_invert_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void invert_34_57___7m4_m53returned()
        {
            //arrange
            Matrix2x2f MatX = new Matrix2x2f(3f, 4f,
                                             5f, 7f);

            Matrix2x2f expected = new Matrix2x2f(7f, -4f,
                                                 -5f, 3f);
            //act

            MatX.invert();
            Matrix2x2f actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transposed_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transposed_34_57__35_47returned()
        {
            //arrange
            Matrix2x2f MatX = new Matrix2x2f(3f, 4f,
                                             5f, 7f);


            Matrix2x2f expected = new Matrix2x2f(3f, 5f,
                                                 4f, 7f);
            //act
            Matrix2x2f actual = MatX.transposed();

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transpose_/////////////////////////////////
        
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transpose_34_57__35_47returned()
        {
            //arrange
            Matrix2x2f MatX = new Matrix2x2f(3f, 4f,
                                             5f, 7f);


            Matrix2x2f expected = new Matrix2x2f(3f, 5f,
                                               4f, 7f);
            //act
            MatX.transpose();
            Matrix2x2f actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);

        }
    }
}
