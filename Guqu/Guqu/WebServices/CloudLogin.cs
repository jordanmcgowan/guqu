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
        private static string[] onedrive_scope = { "onedrive.readwrite", "wl.signin", "wl.offline_access", };
        private const string onedrive_redirect_uri = "https://login.live.com/oauth20_desktop.srf";
        //private const string onedrive_redirect_uri = "https://login.live.com/oauth20_token.srf";
        //


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

        public async static void oneDriveLogin(User user)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            var refreshToken = "";
            var accessToken = "";
            int cloudId = 1;
            ServerCommunicationController db = new ServerCommunicationController();
            //Models.WindowsDownloadManager wdm = new WindowsDownloadManager();

            //these are also login params, should move to login class

            //checks to see if the client is authenticated

            //checks for active session

            if (db.doesUserCloudExist(user.User_id, cloudId))
            {

                await _oneDriveClient.AuthenticateAsync();
                accountSession = _oneDriveClient.AuthenticationProvider.CurrentAccountSession;
                Console.WriteLine("One Drive login succedded for user " + user.User_id);

                //Console.WriteLine("1D refresh: " + oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken);
                //Console.WriteLine("1D token: " + oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken);
                refreshToken = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken;
                accessToken = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken;


                /* BLOCK NOT WORKING                
                Was used to try to login the user in the background 
                But I get an unexpected exception at the GetSilentlyAuthent... method   


                var userClouds = db.SelectUserClouds(user.User_id);

                foreach (var cloud in userClouds)
                {
                    if (db.doesUserCloudExist(user.User_id, cloudId))
                    {
                        accessToken = cloud.Cloud_token;
                        refreshToken = cloud.Refresh_token;
                    }




                }

                

                try
                {
                    
                    _oneDriveClient = await OneDriveClient.GetSilentlyAuthenticatedMicrosoftAccountClient(
                            onedrive_client_id,
                            onedrive_redirect_uri,
                            onedrive_scope,
                            refreshToken);
                }
                
                    catch (OneDriveException e)
                {
                    Console.WriteLine(e);
                }*/
            }

            //trys this sneak silent authenticator


            else {


                try
                {
                    await _oneDriveClient.AuthenticateAsync();

                    accountSession = _oneDriveClient.AuthenticationProvider.CurrentAccountSession;
                    Console.WriteLine("One Drive login succedded for user " + user.User_id);

                    //Console.WriteLine("1D refresh: " + oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken);
                    //Console.WriteLine("1D token: " + oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken);
                    refreshToken = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken;
                    accessToken = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken;


                    db.InsertNewUserCloud(user.User_id, accessToken, cloudId, refreshToken);

                }
                catch (Exception e)
                {
                    Console.WriteLine("One Drive login FAILED: " + e.Message);
                }
            }

            InitializeAPI.oneDriveClient = _oneDriveClient;
        }
    }
}

