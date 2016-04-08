using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Guqu.Models.Tests
{
    [TestClass()]
    public class MetaDataControllerTests
    {
        [TestMethod()]
        public void MetaDataControllerTest()
        {
            string root = "L://TestingFolder";
            try
            {
                MetaDataController controller = new MetaDataController(root);
                if (controller.GetType() == null)
                    Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void getMetaDataFileTest()
        {
            try
            {
                MetaDataController mc = new MetaDataController("\\");
                mc.getMetaDataFile("\\");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }


        }

        [TestMethod()]
        public void addMetaDataFileTest()
        {
            string root = "L://TestingFolder";
            MetaDataController mc = new MetaDataController(root);
            string testString = mc.addMetaDataFile("", "", "a*");
            Assert.AreEqual("a_", testString);
        }


        [TestMethod()]
        public void addMetaDataFolderTest()
        {

            string root = "L://TestingFolder";
            MetaDataController mc = new MetaDataController(root);
            string testString = mc.addMetaDataFolder("", "", "a*");
            Assert.AreEqual("a_", testString);
        }

        [TestMethod()]
        public void addCommonDescriptorFileTest()
        {
            try
            {
                string root = "L://TestingFolder";
                CommonDescriptor cd = new CommonDescriptor("name", "ftype", "fpath", "fid", new DateTime(1), 10);
                MetaDataController mc = new MetaDataController(root);
                mc.addCommonDescriptorFile(cd);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void deleteCloudObjetTest()
        {
            try
            {
                string root = "L://TestingFolder";
                CommonDescriptor cd = new CommonDescriptor("name", "ftype", "fpath", "fid", new DateTime(), 10);
                MetaDataController mc = new MetaDataController(root);
                mc.deleteCloudObjet(cd);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        // probably not great for code coverage
        public void removeFileTest()
        {
            string root = "L://TestingFolder";
            MetaDataController mc = new MetaDataController(root);
            try
            {
                mc.removeFile("", "");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void getRootTest()
        {
            try
            {
                string root = "L://TestingFolder";
                MetaDataController mc = new MetaDataController(root);
                mc.getRoot("");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}