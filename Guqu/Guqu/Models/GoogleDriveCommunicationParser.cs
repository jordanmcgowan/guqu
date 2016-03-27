using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Guqu.Models
{
    class GoogleDriveCommunicationParser : ICommunicationParser
    {
        private static Dictionary<string, string> mimeType_extension_Dictionary;
        private static Dictionary<string, string> cd_google_Term_Dictionary;
        private Dictionary<string, string> cd_google_Value_Dictionary;
        public GoogleDriveCommunicationParser()
        {
            //TODO: can these be moved to another class/into a file.
            //Instantiate the Common Descriptor to Google Terminology dictionary
            cd_google_Term_Dictionary = new Dictionary<string, string>();
            cd_google_Term_Dictionary.Add("fileName", "Name");
            cd_google_Term_Dictionary.Add("fileType", "MimeType");
            cd_google_Term_Dictionary.Add("fileSize", "Size");
            cd_google_Term_Dictionary.Add("fileID", "Id");
            cd_google_Term_Dictionary.Add("lastModified", "ModifiedTime");

            //Instantiate the mimetype to extension dictionary
            mimeType_extension_Dictionary = new Dictionary<string, string>();
            mimeType_extension_Dictionary.Add("text/html", ".HTML");
            mimeType_extension_Dictionary.Add("text/plain", ".txt");
            mimeType_extension_Dictionary.Add("application/rtf", ".rtf");
            mimeType_extension_Dictionary.Add("application/vnd.oasis.opendocument.text", ".odt");
            mimeType_extension_Dictionary.Add("applicaion/pdf", ".pdf");
            //TODO: is this the correct extension, is there a '-' (dash) in the mimetype?
            mimeType_extension_Dictionary.Add("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".doc");
            mimeType_extension_Dictionary.Add("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx");
            mimeType_extension_Dictionary.Add("application/x-vnd.oasis.opendocument.spreadsheet", ".ods");
            mimeType_extension_Dictionary.Add("application/pdf", ".pdf");
            mimeType_extension_Dictionary.Add("text/csv", ".csv");
            mimeType_extension_Dictionary.Add("image/jpeg", ".jpg");
            mimeType_extension_Dictionary.Add("image/png", ".png");
            mimeType_extension_Dictionary.Add("image/svg+xml", ".svg");
            mimeType_extension_Dictionary.Add("application/vnd.openxmlformats-officedocument.presentationml.presentation", ".ppt");
            mimeType_extension_Dictionary.Add("application/vnd.google-apps.script+json", ".JSON");

        }   

        //Required by interface
        public CommonDescriptor createCommonDescriptor(StreamReader fileStreamReader, string relativeFilePath)
        {

            JsonFileParser parser = new JsonFileParser();
            cd_google_Value_Dictionary = parser.retrieveValues(cd_google_Term_Dictionary, fileStreamReader);

            //variables to pass in
            string fileName, fileType, fsize, lastMod, fileID;
            DateTime lastModified;
            int fileSize;

            //google specific logic for creation
            cd_google_Value_Dictionary.TryGetValue("fileName", out fileName);
            cd_google_Value_Dictionary.TryGetValue("fileType", out fileType);
            if (fileType.Equals("application/vnd.google-apps.folder")) //googles folders are called this.
            {
                //do translaion for fileType
                fileType = "folder";
            }
            cd_google_Value_Dictionary.TryGetValue("fileID", out fileID);
            cd_google_Value_Dictionary.TryGetValue("fileSize", out fsize);
            cd_google_Value_Dictionary.TryGetValue("lastModified", out lastMod);
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
            builder.AppendLine("POST /upload/drive/v3/files?uploadType=multipart HTTP/1.1");
            builder.AppendLine("Host: www.googleapis.com");
            builder.Append("Authorization: Bearer ");
            //TODO: ensure this is the correct file location of the keys.
            //TODO: put these files with the metaData?
            //TODO: create a 'keys' controller that can give the absolute path of a file for something like that.
            string google_auth_token = File.ReadAllText("..\\keys\\guqu_drive_client_id.json");
            builder.AppendLine(google_auth_token);

            //TODO: use a unique string to act as the boundary calls for all uploads? For google, for everything?
            string boundary = "BOUNDARYSTRING";
            builder.AppendLine("Content-Type: multipart/related; boundary=" + boundary);

            //Dictionary<string, string> dict = descriptor.getRequestHeaders();
            CommonDescriptor cd = descriptor.getCommonDescriptor();
            
            builder.AppendLine("Content-Length: " + cd.FileSize + "\n");
            builder.AppendLine("--" + boundary);
            builder.AppendLine("Content-Type: application/json; charset=UTF-8\n");
            builder.AppendLine("{ name: " + cd.FileName + " }");
            builder.AppendLine("--" + boundary);
            builder.AppendLine("Content-Type: " + cd.FileType);

            //TODO: then you add the data to this, and terminate with another boundary string.


            return builder.ToString();
        }

        public string getExtension(string fileType)
        {
            //Convert between the mimetype and the extension it is linked to.
            //https://developers.google.com/drive/v3/web/manage-downloads#downloading_google_documents
            string extension;
            mimeType_extension_Dictionary.TryGetValue(fileType, out extension);
            return extension;
        }
        
    }
}
