using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

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
            StringBuilder builder = new StringBuilder("");
            builder.Append("POST /upload/drive/v3/files?uploadType=multipart HTTP/1.1\n");
            builder.Append("Host: www.googleapis.com\n");
            builder.Append("Authorization: Bearer ");
            //TODO: ensure this is the correct file location of the keys.
            //TODO: put these files with the metaData?
            //TODO: create a 'keys' controller that can give the absolute path of a file for something like that.
            string google_auth_token = File.ReadAllText("..\\keys\\guqu_drive_client_id.json");
            builder.Append(google_auth_token + "\n");

            //TODO: use a unique string to act as the boundary calls for all uploads? For google, for everything?
            string boundary = "BOUNDARYSTRING";
            builder.Append("Content-Type: multipart/related; boundary=" + boundary + "\n");

            //Dictionary<string, string> dict = descriptor.getRequestHeaders();
            CommonDescriptor cd = descriptor.getCommonDescriptor();
            
            builder.Append("Content-Length: " + cd.FileSize + "\n\n");
            builder.Append("--" + boundary);
            builder.Append("Content-Type: application/json; charset=UTF-8\n\n");
            builder.Append("{ name: " + cd.FileName + " }\n");
            builder.Append("--" + boundary);
            builder.Append("Content-Type: " + cd.FileType);

            //TODO: then you add the data to this, and terminate with another boundary string.


            return builder.ToString();
        }

    }
}
