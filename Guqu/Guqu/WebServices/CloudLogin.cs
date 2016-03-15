using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
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

            var _oneDriveClient = InitializeAPI.oneDriveClient;
            //these are also login params, should move to login class

            if (!_oneDriveClient.IsAuthenticated) { 
                try
                {
                    await _oneDriveClient.AuthenticateAsync();
                  var token = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken;
                    Console.WriteLine("This succedded and Jordan is a bitch");

                }
                catch (OneDriveException e)
                {
                    Console.WriteLine(e);

                }
        }

           

            try {
                var root = await _oneDriveClient.Drive.Root.Request().Expand("children").GetAsync();
                Console.WriteLine(root.Id);

               Stream cStream = await _oneDriveClient.Drive.Items[root.Id].Content.Request().GetAsync();
                Console.WriteLine(cStream.ToString());





             
                          
           
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }


        }

        private static bool boxLogin()
        {

            return false;
        }



    }
}
