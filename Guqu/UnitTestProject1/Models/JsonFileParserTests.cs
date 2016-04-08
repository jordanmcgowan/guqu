using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models.Tests
{
    [TestClass()]
    public class JsonFileParserTests
    {
        [TestMethod()]
        public void JsonFileParserTest()
        {
            try
            {
                JsonFileParser parser = new JsonFileParser();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void retrieveValuesTest()
        {
            Assert.Fail();
        }
    }
}