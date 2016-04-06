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


        static AccountSession accountSession;
        private const string onedrive_client_id = "000000004018A88F";
        private const string onedrive_client_secret = "ancYlnjuaGCF15jnUZDO-jQDQ6Yn8tdY";
        private static string[] onedrive_scope = { "onedrive.readwrite", "wl.signin", "wl.offline_access" };
        private const string onedrive_redirect_uri = "https://login.live.com/oauth20_desktop.srf";


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

            //checks to see if the client is authenticated
            if (!_oneDriveClient.IsAuthenticated)
            {
                //checks for active session
                if (accountSession != null)
                {
                    var token = accountSession.RefreshToken;

                    //trys this sneak silent authenticator
                    await OneDriveClient.GetSilentlyAuthenticatedMicrosoftAccountClient(
                        onedrive_client_id,
                        onedrive_redirect_uri,
                        onedrive_scope,
                        token);
                }
                else{

                    //prompt to sign in or click yes
                    await _oneDriveClient.AuthenticateAsync();
                    accountSession = _oneDriveClient.AuthenticationProvider.CurrentAccountSession;
                }


                Console.WriteLine("This succedded");

                InitializeAPI.oneDriveClient = _oneDriveClient;

            }



        }
    }
}
