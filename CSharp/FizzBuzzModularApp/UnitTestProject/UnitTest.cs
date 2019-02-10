using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClassLibrary;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            FizzBuzzClass fb = new FizzBuzzClass();
            Assert.AreEqual("Fizz", fb.GetFizzBuzz(3));
            Assert.AreEqual("Fizz", fb.GetFizzBuzz(9));
            Assert.AreEqual("Buzz", fb.GetFizzBuzz(5));
            Assert.AreEqual("Buzz", fb.GetFizzBuzz(10));
            Assert.AreEqual("FizzBuzz", fb.GetFizzBuzz(15));
            Assert.AreEqual("1", fb.GetFizzBuzz(1));
        }

        [TestMethod]
        public void TestMethodCustomValues()
        {
            FizzBuzzClass fb = new FizzBuzzClass("F", "B");
            Assert.AreEqual("F", fb.GetFizzBuzz(3));
            Assert.AreEqual("F", fb.GetFizzBuzz(9));
            Assert.AreEqual("B", fb.GetFizzBuzz(5));
            Assert.AreEqual("B", fb.GetFizzBuzz(10));
            Assert.AreEqual("FB", fb.GetFizzBuzz(15));
            Assert.AreEqual("1", fb.GetFizzBuzz(1));
        }
    }
}
