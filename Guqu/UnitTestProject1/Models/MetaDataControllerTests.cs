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
    public class MetaDataControllerTests
    {
        [TestMethod()]
        public void MetaDataControllerTest()
        {
            string root = "L://TestingFolder";
            try {
                MetaDataController controller = new MetaDataController(root);
                if (controller.GetType() == null)
                    Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.Fail(); 
            }
            
        }

        [TestMethod()]
        public void getMetaDataFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void addMetaDataFileTest()
        {
            Assert.Fail();
        }    
       

        [TestMethod()]
        public void addMetaDataFolderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void addCommonDescriptorFileTest()
        {
            //CommonDescriptor cd = new CommonDescriptor();
            
            Assert.Fail();
        }

        [TestMethod()]
        public void deleteCloudObjetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void removeFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getRootTest()
        {
            Assert.Fail();
        }
    }
}