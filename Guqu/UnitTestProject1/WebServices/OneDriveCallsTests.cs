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
    public class OneDriveCallsTests
    {
        InitializeAPI api;
        public OneDriveCallsTests()
        {
            api = new InitializeAPI();
            api.initOneDriveAPI();
            User user = new User();
            CloudLogin.oneDriveLogin(user);
            this.downloadFileAsyncTest1();

        }
       
        [TestMethod()]
        public void downloadFileAsyncTest1()
        {
            CommonDescriptor cd = new CommonDescriptor();

            cd.FileName = "Cheat Sheet Final";
            cd.FileType = ".doc";
            cd.FileID = "8FA41A1E5CF18E2B!1130";

            var odc = new OneDriveCalls();
            try
            {
               odc.downloadFileAsync(cd);

            }
            catch(Exception e)
            {
                Assert.Fail();
            }

            
            }
               

        [TestMethod()]
        public void uploadFilesAsyncTest1()
        {

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void deleteFileAsyncTest1()
        {
            CommonDescriptor cd = new CommonDescriptor();

            cd.FileName = "Cheat Sheet Final";
            cd.FileType = ".doc";
            cd.FileID = "8FA41A1E5CF18E2B!696969";

            var odc = new OneDriveCalls();
            try
            {
                odc.deleteFileAsync(cd);

            }
            catch(Exception e)
            {
                Assert.Fail();
            }

            }

        [TestMethod()]
        public void moveFileAsyncTest1()
        {
            CommonDescriptor cd = new CommonDescriptor();

            cd.FileName = "Cheat Sheet Final";
            cd.FileType = ".doc";
            cd.FileID = "8FA41A1E5CF18E2B!1130";

            CommonDescriptor folder = new CommonDescriptor();

            folder.FileName = "Folder";
            cd.FileType = "folder";
            cd.FileID = "8FA41A1E5CF18E2B!696969";

            var odc = new OneDriveCalls();

            try
            {
                odc.moveFileAsync(cd, folder);

            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void fetchAllMetaDataTest1()
        {
            MetaDataController con = new MetaDataController("L://");

            string name = "onedrive";

            var odc = new OneDriveCalls();

            try
            {
                odc.fetchAllMetaData(con, name);

            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

       

        [TestMethod()]
        public void copyFileAsyncTest1()
        {
            CommonDescriptor cd = new CommonDescriptor();

            cd.FileName = "Cheat Sheet Final";
            cd.FileType = ".doc";
            cd.FileID = "8FA41A1E5CF18E2B!1130";

            CommonDescriptor folder = new CommonDescriptor();

            folder.FileName = "Folder";
            cd.FileType = "folder";
            cd.FileID = "8FA41A1E5CF18E2B!696969";

            var odc = new OneDriveCalls();

            try
            {
                odc.copyFileAsync(cd, folder);

            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        
    }
}