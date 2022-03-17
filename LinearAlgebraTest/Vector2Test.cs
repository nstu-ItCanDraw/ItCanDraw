using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearAlgebra.Test
{
    [TestClass]
    public class Vector2Test
    {

        /////////////////////////////////dot/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_2m3andm6m4_0returned()
        {
            //arrange
            Vector2 VectX = new Vector2(2, -3);
            Vector2 VectY = new Vector2(-6, -4);

            double expected = 0;

            //act
            double actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны
        [TestMethod]
        public void dot_12and22_6returned()
        {
            //arrange
            Vector2 VectX = new Vector2(1, 2);
            Vector2 VectY = new Vector2(2, 2);

            double expected = 6;

            //act
            double actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void dot_12and00_0returned()
        {
            //arrange
            Vector2 VectX = new Vector2(1, 2);
            Vector2 VectY = new Vector2(0, 0);

            double expected = 0;

            //act
            double actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////vecMul/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void vecMul_11and22_0returned()
        {
            //arrange
            Vector2 VectX = new Vector2(1, -3);
            Vector2 VectY = new Vector2(-6, -4);

            double expected = -22;

            //act
            double actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void vecMul_12and34_m2returned()
        {
            //arrange
            Vector2 VectX = new Vector2(1, 2);
            Vector2 VectY = new Vector2(3, 4);

            double expected = -2;

            //act
            double actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void vecMul_00and12_0returned()
        {
            //arrange
            Vector2 VectX = new Vector2(0, 0);
            Vector2 VectY = new Vector2(1, 2);

            double expected = 0;

            //act
            double actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////isColleniar/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не коллинеарны
        [TestMethod]
        public void isColleniarTo_12and21_freturned()
        {
            //arrange
            Vector2 VectX = new Vector2(1, 2);
            Vector2 VectY = new Vector2(2, 1);

            bool expected = false;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора коллинеарны и сонаправлены
        [TestMethod]
        public void isColleniarTo_32and96_treturned()
        {
            //arrange
            Vector2 VectX = new Vector2(3, 2);
            Vector2 VectY = new Vector2(9, 6);

            //double eps = Constants.Epsilon;
            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора коллинеарны и противоположно направлены
        [TestMethod]
        public void isColleniarTo_m24and3m6_treturned()
        {
            //arrange
            Vector2 VectX = new Vector2(-2, 4);
            Vector2 VectY = new Vector2(3, -6);

            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один вектор нулевой, второй не нулевой и всего его компоненты не нулевые
        [TestMethod]
        public void isColleniarTo_00and36_treturned()
        {
            //arrange
            Vector2 VectX = new Vector2(0, 0);
            Vector2 VectY = new Vector2(3, 6);

            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////projectOnVector/////////////////

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка не лежит на прямой
        [TestMethod]
        public void projectOnVector_1515and105_75returned()
        {
            //arrange
            Vector2 UnitX = new Vector2(15, 15);
            Vector2 UnitY = new Vector2(10, 5);

            Vector2 expected = new Vector2(7.5, 7.5);

            //act
            Vector2 actual = UnitY.projectOnVector(UnitX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка лежит на прямой
        [TestMethod]
        public void projectOnVector_36and24_24returned()
        {
            //arrange
            Vector2 UnitX = new Vector2(3, 6);
            Vector2 UnitY = new Vector2(2, 4);

            Vector2 expected = new Vector2(2, 4);

            //act
            Vector2 actual = UnitY.projectOnVector(UnitX);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
