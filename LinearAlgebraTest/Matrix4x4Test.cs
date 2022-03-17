using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Tests
{
    [TestClass]
    public class Matrix4x4Tests
    {

        /////////////////////////////////_*_/////////////////////////////////

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult__1222_3112_3112_4251and1234_4321_3223_1122__17141516_12131720_12131720_28252835returned()
        {
            //arrange
            Matrix4x4 MatX = new Matrix4x4(1, 2, 2, 2,
                                           3, 1, 1, 2,
                                           3, 1, 1, 2,
                                           4, 2, 5, 1);
            Matrix4x4 MatY = new Matrix4x4(1, 2, 3, 4,
                                            4, 3, 2, 1,
                                            3, 2, 2, 3,
                                            1, 1, 2, 2);

            Matrix4x4 expected = new Matrix4x4(17, 14, 15, 16,
                                               12, 13, 17, 20,
                                               12, 13, 17, 20,
                                               28, 25, 28, 35);
            //act
            Matrix4x4 actual = MatX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void mult__1222and1234_4321_3223_1122__17141516returned()
        {
            //arrange
            Vector4 VectX = new Vector4(1, 2, 2, 2);
            Matrix4x4 MatY = new Matrix4x4(1, 2, 3, 4,
                                           4, 3, 2, 1,
                                           3, 2, 2, 3,
                                           1, 1, 2, 2);

            Vector4 expected = new Vector4(17, 14, 15, 16);
            //act
            Vector4 actual = VectX * MatY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Обе матрицы не нулевые, все их компоненты не нулевые
        [TestMethod]
        public void multmult_1234_4321_3223_1122and1222__17141516returned()
        {
            //arrange
            
            Matrix4x4 MatX = new Matrix4x4(1, 2, 3, 4,
                                           4, 3, 2, 1,
                                           3, 2, 2, 3,
                                           1, 1, 2, 2);
            Vector4 VectY = new Vector4(1, 2, 2, 2);

            Vector4 expected = new Vector4(19, 16, 17, 11);
            //act
            Vector4 actual = MatX * VectY;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_inverse_/////////////////////////////////

        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void inverse1232_2312_111m1_1m1m2m8__m115m252465_9525m2055_m05m052m05_m25m055m15returned()
        {
            Matrix4x4 MatX = new Matrix4x4(1, 2, 3, 2,
                                           2, 3, 1, 2,
                                           1, 1, 1, -1,
                                           1, -1, -2, -8);

            Matrix4x4 expected = new Matrix4x4(-11.5, -2.5, 24, -6.5,
                                                9.5, 2.5, -20, 5.5,
                                               -0.5, -0.5, 2, -0.5,
                                               -2.5, -0.5, 5, -1.5);
            //act
            Matrix4x4 actual = MatX.inverse();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_invert_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не вырождена
        [TestMethod]
        public void invert1232_2312_111m1_1m1m2m8__m115m252465_9525m2055_m05m052m05_m25m055m15returned()
        {
            //arrange
            Matrix4x4 MatX = new Matrix4x4(1, 2, 3, 2,
                                           2, 3, 1, 2,
                                           1, 1, 1, -1,
                                           1, -1, -2, -8);

            Matrix4x4 expected = new Matrix4x4(-11.5, -2.5, 24, -6.5,
                                                9.5, 2.5, -20, 5.5,
                                               -0.5, -0.5, 2, -0.5,
                                               -2.5, -0.5, 5, -1.5);
            //act

            MatX.invert();
            Matrix4x4 actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////_transposed_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transposed_1232_2312_111m1_1m1m2m8__1211_231m1_311m2_22m1m8returned()
        {
            //arrange
            Matrix4x4 MatX = new Matrix4x4(1, 2, 3, 2,
                                           2, 3, 1, 2,
                                           1, 1, 1, -1,
                                           1, -1, -2, -8);

            Matrix4x4 expected = new Matrix4x4(1, 2, 1, 1,
                                               2, 3, 1, -1,
                                               3, 1, 1, -2,
                                               2, 2, -1, -8);
            //act
            Matrix4x4 actual = MatX.transposed();

            //assert
            Assert.AreEqual(expected, actual);

        }

        /////////////////////////////////_transpose_/////////////////////////////////
        //Матрица не нулевая, все её компоненты не нулевые, она не симметрична
        [TestMethod]
        public void transpose_1232_2312_111m1_1m1m2m8__1211_231m1_311m2_22m1m8returned()
        {
            //arrange
            Matrix4x4 MatX = new Matrix4x4(1, 2, 3, 2,
                                           2, 3, 1, 2,
                                           1, 1, 1, -1,
                                           1, -1, -2, -8);

            Matrix4x4 expected = new Matrix4x4(1, 2, 1, 1,
                                               2, 3, 1, -1,
                                               3, 1, 1, -2,
                                               2, 2, -1, -8);
            //act
            MatX.transpose();
            Matrix4x4 actual = MatX;

            //assert
            Assert.AreEqual(expected, actual);
        }  
    }
}
