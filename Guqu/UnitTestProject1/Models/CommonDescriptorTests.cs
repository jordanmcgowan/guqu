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
    public class CommonDescriptorTests
    {
        [TestMethod()]
        public void CommonDescriptorTest()
        {
            string name, type, path, id;
            name = "name";
            type = "type";
            path = "path";
            DateTime date = DateTime.Now;
            long size = 1024; ;
            id = "id";

            CommonDescriptor con = new CommonDescriptor(name, type, path, id, date, size);
            CommonDescriptor cd = con;
            cd.FileName = name;
            cd.FileType = type;
            cd.FilePath = path;
            cd.LastModified = date;
            cd.FileSize = size;
            cd.FileID = id;

            Assert.AreEqual(cd, con);
            
        }

        
        
    }
}