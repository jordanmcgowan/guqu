using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guqu.Models;

namespace Guqu.WebServices.Tests
{
    [TestClass()]
    public class GoogleDriveCallsTests
    {
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
            InitializeAPI api = new InitializeAPI();
            api.initGoogleDriveAPI();

            var gds = InitializeAPI.googleDriveService;

            CommonDescriptor cd = new CommonDescriptor();

            cd.FileName = "";
            cd.FileID = "";
            cd.FileType = "";

           
        }

        [TestMethod()]
        public void fetchAllMetaDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void shareFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void uploadFilesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void deleteFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void moveFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void copyFileTest()
        {
            Assert.Fail();
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
                    Assert.Fail();
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
                    Assert.Fail();
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
                    Assert.Fail();
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
                    Assert.Fail();
                }

            }
        }
    }
}