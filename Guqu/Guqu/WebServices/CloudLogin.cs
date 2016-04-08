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
using GuquMysql;

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

        public async static Task googleDriveLogin()
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

        public async static Task oneDriveLogin(User user)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            int cloudId = 1;
            //Models.WindowsDownloadManager wdm = new WindowsDownloadManager();

            //these are also login params, should move to login class

            //checks to see if the client is authenticated
            if (!_oneDriveClient.IsAuthenticated)
            {
                //checks for active session
                if (accountSession != null)
                {
                    var refreshToken = accountSession.RefreshToken;
                    string[] secret = { onedrive_client_secret };

                    //trys this sneak silent authenticator
                    /*
                    await OneDriveClient.GetSilentlyAuthenticatedMicrosoftAccountClient(
                        onedrive_client_id,
                        onedrive_redirect_uri,
                        secret,
                        refreshToken);
                        */
                }
                else{
                    
                    try {

                        await _oneDriveClient.AuthenticateAsync();
                        
                        accountSession = _oneDriveClient.AuthenticationProvider.CurrentAccountSession;
                        Console.WriteLine("One Drive login succedded for user " + user.User_id);

                        //Console.WriteLine("1D refresh: " + oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken);
                        //Console.WriteLine("1D token: " + oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken);
                        string refreshToken = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken;
                        string accessToken = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken;
                        ServerCommunicationController db = new ServerCommunicationController();
                        db.InsertNewUserCloud(user.User_id, accessToken, cloudId, refreshToken);

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("One Drive login FAILED: " + e.Message);
                    }
                    
                                      
                }
                
            }

            InitializeAPI.oneDriveClient = _oneDriveClient;

        }
    }
}
