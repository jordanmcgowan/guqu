using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    /*
    * Object that holds the information for each metadata file. 
    */
    [Serializable]
    public class CommonDescriptor
    {
        private string fileName, fileType, filePath, fileID, accountType;
        private string[] owners;
        private DateTime lastModified;
        private long fileSize;

        public CommonDescriptor(string name, string fileType, string filePath, string fileID, string accountType, DateTime lastModified, long fileSize)
        {
            //TODO: store a isFile boolean, in the future save the fileType as 'vnd.applicaiton.google.folder' (or whatever), and set is file to true
            FileName = name;
            FileType = fileType;
            FilePath = filePath;
            LastModified = lastModified;
            FileSize = fileSize;
            FileID = fileID;
            AccountType = accountType;
            //Owners = owners;
        }
        public CommonDescriptor()
        {

        }
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public string FileType
        {
            get
            {
                return fileType;
            }

            set
            {
                fileType = value;
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }
        public string FileID
        {
            get
            {
                return fileID;
            }

            set
            {
                fileID = value;
            }
        }
        public string AccountType
        {
            get
            {
                return accountType;
            }
            set
            {
                accountType = value;
            }
        }
        public string[] Owners
        {
            get
            {
                return owners;
            }
            set
            {
                owners = value;
            }
        }
        public DateTime LastModified
        {
            get
            {
                return lastModified;
            }

            set
            {
                lastModified = value;
            }
        }

        public long FileSize
        {
            get
            {
                return fileSize;
            }

            set
            {
                fileSize = value;
            }
        }



    }
}
