using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu;
using Guqu.WebServices;
namespace UnitTestProject
{
    [TestClass]
    public class Dimensions
    {
        [TestMethod]
        public void logInWindowDimensions()
        {
            logInWindow winUT = new logInWindow();
            Assert.AreEqual(300, winUT.Height);
            Assert.AreEqual(300, winUT.Width);
        }
    }
    //Simply tests if every window can be opened and close, if these fails you messed up good
    [TestClass]
    public class OpenAndCloseWindows
    {
        [TestMethod]
        public void mainWindowOpenAndClose()
        {
            User user = new User();
            MainWindow winUT = new MainWindow(user);
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
    // Not ready to test yet
    //    [TestMethod]
    //    public void changePasswordWindowOpenAndClose()
    //    {
    //        changePasswordWindow winUT = new changePasswordWindow();
    //        try
    //        {
    //            winUT.Show();
    //           winUT.Close();
    //        }
    //        catch (InvalidOperationException e)
    //        {
    //            Assert.Fail();
    //        }
    //    }
        [TestMethod]
        public void changePathWindowOpenAndClose()
        {
            changePathWindow winUT = new changePathWindow();
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void cloudLoginWindowOpenAndClose()
        {
            try
            {
                cloudLoginWindow winUT = new cloudLoginWindow("box");
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void cloudLoginWindowOpenAndClose2()
        {
            try
            {
                cloudLoginWindow winUT = new cloudLoginWindow("supaCoolCloudService");
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Only accepts box, oneDrive, or googleDrive as accountTypes");
            }
        }
        [TestMethod]
        public void cloudPickerWindowOpenAndClose()
        {
            User user = new User();
            cloudPicker winUT = new cloudPicker(user);
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void confirmationPromptOpenAndClose()
        {
            confirmationPrompt winUT = new confirmationPrompt();
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void createAccountWindowOpenAndClose()
        {
            createAccountWindow winUT = new createAccountWindow();
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void dynamicPromptOpenAndClose()
        {
            string[] arrayOfStrings = new string[3]{"this","that","junk"};
            dynamicPrompt winUT = new dynamicPrompt(arrayOfStrings);
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void loginWindowOpenAndClose()
        {
            logInWindow winUT = new logInWindow();
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void manageCloudAccountsWindowOpenAndClose()
        {
            User user = new User();
            manageCloudAccountsWindow winUT = new manageCloudAccountsWindow(user);
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void moveViewWindowOpenAndClose()
        {
            moveView winUT = new moveView();
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void shareWindowOpenAndClose()
        {
            shareWindow winUT = new shareWindow();
            try
            {
                winUT.Show();
                winUT.Close();
            }
            catch (InvalidOperationException e)
            {
                Assert.Fail();
            }
        }


    }

}
