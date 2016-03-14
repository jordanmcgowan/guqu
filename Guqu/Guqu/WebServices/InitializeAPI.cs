using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Google Drive Usages
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Box Usages
using Box.V2;
using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Converter;
using Box.V2.Extensions;
using Box.V2.Models;
using Box.V2.Services;




namespace Guqu.WebServices
{
    class InitializeAPI
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json

        static string ApplicationName = "Guqu";

        static UserCredential googleDriveCredential;
        public static DriveService googleDriveService;
     

        private static string box_client_id = "ihmzn176po17yuln4khbbihpsnnb7485";
        private static string box_secret = "2iZ4SIekP9Q96FhhTIGCTS7Nta9gQ3aE";
        private static Uri box_redirect_uri;
        private static string box_redirect_uri_string;

        private static string onedrive_client_id = "000000004018A88F";
        private static string onedrive_client_secret = "ancYlnjuaGCF15jnUZDO-jQDQ6Yn8tdY";
        private static string onedrive_scope = "onedrive.readwrite";
        private static string onedrive_redirect_uri = "https://login.live.com/oauth20_desktop.srf";

        private static string google_client_secret = "ecgV2ElpJNNg3FunOu1QK43x";
        private static string google_client_id = "177370389076-4ho5v080260k2c8ieiscncdn0e48sdcp.apps.googleusercontent.com";







        public InitializeAPI()
        {
            initGoogleDriveAPI();
            initBoxAPI();
            initOneDriveAPI();
        }

        /*
        Code needed to initialize the Google Drive 
        From https://developers.google.com/drive/v3/web/quickstart/dotnet
                    
            */
        private static void initGoogleDriveAPI()
        {

            //Scopes for use with the Google Drive API
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                 DriveService.Scope.DriveFile};
          
                                               // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            googleDriveCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets{ClientId = google_client_id, ClientSecret = google_client_secret},
                                                                        scopes,
                                                                        Environment.UserName,
                                                                        CancellationToken.None,
                                                                        new FileDataStore("Guqu.GoogleDrive.Auth.Store")).Result;




            // Create Drive API service. Needed for actual log on - might have to move this
            googleDriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleDriveCredential,
                ApplicationName = ApplicationName,
            });

        }//end init google drive


        /*
        Box init - TODO
            
            
            */
        private static async void initBoxAPI()
        {
            
            box_redirect_uri = null;

            var boxConfig = new BoxConfig(box_client_id, box_secret, box_redirect_uri);
            var boxClient = new BoxClient(boxConfig);




            

            
            //await boxClient.Auth.AuthenticateAsync(box_authCode);
                    

        }



        
        /* 
        One Drive Initialization - Uses the 

        */
        private static void initOneDriveAPI() {

       }

    }
}
