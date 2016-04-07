using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu;
namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            logInWindow winUT = new logInWindow();
            Assert.AreEqual(300, winUT.Height);
            Assert.AreEqual(300, winUT.Width);

        }
    }
}
