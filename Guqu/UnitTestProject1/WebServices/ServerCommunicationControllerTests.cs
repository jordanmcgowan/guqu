using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuquMysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guqu.WebServices;

namespace GuquMysql.Tests
{
    [TestClass()]
    public class ServerCommunicationControllerTests
    {
        [TestMethod()]
        public void ServerCommunicationControllerTest()
        {
            try
            {
                ServerCommunicationController db = new ServerCommunicationController();
            }
            catch (ArgumentException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void InsertDeleteTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                db.Insert("users", "test@test.test", "hash", "salt");
                db.Delete("test@test.test");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void InsertDeleteNewUserCloudTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                db.InsertNewUserCloud(777, "token", 2, "token"); 
                db.DeleteUserCloud(777);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void doesUserCloudExistTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                bool testBool = db.doesUserCloudExist(778); //userCloud with user_cloud_id = 778 already added in DB for testing purpose
                Assert.AreEqual(testBool, true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void SelectUserTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                User testUser = db.SelectUser("dont@remove.me"); //user with email = 'dont@remove.me' already added in DB for testing purpose
                Assert.AreEqual(testUser.Email, "dont@remove.me");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void SelectUserCloudsTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                List<UserCloud> testUserCloudList = new List<UserCloud>();
                testUserCloudList = db.SelectUserClouds(778); //userCloud with user_cloud_id = 778 already added in DB for testing purpose
                Assert.AreEqual(testUserCloudList.Count > 0, true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void emailExistsTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                bool testBool = db.emailExists("dont@remove.me");
                Assert.AreEqual(testBool, true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void UpdateLastLoginTest()
        {
            ServerCommunicationController db = new ServerCommunicationController();
            try
            {
                db.UpdateLastLogin("dont@remove.me");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
    }
}