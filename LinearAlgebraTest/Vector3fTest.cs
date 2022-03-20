using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearAlgebra.Test
{
    [TestClass]
    public class Vector3fTest
    {
        //public TestContext TestContext { get; set; }
        /////////////////////////////////Vector3f_dot/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_231and31m9_0returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(2f, 3f, 1f);
            Vector3f VectY = new Vector3f(3f, 1f, -9f);

            float expectedf = 0f;

            //act
            float actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны
        [TestMethod]
        public void dot_121and212_6returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(1f, 2f, 1f);
            Vector3f VectY = new Vector3f(2f, 1f, 2f);

            float expected = 6;

            //act
            float actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void dot_121and000_0returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(1f, 2f, 1f);
            Vector3f VectY = new Vector3f(0f, 0f, 0f);

            float expected = 0f;

            //act
            float actual = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }


        /////////////////////////////////Vector3f_vecMul/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void vecMul_231and31m9_m2821m7returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(2f, 3f, 1f);
            Vector3f VectY = new Vector3f(3f, 1f, -9f);

            Vector3f expected = new Vector3f(-28f, 21f, -7f);

            //act
            Vector3f actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны перпендикулярны
        [TestMethod]
        public void vecMul_113and223_m330returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(1f, 1f, 3f);
            Vector3f VectY = new Vector3f(2f, 2f, 3f);

            Vector3f expected = new Vector3f(-3f, 3f, 0f);

            //act
            Vector3f actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void vecMul_113and000_0returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(1f, 1f, 3f);
            Vector3f VectY = new Vector3f(0f, 0f, 0f);

            Vector3f expected = new Vector3f(0f, 0f, 0f);

            //act
            Vector3f actual = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////Vector3f_isColleniar/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не коллинеарны
        [TestMethod]
        public void isColleniarTo_123and213_freturned()
        {
            //arrange
            Vector3f VectX = new Vector3f(1f, 2f, 3f);
            Vector3f VectY = new Vector3f(2f, 1f, 3f);

            //double eps = Constants.Epsilon;
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
            Vector3f VectX = new Vector3f(2f, 2f, 2f);
            Vector3f VectY = new Vector3f(3f, 3f, 3f);

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
            Vector3f VectX = new Vector3f(2f, 2f, 2f);
            Vector3f VectY = new Vector3f(-3f, -3f, -3f);

            //double eps = Constants.Epsilon;
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
            Vector3f VectX = new Vector3f(2f, 2f, 2f);
            Vector3f VectY = new Vector3f(0f, 0f, 0f);

            //double eps = Constants.Epsilon;
            bool expected = true;

            //act
            bool actual = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expected, actual);
        }

        /////////////////////////////////Vector3f_projectOnVector/////////////////

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка лежит на прямой
        [TestMethod]
        public void projectOnVector_666and333_333returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(6f, 6f, 6f);
            Vector3f VectY = new Vector3f(3f, 3f, 3f);

            Vector3f expected = new Vector3f(3f, 3f, 3f);

            //act
            Vector3f actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка не лежит на прямой
        [TestMethod]
        public void projectOnVector_101010and525_444returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(10f, 10f, 10f);
            Vector3f VectY = new Vector3f(5f, 2f, 5f);

            Vector3f expected = new Vector3f(4f, 4f, 4f);

            //act
            Vector3f actual = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }


        /////////////////////////////////Vector3f_projectOnFlat/////////////////


        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит на плоскости
        [TestMethod]
        public void projectOnFlat_231and31m9_31m9returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(2f, 3f, 1f);
            Vector3f VectY = new Vector3f(3f, 1f, -9f);

            Vector3f expected = new Vector3f(3f, 1f, -9f);

            //act
            Vector3f actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //Нормаль не нулевая, все её компоненты не нулевые, точка лежит не на плоскости
        [TestMethod]
        public void projectOnFlat_231and427_1m2555returned()
        {
            //arrange
            Vector3f VectX = new Vector3f(2f, 3f, 1f);
            Vector3f VectY = new Vector3f(4f, 2f, 7f);

            Vector3f expected = new Vector3f(1f, -2.5f, 5.5f);

            //act
            Vector3f actual = VectY.projectOnFlat(VectX);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
