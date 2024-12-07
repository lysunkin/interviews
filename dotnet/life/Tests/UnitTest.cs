using NUnit.Framework;
using Life.Board;

namespace Life.Tests
{
    public class Tests
    {
        private LifeBoard testBoard;

        [SetUp]
        public void SetUp()
        {
            testBoard = new LifeBoard(3, 3);
        }

        [Test]
        public void TestEmpty()
        {
            bool[,] test1 = { { false, false, false }, { false, false, false }, { false, false, false } };
            testBoard.SetBoard(test1);
            Assert.That(testBoard.ToString() == "...\n...\n...\n", "test1.1 failed");

            testBoard.Tick();
            Assert.That(testBoard.ToString() == "...\n...\n...\n", "test1.2 failed");
        }

        [Test]
        public void TestOvercrowded()
        {
            bool[,] test2 = { { true, true, true }, { true, true, true }, { true, true, true } };
            testBoard.SetBoard(test2);
            Assert.That(testBoard.ToString() == "XXX\nXXX\nXXX\n", "test2.1 failed");

            testBoard.Tick();
            Assert.That(testBoard.ToString() == "X.X\n...\nX.X\n", "test2.2 failed");

            testBoard.Tick();
            Assert.That(testBoard.ToString() == "...\n...\n...\n", "test2.3 failed");
        }
    }
}
