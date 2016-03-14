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




namespace Guqu.WebServices
{
    class InitializeAPI
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json

        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Guqu";
        static UserCredential googleDriveCredential;
        public static DriveService googleDriveService;
        static BoxClient boxClient;

        private static string box_client_id = "ihmzn176po17yuln4khbbihpsnnb7485";
        private static string box_secret = "2iZ4SIekP9Q96FhhTIGCTS7Nta9gQ3aE";
        private static Uri box_redirect_uri;
        private static string box_redirect_uri_string;

        private static string onedrive_client_id = "000000004018A88F";
        private static string onedrive_client_secret = "ancYlnjuaGCF15jnUZDO-jQDQ6Yn8tdY";
        private static string onedrive_scope = "onedrive.readwrite";
        private static string onedrive_redirect_uri = "https://login.live.com/oauth20_desktop.srf";




        public InitializeAPI()
        {
            initGoogleDriveAPI();




        }

        /*
        Code needed to initialize the Google Drive 
        From https://developers.google.com/drive/v3/web/quickstart/dotnet
                    
            */
        private static void initGoogleDriveAPI()
        { 
        
            using (var stream =
                new FileStream("../../WebServices/guqu_drive_client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                googleDriveCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
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
            /*
            box_redirect_uri = null;

            var boxConfig = new BoxConfig(box_client_id, box_secret, box_redirect_uri);
            var boxClient = new BoxClient(boxConfig);
            


            String box_authCode = await OAuth2Sample.GetAuthCode(boxConfig.AuthCodeBaseUri, new Uri(boxConfig.RedirectUri));
            await boxClient.Auth.AuthenticateAsync(box_authCode);
            */           

        }



        
        /* 
        One Drive Initialization - Uses the 

        */
        private static void initOneDriveAPI() {

            












        }













    }
}
