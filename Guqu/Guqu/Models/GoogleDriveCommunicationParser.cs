using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Guqu.Models
{
    class GoogleDriveCommunicationParser : ICommunicationParser
    {
        
        private static Dictionary<string, string> google_CD_Dictionary;
        public GoogleDriveCommunicationParser()
        {

            google_CD_Dictionary = new Dictionary<string, string>();
            google_CD_Dictionary.Add("title", "fileName");
            google_CD_Dictionary.Add("mimeType", "fileType");
            google_CD_Dictionary.Add("fileSize", "fileSize");
            google_CD_Dictionary.Add("modifiedDate", "lastModified");


            //TODO: pass in the relative filepath? The overall controller should have it.
            // commonDescriptor_google_Dictionary.Item["filePath"] = 


            //           private string fileName, fileType, filePath;
            // private DateTime lastModified;
            //private int fileSize;
        }

        //Required by interface
        public CommonDescriptor createCommonDescriptor(StreamReader fileStreamReader, string relativeFilePath)
        {
            Dictionary<string, string> mdValues = new Dictionary<string, string>();
            string curLine;
            string curElement;
            int firstIndex, secondIndex, beginValueIndex, endValueIndex;
            while((curLine = fileStreamReader.ReadLine()) != null)
            {
                //see if the line contains " "
                if (curLine.Contains('"'))
                {
                    firstIndex = curLine.IndexOf('"');
                    secondIndex = curLine.IndexOf('"', firstIndex);
                    //TODO: check for OBO
                    curElement = curLine.Substring(firstIndex, secondIndex - firstIndex);

                    //check to see if the element is required.
                    if (google_CD_Dictionary.ContainsValue(curElement))
                    {
                        //write the value to mdValues
                        beginValueIndex = curLine.IndexOf(':');
                        //TODO, add the value to the md valued dict. key = value from google_CD_dictionary, value is substring from beginValueIndex.
                        //mdValues.Add(google_CD_Dictionary.get)
                    }
                }
            }
            //to create a common descriptor object we need to find everything in the hashmap. 
            return null;
        }
        //Required by interface
        public string createUploadBody(ServiceDescriptor descriptor)
        {
            return null;
        }

    }
}
