using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.WebServices
{
    class CloudLogin
    {

        public CloudLogin() {
            //empty constructor 
        }

        public async static void googleDriveLogin() {

            var _googleDriveCredential = InitializeAPI.googleDriveCredential;
            var _googleDriveService = InitializeAPI.googleDriveService;

            // Create Drive API service & login 
            //TODO - move this code snippet to the login section - not init
            _googleDriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _googleDriveCredential,
                ApplicationName = "Guqu",
            });



            /*
            ****************************************
           THIS BLOCK NEEDED FOR TESTING
           WILL PRINT OUT LIST OF FILES IN CONSOLE
            ****************************************

           FilesResource.ListRequest listRequest = _googleDriveService.Files.List();
           listRequest.PageSize = 10;
           listRequest.Fields = "nextPageToken, files(id, name)";
           
           // List files.
           IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
               .Files;
           Console.WriteLine("Files:");
           if (files != null && files.Count > 0)
           {
               foreach (var file in files)
               {
                   Console.WriteLine("{0} ({1})", file.Name, file.Id);
               }
           }
           else
           {
               Console.WriteLine("No files found.");
           }
           Console.Read();
           */

        }

        public async static void oneDriveLogin()
        {
            //these are also login params, should move to login class
            try
            {
                await InitializeAPI.oneDriveClient.AuthenticateAsync();
                Console.Write("This succedded");
            }
            catch (OneDriveException e)
            {
                Console.Write(e);

            }

        }

        private static bool boxLogin()
        {

            return false;
        }



    }
}
