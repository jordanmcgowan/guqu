﻿using System;
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
        private string fileName, fileType, filePath;
        private DateTime lastModified;
        private long fileSize;

        public CommonDescriptor(string name, string fileType, string filePath, DateTime lastModified, long fileSize)
        {
            FileName = name;
            FileType = fileType;
            FilePath = filePath;
            LastModified = lastModified;
            FileSize = fileSize;
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