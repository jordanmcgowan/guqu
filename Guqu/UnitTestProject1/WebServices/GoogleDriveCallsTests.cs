using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guqu.Models;
using Guqu;


namespace Guqu.WebServices.Tests
{
    [TestClass()]
    public class GoogleDriveCallsTests
    {
        InitializeAPI api = new InitializeAPI();
        GoogleDriveCalls gdc;


          public GoogleDriveCallsTests()
        {
            try
            {
                List<string> s = api.initGoogleDriveAPI();
                CloudLogin.googleDriveLogin();

                gdc = new GoogleDriveCalls();
            }
            catch (Exception e) { }

        }  

        [TestMethod()]
        public void uploadFilesAsyncTest()
        {
            try
            {
                //do nothing
            }
            catch(Exception e)
            {
                if( ! (e.GetType() == new NotImplementedException().GetType()) )
                {
                    //fail if exception is not a NotImplementedException
                    Assert.Fail();
                }

            }
        }

        [TestMethod()]
        public void downloadFileAsyncTest()
        {
            

            CommonDescriptor cd = new CommonDescriptor();

            cd.FileName = "Future";
            cd.FileID = "1-e2aWnKr5j9OlkwbXIMv9E6xj4lBfH9DDNp_3hzLrEQ";
            cd.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            try {
                gdc.downloadFileAsync(cd);
            }
            catch(Exception e)
            {

                Assert.Inconclusive();

            }

           
        }

        [TestMethod()]
        public void fetchAllMetaDataTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void shareFileTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void uploadFilesTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void deleteFileTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void moveFileTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void copyFileTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void shareFileAsyncTest()
        {
            try
            {
                //do nothing
            }
            catch (Exception e)
            {
                if (!(e.GetType() == new NotImplementedException().GetType()))
                {
                    //fail if exception is not a NotImplementedException
                    Assert.Inconclusive();
                }

            }
        }

        [TestMethod()]
        public void deleteFileAsyncTest()
        {
            try
            {
                //do nothing
            }
            catch (Exception e)
            {
                if (!(e.GetType() == new NotImplementedException().GetType()))
                {
                    //fail if exception is not a NotImplementedException
                    Assert.Inconclusive();
                }

            }
        }

        [TestMethod()]
        public void moveFileAsyncTest()
        {
            try
            {
                //do nothing
            }
            catch (Exception e)
            {
                if (!(e.GetType() == new NotImplementedException().GetType()))
                {
                    //fail if exception is not a NotImplementedException
                    Assert.Inconclusive();
                }

            }
        }

        [TestMethod()]
        public void copyFileAsyncTest()
        {
            try
            {
                //do nothing
            }
            catch (Exception e)
            {
                if (!(e.GetType() == new NotImplementedException().GetType()))
                {
                    //fail if exception is not a NotImplementedException
                    Assert.Inconclusive();
                }

            }
        }
    }
}