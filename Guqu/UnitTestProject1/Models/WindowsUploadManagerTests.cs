﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guqu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models.Tests
{
    [TestClass()]
    public class WindowsUploadManagerTests
    {
        [TestMethod()]
        public void WindowsUploadManagerTest()
        {
            try
            {
               WindowsUploadManager wum  = new WindowsUploadManager();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void getUploadFilesTest()
        {
            try
            {
                WindowsUploadManager wum = new WindowsUploadManager();
                wum.getUploadFiles();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}