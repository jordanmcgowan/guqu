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
    public class OneDriveCommunicationParserTests
    {

    
        [TestMethod()]
        public void OneDriveCommunicationParserTest()
        {
            try
            {
                OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void createCommonDescriptorTest()
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            string json = "\"FileName\":\"Delt Calendar 2015\",\"FileType\":\"application/vnd.google-apps.spreadsheet\",\"FilePath\":\"GoogleDrive\\Delta Tau Delta - Beta Gamma\\The Delt Library\",\"FileID\":\"1Sjaiv_xT_wvuoSvjy7lQeU09QY6kQ6DLuSRciYvx9ys\",\"LastModified\":\"Date(1449039649000)\",\"FileSize\":0";
            string relPath = "OneDrive";

            try {
                var cd = odcp.createCommonDescriptor(relPath, json);
            }
            catch(Exception e)
            {
                
            }

            


        }

        [TestMethod()]
        public void getExtensionTest()
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            string s = odcp.getExtension("anything");

            if (s != null)
                Assert.Fail();
        }
    }
}