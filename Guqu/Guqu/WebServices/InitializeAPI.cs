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

using System.IO;
using System.Threading;

//Box Usages
using Box.V2;
using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Converter;
using Box.V2.Extensions;
using Box.V2.Models;
using Box.V2.Services;
using System.Net;

using Microsoft.OneDrive.Sdk.WindowsForms;
using Microsoft.OneDrive;
using Microsoft.OneDrive.Sdk;
using GuquMysql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Guqu.WebServices
{
    class InitializeAPI
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json

        static string ApplicationName = "Guqu";

        public static UserCredential googleDriveCredential;
        public static DriveService googleDriveService;

        private static string box_base_URI = "https://www.box.com/api/oauth2/authorize?";
        private static string box_client_id = "ihmzn176po17yuln4khbbihpsnnb7485";
        private static string box_secret = "2iZ4SIekP9Q96FhhTIGCTS7Nta9gQ3aE";
        private static Uri box_redirect_uri;
        private static string box_redirect_uri_string = "&redirect_uri=https://app.box.com/services/guqu";
        private static string responseType = "&response_type=code";

        public static IOneDriveClient oneDriveClient;
        private const string onedrive_client_id = "000000004018A88F";
        private const string onedrive_client_secret = "ancYlnjuaGCF15jnUZDO-jQDQ6Yn8tdY";
        private static string[] onedrive_scope = { "wl.signin", "wl.offline_access", "onedrive.readwrite" };
        private const string onedrive_redirect_uri = "https://login.live.com/oauth20_desktop.srf";

        private static string google_client_secret = "ecgV2ElpJNNg3FunOu1QK43x";
        private static string google_client_id = "177370389076-4ho5v080260k2c8ieiscncdn0e48sdcp.apps.googleusercontent.com";







        public InitializeAPI()
        {
            
        }

        /*
        Code needed to initialize the Google Drive 
        From https://developers.google.com/drive/v3/web/quickstart/dotnet
                    
            */

        public List<String> initGoogleDriveAPI()

        {

            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/drive-dotnet-quickstart.json
            string[] Scopes = { DriveService.Scope.Drive };


            
            using (var stream =
                    new FileStream("../../WebServices/guqu_drive_client_id.json", FileMode.Open, FileAccess.Read))
                {

                //TODO: saving as suth token in folder that ends with .json
                string credPath = System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.LocalApplicationData);
                    credPath = Path.Combine(credPath, "Guqu/.credentials/guqu_gdrive_creds.json");

                    
                

                    googleDriveCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);



                using (FileStream fs = new FileStream(credPath + "\\Google.Apis.Auth.OAuth2.Responses.TokenResponse-user", FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fs))
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    List<String> toReturn = new List<String>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            // Load each object from the stream and do something with it
                            JObject obj = JObject.Load(reader);
                            //toReturn = obj["access_token"] + "";
                            //Create string vals for tokens
                            string token = (obj["access_token"]).ToString();
                            string refreshToken = (obj["refresh_token"]).ToString();
                            //Console.WriteLine("token: " + token + " ----- " + "refresh: " + refreshToken);
                            //element 0 = access token
                            toReturn.Add(token);
                            //element 1 = refresh token
                            toReturn.Add(refreshToken);


                            Console.WriteLine("Google Drive access_token: " + obj["access_token"]);
                            Console.WriteLine("Google Drive expires in: " + obj["expires_in"]);
                            Console.WriteLine("Google Drive refresh_token: " + obj["refresh_token"]);
                        }
                    }
                    return toReturn;
                }
                
                //return text;
            }

            


            /*
               // Define parameters of request.
               FilesResource.ListRequest listRequest = googleDriveService.Files.List();
               listRequest.PageSize = 10;
               listRequest.Fields = "nextPageToken, files(id, name)";


               /*
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


        }//end init google drive


        /*
        Box init - TODO
            
            
            */
        public async void initBoxAPI()
        {
            /*
            box_redirect_uri = null;

            var boxConfig = new BoxConfig(box_client_id, box_secret, box_redirect_uri);
            var boxClient = new BoxClient(boxConfig);

            
            string authCode = await OAuth2Sample.GetAuthCode(boxConfig.AuthCodeUri, new Uri(boxConfig.RedirectUri));


            //string authCode = box_redirect_uri_string;

            try { 
            await boxClient.Auth.AuthenticateAsync(authCode);
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
            */
            var url = "https://account.box.com/api/oauth2/authorize?response_type=code&client_id=" + box_client_id + box_redirect_uri_string;

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(url);

            WebProxy myProxy = new WebProxy("myproxy", 80);
            myProxy.BypassProxyOnLocal = true;

            wrGETURL.Proxy = WebProxy.GetDefaultProxy();

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}:{1}", i, sLine);
            }
            Console.ReadLine();

            System.Diagnostics.Process.Start(url);

            /*
            try {
                var client = new HttpClient();
                var responseString = client.GetStringAsync("https://account.box.com/api/oauth2/authorize?response_type=code&client_id=" + box_client_id+box_redirect_uri_string);
                    //"&state=security_token%3DKnhMJatFipTAnM0nHlZA");
            }
            catch (Exception e)
            {
                Console.Write(e);
            }*/
            //await boxClient.Auth.AuthenticateAsync(box_authCode);


        }

        
        /* 
        One Drive Initialization - Uses the Web Forms Authenticator
        */
        public async void initOneDriveAPI() {
            oneDriveClient =  OneDriveClient.GetMicrosoftAccountClient(
            onedrive_client_id,
            onedrive_redirect_uri,
            onedrive_scope,
            webAuthenticationUi: new FormsWebAuthenticationUi()
            );
            
            Console.WriteLine("OneDrive auth? " + oneDriveClient.IsAuthenticated);
        }


        /*
        //THIS SHIT DOESNT WORK
        public static async Task<IOneDriveClient> getOneDriveClient()
        {
            var token = oneDriveClient.AuthenticationProvider.CurrentAccountSession.RefreshToken;

            //trys this sneak silent authenticator
            try {
                await OneDriveClient.GetSilentlyAuthenticatedMicrosoftAccountClient(
                    onedrive_client_id,
                    onedrive_redirect_uri,
                    onedrive_scope,
                    onedrive_client_secret,
                    token);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
            return oneDriveClient; 
                
       }
        */

    }
}
