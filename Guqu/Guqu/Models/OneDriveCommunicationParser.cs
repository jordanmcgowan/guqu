using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    public class OneDriveCommunicationParser: ICommunicationParser
    {
        
        private static Dictionary<string, string> cd_od_Term_Dictionary;
        private Dictionary<string, string> cd_od_value_dictionary;
        public OneDriveCommunicationParser()
        {
            cd_od_Term_Dictionary = new Dictionary<string, string>();
            //TODO: determine a way to get these strings from a central source.
            cd_od_Term_Dictionary.Add("fileName", "Name");

            //TODO: fix  the filetype like we discussed
            cd_od_Term_Dictionary.Add("fileType", "File");
            cd_od_Term_Dictionary.Add("fileSize", "Size");
            cd_od_Term_Dictionary.Add("fileID", "Id");
            cd_od_Term_Dictionary.Add("lastModified", "LastModifiedDateTime");
        }

        //Required by interface
        public CommonDescriptor createCommonDescriptor(string relativeFilePath, string jsonMetaData)
        {
            JsonFileParser parser = new JsonFileParser();
            cd_od_value_dictionary = parser.retrieveValues(cd_od_Term_Dictionary, jsonMetaData);

            //variables to pass
            string fileName, fileType, fsize, lastMod, fileID;
            DateTime lastModified;
            int fileSize;

            //OneDrive specific logic
            cd_od_value_dictionary.TryGetValue("fileName", out fileName);
            cd_od_value_dictionary.TryGetValue("fileType", out fileType);
            if(fileType == "")
            {
                //null represents 'not a file' aka a folder
                fileType = "folder";
            }
            else
            {
                JObject metaData = JObject.Parse(jsonMetaData);
                JObject curToken = (JObject)metaData.GetValue("File");
                JToken nextToken = curToken.GetValue("MimeType");
                fileType = (string)Convert.ChangeType(nextToken.ToString(), typeof(string));
            }

            cd_od_value_dictionary.TryGetValue("fileID", out fileID);
            cd_od_value_dictionary.TryGetValue("fileSize", out fsize);
            cd_od_value_dictionary.TryGetValue("lastModified", out lastMod);
            lastModified = Convert.ToDateTime(lastMod);
            Int32.TryParse(fsize, out fileSize);

            CommonDescriptor cd = new CommonDescriptor(fileName, fileType, relativeFilePath, fileID,"One Drive", lastModified, fileSize);
            
            return cd;

        }
       
        public string getExtension(string mimeType)
        {
            return null;
        }

    }
}
