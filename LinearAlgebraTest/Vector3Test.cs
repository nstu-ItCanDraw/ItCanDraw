using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Vector3Test
    {

        /////////////////////////////////dot/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_321and31m9_0returned()
        {
            //arrange
            Vector3 VectX = new Vector3(2, 3, 1);
            Vector3 VectY = new Vector3(3, 1, -9);

            double expectedf = 0;
            //act

            double actualf = VectX.dot(VectY);
            //assert

            Assert.AreEqual(expectedf, actualf);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны
        [TestMethod]
        public void dot_121and212_6returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 2, 1);
            Vector3 VectY = new Vector3(2, 1, 2);

            double expected = 6;

            //act
            double actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void dot_121and000_0returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 2, 1);
            Vector3 VectY = new Vector3(0, 0, 0);

            double expected = 0;

            //act
            double actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }


        /////////////////////////////////vecMul/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void vecMul_231and31m9_m2821m7returned()
        {
            //arrange
            Vector3 VectX = new Vector3(2, 3, 1);
            Vector3 VectY = new Vector3(3, 1, -9);

            Vector3 expected = new Vector3(-28, 21, -7);

            //act
            Vector3 actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны перпендикулярны
        [TestMethod]
        public void vecMul_113and223_m330returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 1, 3);
            Vector3 VectY = new Vector3(2, 2, 3);

            Vector3 expected = new Vector3(-3, 3, 0);

            //act
            Vector3 actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void vecMul_113and000_000returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 1, 3);
            Vector3 VectY = new Vector3(0, 0, 0);

            Vector3 expected = new Vector3(0, 0, 0);

            //act
            Vector3 actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////isColleniarTo/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не коллинеарны
        [TestMethod]
        public void isColleniarTo_123and213_freturned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 2, 3);
            Vector3 VectY = new Vector3(2, 1, 3);

            bool expected = false;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора коллинеарны и сонаправлены
        [TestMethod]
        public void isColleniarTo_222and333_treturned()
        {
            //arrange
            Vector3 VectX = new Vector3(2, 2, 2);
            Vector3 VectY = new Vector3(3, 3, 3);

            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора коллинеарны и противоположно направлены
        [TestMethod]
        public void isColleniarTo_222andm3m3m3_treturned()
        {
            //arrange
            Vector3 VectX = new Vector3(2, 2, 2);
            Vector3 VectY = new Vector3(-3, -3, -3);

            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один вектор нулевой, второй не нулевой и всего его компоненты не нулевые (нулевой вектор считается коллинеарным любому вектору)
        [TestMethod]
        public void isColleniarTo_222andm000_treturned()
        {
            //arrange
            Vector3 VectX = new Vector3(2, 2, 2);
            Vector3 VectY = new Vector3(0, 0, 0);

            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////projectOnVector/////////////////

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка лежит на прямой
        [TestMethod]
        public void projectOnVector_666and333_333returned()
        {
            //arrange
            Vector3 VectX = new Vector3(6, 6, 6);
            Vector3 VectY = new Vector3(3, 3, 3);

            Vector3 expected = new Vector3(3, 3, 3);

            //act
            Vector3 actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка не лежит на прямой
        [TestMethod]
        public void projectOnVector_101010and525_444returned()
        {
            //arrange
            Vector3 VectX = new Vector3(10, 10, 10);
            Vector3 VectY = new Vector3(5, 2, 5);

            Vector3 expected = new Vector3(4, 4, 4);

            //act
            Vector3 actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }


        /////////////////////////////////projectOnFlat/////////////////

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_11m2and333_333returned()
        {
            //arrange
            Vector3 VectX = new Vector3(1, 1, -2);
            Vector3 VectY = new Vector3(3, 3, 3);

            Vector3 expected = new Vector3(3, 3, 3);

            //act
            Vector3 actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_11m2and525_55254returned()
        {
            //arrange
            Vector3 UnitX = new Vector3(1, 1, -2);
            Vector3 UnitY = new Vector3(5, 2, 5);

            Vector3 expected = new Vector3(5.5, 2.5, 4);

            //act
            Vector3 actual = UnitY.projectOnFlat(UnitX);

            //assert
            Assert.AreEqual(expected, actual);
        }

    }
}
