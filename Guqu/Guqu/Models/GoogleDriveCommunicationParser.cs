using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace Guqu.Models
{
    class GoogleDriveCommunicationParser : ICommunicationParser
    {
        
        private static Dictionary<string, string> cd_google_Term_Dictionary;
        private Dictionary<string, string> cd_google_Value_Dictionary;
        public GoogleDriveCommunicationParser()
        {
            cd_google_Term_Dictionary = new Dictionary<string, string>();
            cd_google_Term_Dictionary.Add("fileName", "title");
            cd_google_Term_Dictionary.Add("fileType", "mimeType");
            cd_google_Term_Dictionary.Add("fileSize", "fileSize");
            cd_google_Term_Dictionary.Add("lastModified", "modifiedData");
        }

        //Required by interface
        public CommonDescriptor createCommonDescriptor(StreamReader fileStreamReader, string relativeFilePath)
        {
            string googleJsonData = fileStreamReader.ReadToEnd();
            Dictionary<string, string> mdValues = new Dictionary<string, string>();
            if (googleJsonData != null)
            {
                JObject googleMD = JObject.Parse(googleJsonData);
                JToken curToken;
                string mdValue;
                foreach(KeyValuePair<string, string> entry in cd_google_Term_Dictionary)
                {
                    //todo: should we use trygetvalue?
                    curToken = googleMD.GetValue(entry.Value);
                    //curToken holds the data from Jobject for a specific field.
                    mdValue = (string)Convert.ChangeType(curToken.ToString(), typeof(string));
                    //mdValue gets the value for that field 
                    cd_google_Value_Dictionary.Add(entry.Key, mdValue);
                    //cd_google_Value_Dictionary holds the pairing from cd terms to the actual values.
                }
                //have all of the needed information to create a CD object.

                //TODO: is there a better way to do the following?
                string fileName, fileType, fsize, lastMod;
                DateTime lastModified;
                int fileSize;
                cd_google_Value_Dictionary.TryGetValue("fileName", out fileName);
                cd_google_Value_Dictionary.TryGetValue("fileType", out fileType);
                cd_google_Value_Dictionary.TryGetValue("fileSize", out fsize);
                cd_google_Value_Dictionary.TryGetValue("lastModified", out lastMod);
                lastModified = Convert.ToDateTime(lastMod);
                Int32.TryParse(fsize, out fileSize);

                CommonDescriptor cd = new CommonDescriptor(fileName, fileType, relativeFilePath, lastModified, fileSize);
                return cd;
            }
            return null;


        }
        //Required by interface
        public string createUploadBody(ServiceDescriptor descriptor)
        {
            return null;
        }

    }
}
