﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Guqu.Exceptions;

namespace Guqu.Models
{
    public class GoogleDriveCommunicationParser : ICommunicationParser
    {
        private static Dictionary<string, string> extension_mimeType_Dictionary;
        private static Dictionary<string, string> convert_extension_Dictionary;
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

            /*
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
            */

            //TODO: input some 'default' values so if there is no extension (a google doc created item), make a 'document' become .odt, or all images become .png. etc etc
            
            //Instantiate the extension to mimetype dictionary
            extension_mimeType_Dictionary = new Dictionary<string, string>();
            extension_mimeType_Dictionary.Add(".HTML", "text/html");
            extension_mimeType_Dictionary.Add(".txt", "text/plain");
            extension_mimeType_Dictionary.Add(".rtf", "application/rtf");
            extension_mimeType_Dictionary.Add(".odt", "application/vnd.oasis.opendocument.text");
            extension_mimeType_Dictionary.Add(".pdf", "applicaion/pdf");
            extension_mimeType_Dictionary.Add(".doc", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            extension_mimeType_Dictionary.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            extension_mimeType_Dictionary.Add(".ods", "application/x-vnd.oasis.opendocument.spreadsheet");
            extension_mimeType_Dictionary.Add(".csv", "text/csv");
            extension_mimeType_Dictionary.Add(".jpg", "image/jpeg");
            extension_mimeType_Dictionary.Add(".png", "image/png");
            extension_mimeType_Dictionary.Add(".svg", "image/svg+xml");
            extension_mimeType_Dictionary.Add(".ppt", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            extension_mimeType_Dictionary.Add(".JSON", "application/vnd.google-apps.script+json");
            extension_mimeType_Dictionary.Add(".zip", "application/zip");

            //defaults, TODO: let user define these? Prompt the user for them?
            //Found from https://developers.google.com/drive/v3/web/mime-types
            //TODO: Add the remainder
            convert_extension_Dictionary = new Dictionary<string, string>();
            convert_extension_Dictionary.Add("application/vnd.google-apps.document", ".doc");
            convert_extension_Dictionary.Add("application/vnd.google-apps.drawing", ".jpg");
            convert_extension_Dictionary.Add("application/vnd.google-apps.spreadsheet", ".xlsx");
            convert_extension_Dictionary.Add("application/vnd.google-apps.folder", ".zip");
            convert_extension_Dictionary.Add("folder", ".zip");
            

        }

        //Required by interface
        public CommonDescriptor createCommonDescriptor(string relativeFilePath, string jsonMetaDataFile)
        {

            JsonFileParser parser = new JsonFileParser();
            cd_google_Value_Dictionary = parser.retrieveValues(cd_google_Term_Dictionary, jsonMetaDataFile);

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
            
            CommonDescriptor cd = new CommonDescriptor(fileName, fileType, relativeFilePath, fileID, "Google Drive", lastModified, fileSize);

            return cd;
        }
       
        public string convertExtension(string oldExtension)
        {
            //Convert between the mimetype and the extension it is linked to.
            //https://developers.google.com/drive/v3/web/manage-downloads#downloading_google_documents
            string convertedExtension;
            oldExtension = getValidInput(oldExtension, convert_extension_Dictionary);
            if(oldExtension == null)
            {
                return null;
            }
            convert_extension_Dictionary.TryGetValue(oldExtension, out convertedExtension);
            return convertedExtension;
        }
        public string getMimeType(string extension)
        {
            //TODO: need to reverse the dictionary before this works.
            string mimeType;
            extension = getValidInput(extension, extension_mimeType_Dictionary);
            if(extension == null)
            {
                return null;
            }
            extension_mimeType_Dictionary.TryGetValue(extension, out mimeType);
            //will return null if the user cancels.
            return mimeType;
        }
        private string getValidInput(string originalValue, Dictionary<string, string> validValues)
        {
            string userInput = originalValue;
            //launch error window
            while(validValues.ContainsKey(originalValue) != true)
            {
                //when user presses okay
                //check for value
                //userInput = errorPrompt.getValue();

                //capture user closing the window
                //return null
            }
            //close errorWindow
            return userInput;
        }
        
    }
}
