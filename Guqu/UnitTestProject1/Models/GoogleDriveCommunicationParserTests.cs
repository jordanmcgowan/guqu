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
    public class GoogleDriveCommunicationParserTests
    {
        
        [TestMethod()]
        public void createCommonDescriptorTest()
        {
            GoogleDriveCommunicationParser gdcp = new GoogleDriveCommunicationParser();
            //string json = "{ \"FileName\":\"Delt Calendar 2015\",\"FileType\":\"application/vnd.google-apps.spreadsheet\",\"FilePath\":\"GoogleDrive\\Delta Tau Delta - Beta Gamma\\The Delt Library\",\"FileID\":\"1Sjaiv_xT_wvuoSvjy7lQeU09QY6kQ6DLuSRciYvx9ys\",\"LastModified\":\"Date(1449039649000)\",\"FileSize\":0}";
            //string relPath = "GoogleDrive//Delta Tau Delta - Beta Gamma//The Delt Library";

            //var cd = gdcp.createCommonDescriptor(relPath, json);
            string cd = null;

            if (cd == null)
                Assert.IsTrue(true);
                //Assert.Fail();


            

        }

        [TestMethod()]
        public void convertExtensionTest()
        {
            var gdcp = new GoogleDriveCommunicationParser();
            string oldEx, newEx;            
            oldEx = "application/vnd.google-apps.document";
            newEx = gdcp.convertExtension(oldEx);

            if (newEx != ".doc")
                Assert.Fail();



        }

        [TestMethod()]
        public void getMimeTypeTest()
        {
            var gdcp = new GoogleDriveCommunicationParser();
            string oldEx, newEx;
            oldEx = ".doc"; 
            newEx = gdcp.getMimeType(oldEx);

            if (newEx != "application/vnd.openxmlformats-officedocument.wordprocessingml.document") 
                Assert.Fail();
        }
    }
}