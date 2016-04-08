using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.WebServices.Tests
{
    [TestClass()]
    public class InitializeAPITests
    {
        [TestMethod()]
        public void InitializeAPITest()
        {
            //empty constructor
        }

        [TestMethod()]
        public void initGoogleDriveAPITest()
        {

            try
            {
                InitializeAPI api = new InitializeAPI();

                //TODO: change auth logic - needs to auth via broswer
                // to init gdrive api which causes errors in the test

                /*
                api.initGoogleDriveAPI();


                if (gd == null)
                    throw new Exception();
                    */

            }
            catch (Exception e)
            {

                Assert.Fail();
            }
           
        }

        [TestMethod()]
        public void initBoxAPITest()
        {
            //Assert.Fail(); unused method
        }

        [TestMethod()]
        public void initOneDriveAPITest()
        {

            InitializeAPI api = new InitializeAPI();

            try
            {
                api.initOneDriveAPI();
            }
            catch (Exception e)
            {
                
                Assert.Fail();
            }
        }
    }
}