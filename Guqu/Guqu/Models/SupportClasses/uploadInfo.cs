using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models.SupportClasses
{
    class UploadInfo
    {
        Stream fileToUpload;
        string fileName;
        public UploadInfo(Stream fileToUpload, string fileName)
        {
            this.fileToUpload = fileToUpload;
            this.fileName = fileName;
        }
        public Stream getFileStream()
        {
            return fileToUpload;
        }
        public string getFileName()
        {
            return fileName;
        }
    }
}
