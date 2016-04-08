using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.OneDrive.Sdk;
using System;

namespace Guqu.WebServices.Tests
{
    [TestClass()]
    public class CloudLoginTests
    {

        //TODO: find out how to test things that need to be authenticated! 
       [TestMethod()]
        public void googleDriveLoginTest()
        {
            try
            {

                InitializeAPI api = new InitializeAPI();
            var s = api.initGoogleDriveAPI();
            
                var _googleDriveService = InitializeAPI.googleDriveService;
                
            }
            catch(Exception e)
            {
                Assert.Inconclusive();
            }
            
        }

        [TestMethod()]
        public void oneDriveLoginTest()
        {
            InitializeAPI api = new InitializeAPI();

            try
            {
                api.initOneDriveAPI();
                var _oneDriveClient = InitializeAPI.oneDriveClient;
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }
    }
}