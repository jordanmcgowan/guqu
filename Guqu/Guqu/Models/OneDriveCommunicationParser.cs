using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    class OneDriveCommunicationParser: ICommunicationParser
    {
        
        private static Dictionary<string, string> cd_od_Term_Dictionary;
        private Dictionary<string, string> cd_od_value_dictionary;
        public OneDriveCommunicationParser()
        {
            cd_od_Term_Dictionary = new Dictionary<string, string>();
            //TODO: determine a way to get these strings from a central source.
            cd_od_Term_Dictionary.Add("fileName", "name");
            cd_od_Term_Dictionary.Add("fileType", "name");
            cd_od_Term_Dictionary.Add("fileSize", "size");
            cd_od_Term_Dictionary.Add("fileID", "id");
            cd_od_Term_Dictionary.Add("lastModified", "lastModifiedDateTime");
        }

        //Required by interface
        public CommonDescriptor createCommonDescriptor(StreamReader fileStreamReader, string relativeFilePath)
        {
            JsonFileParser parser = new JsonFileParser();
            cd_od_value_dictionary = parser.retrieveValues(cd_od_Term_Dictionary, fileStreamReader);

            //variables to pass
            string fileName, fileType, fsize, lastMod, fileID;
            DateTime lastModified;
            int fileSize;

            //OneDrive specific logic
            cd_od_value_dictionary.TryGetValue("fileName", out fileName);
            if (fileName.IndexOf('.') == -1)
            {
                fileType = "folder";
            }
            else
            {
                fileName = fileName.Substring(0, fileName.IndexOf('.')); //name is up to the .
                cd_od_value_dictionary.TryGetValue("fileType", out fileType);
                fileType = fileType.Substring(fileType.IndexOf('.')); //type is . and after (ie .jpg)
            }
            cd_od_value_dictionary.TryGetValue("fileID", out fileID);
            cd_od_value_dictionary.TryGetValue("fileSize", out fsize);
            cd_od_value_dictionary.TryGetValue("lastModified", out lastMod);
            lastModified = Convert.ToDateTime(lastMod);
            Int32.TryParse(fsize, out fileSize);

            CommonDescriptor cd = new CommonDescriptor(fileName, fileType, relativeFilePath, fileID, lastModified, fileSize);
            return cd;

        }
        //Required by interface
        public string createUploadBody(ServiceDescriptor descriptor)
        {
            //todo itr2
            StringBuilder builder = new StringBuilder("");
            string folderID = "TODO"; //how should I get this>???
            string boundary = "BOUNDARYSTRING";
            builder.AppendLine("POST /drive/items/{" + folderID + "}/children");
            builder.AppendLine("Content-Type: multipart/related; boundary= " + boundary + "\n");
            builder.AppendLine("--" + boundary);
            string metaData = "TODO"; //what is metaData???
            builder.AppendLine("Content-ID: " + metaData);
            builder.AppendLine("Content-Type: application/json\n");
            builder.AppendLine("{");
            string fileName = descriptor.getCommonDescriptor().FileName + descriptor.getCommonDescriptor().FileType;
            builder.AppendLine("\"name\": \"" + fileName + "\",");
            builder.AppendLine("\"file\": {},");
            builder.AppendLine("\"@content.sourceURL\": \"cid:content\",");
            builder.AppendLine("\"@name.conflictBehavior\": \"rename\"");
            builder.AppendLine("}\n");
            builder.AppendLine("--" + boundary);
            builder.AppendLine("Content-ID: <content>");
            builder.AppendLine("Content-Type: test/plain\n");

            //TODO: UPLOAD FILE

            //TODO: APPEND boundary string


            return builder.ToString();
        }
        public string getExtension(string mimeType)
        {
            return null;
        }

    }
}
