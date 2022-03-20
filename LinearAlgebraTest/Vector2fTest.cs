using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LinearAlgebra.Test
{
    [TestClass]
    public class Vector2fTest
    {

        /////////////////////////////////dot/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void dot_23andm6m4_0returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(2f, -3f);
            Vector2f VectY = new Vector2f(-6f, -4f);

            float expectedf = 0f;
            //act

            float actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);

        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны
        [TestMethod]
        public void dot_12and22_6returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(1f, 2f);
            Vector2f VectY = new Vector2f(2f, 2f);

            float expectedf = 6f;

            //act
            float actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void dot_12and00_0returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(1f, 2f);
            Vector2f VectY = new Vector2f(0f, 0f);

            float expectedf = 0f;

            //act
            float actualf = VectX.dot(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        /////////////////////////////////vecMul/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора перпендикулярны
        [TestMethod]
        public void vecMul_2m3andm6m4_m26returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(2f, -3f);
            Vector2f VectY = new Vector2f(-6f, -4f);

            float expectedf = -26f;

            //act
            float actualf = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не перпендикулярны
        [TestMethod]
        public void vecMul_12and34_m2returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(1f, 2f);
            Vector2f VectY = new Vector2f(3f, 4f);

            float expectedf = -2f;

            //act
            float actualf = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Один из векторов нулевой, второй вектор не нулевой, все его компоненты не нулевые
        [TestMethod]
        public void vecMul_00and12_0returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(0f, 0f);
            Vector2f VectY = new Vector2f(1f, 2f);

            float expectedf = 0f;

            //act
            float actualf = VectX.vecMul(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        /////////////////////////////////isColleniar/////////////////////////////////

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора не коллинеарны
        [TestMethod]
        public void isColleniarTo_12and21_freturned()
        {
            //arrange

            Vector2f VectX = new Vector2f(1f, 2f);
            Vector2f VectY = new Vector2f(2f, 1f);

            bool expectedf = false;

            //act
            bool actualf = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора коллинеарны и сонаправлены
        [TestMethod]
        public void isColleniarTo_32and96_treturned()
        {
            //arrange
            Vector2f VectX = new Vector2f(3f, 2f);
            Vector2f VectY = new Vector2f(9f, 6f);

            bool expectedf = true;

            //act
            bool actualf = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);

        }

        //Оба вектора не нулевые, все компоненты каждого вектора не нулевые, вектора коллинеарны и противоположно направлены
        [TestMethod]
        public void isColleniarTo_m24and3m6_treturned()
        {
            //arrange
            Vector2f VectX = new Vector2f(-2f, 4f);
            Vector2f VectY = new Vector2f(3f, -6f);

            bool expectedf = true;

            //act
            bool actualf = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        //Один вектор нулевой, второй не нулевой и всего его компоненты не нулевые
        [TestMethod]
        public void isColleniarTo_00and36_treturned()
        {
            //arrange
            Vector2f VectX = new Vector2f(0f, 0f);
            Vector2f VectY = new Vector2f(3f, 6f);

            bool expectedf = true;

            //act
            bool actualf = VectX.isCollinearTo(VectY);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

        /////////////////////////////////Vector2f_projectOnVector/////////////////

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка не лежит на прямой
        [TestMethod]
        public void projectOnVector_1515and105_75returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(15f, 15f);
            Vector2f VectY = new Vector2f(10f, 5f);

            Vector2f expectedf = new Vector2f(7.5f, 7.5f);

            //act
            Vector2f actualf = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expectedf, actualf);

        }

        //Направляющий вектор не нулевой, все его компоненты не нулевые, точка лежит на прямой
        [TestMethod]
        public void projectOnVector_36and24_24returned()
        {
            //arrange
            Vector2f VectX = new Vector2f(3f, 6f);
            Vector2f VectY = new Vector2f(2f, 4f);

            Vector2f expectedf = new Vector2f(2f, 4f);

            //act
            Vector2f actualf = VectY.projectOnVector(VectX);

            //assert
            Assert.AreEqual(expectedf, actualf);
        }

    }
}
