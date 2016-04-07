using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Guqu.Models
{
    interface ICommunicationParser
    {
        /*
        *Create a commonDescriptor from the information available in the
        *original metadata file retrieved from the cloud storage servce.
        */
        CommonDescriptor createCommonDescriptor(string relativeFilePath, string jsonMetaDataFile);


        /*
        *Return a string to represent the extension to be used for downloading.
        *Input parameter is the Type of the file in question
        *For example, the method will return ".xcel",".pdf", etc etc
        
        string get(string fileType);
        */
    }
}
