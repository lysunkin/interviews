using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        private const int BUFF_SIZE = 80;

        [TestMethod]
        public void TestMethod()
        {
            try
            {
                TestDataAggregator da = new TestDataAggregator();

                using (TestStream sr = new TestStream())
                {
                    while (sr.CanRead)
                    {
                        byte[] buffer = new byte[BUFF_SIZE];
                        int itemsRead = sr.Read(buffer, 0, buffer.Length);
                        for (int i = 0; i < itemsRead; i++)
                        {
                            if (buffer[i] != 0)
                                da.AddChar(Convert.ToChar(buffer[i]));
                        }
                    }
                }

                da.LogStatus();
            }
            catch (Exception e)
            {
                // any exception => fail
                Assert.Fail("Exception: " + e.Message);
            }
        }
    }
}
