using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guqu.Models;
using WindowsDownloadManager = Guqu.Models.WindowsDownloadManager;


namespace Guqu.WebServices
{
    class CloudLogin
    {

        public CloudLogin()
        {
            //empty constructor 
        }

        public async static void googleDriveLogin()
        {

            var _googleDriveCredential = InitializeAPI.googleDriveCredential;
            var _googleDriveService = InitializeAPI.googleDriveService;

            // Create Drive API service & login 
            //TODO - move this code snippet to the login section - not init
            _googleDriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _googleDriveCredential,
                ApplicationName = "Guqu",
            });

            InitializeAPI.googleDriveService = _googleDriveService;

            

        }

        public async static void oneDriveLogin()
        {

            var _oneDriveClient = InitializeAPI.oneDriveClient;
            //Models.WindowsDownloadManager wdm = new WindowsDownloadManager();
            
            //these are also login params, should move to login class

            if (! _oneDriveClient.IsAuthenticated)
            {
                
                    await _oneDriveClient.AuthenticateAsync();
                    var token = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken;

                    Console.WriteLine("This succedded and Jordan is a bitch");
                    
                    InitializeAPI.oneDriveClient = _oneDriveClient;
                               
                }
                


                
            

            /*
            *******************
            This is all for testing how to download
            It is not actually needed for instantiating One Drive
            *******************


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
         */


        }
    }
}
