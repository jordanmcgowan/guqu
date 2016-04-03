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

            /*
            ****************************************
           THIS BLOCK NEEDED FOR TESTING
           WILL PRINT OUT LIST OF FILES IN CONSOLE
            *****************************************



            FilesResource.ListRequest listRequest = _googleDriveService.Files.List();
            listRequest.PageSize = 1;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
           
            
            int count = 0;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {

                    if (count == 0)
                    {
                        count++;
                        var fileId = file.Id;

                        Console.WriteLine("********");
                        Console.WriteLine(fileId);
                        Console.WriteLine(file.Name);
                        Console.Write(file.MimeType);
                        Console.WriteLine("********");
                        var request = _googleDriveService.Files.Export(fileId, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                        var stream = new MemoryStream();
                        WindowsDownloadManager wdm = new WindowsDownloadManager();
                        

                        
                        request.MediaDownloader.ProgressChanged +=
                        (IDownloadProgress progress) =>
                        {
                            switch (progress.Status)
                            {
                                case DownloadStatus.Downloading:
                                    {
                                        Console.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        Console.WriteLine("Download complete.");
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        Console.WriteLine("Download failed.");
                                        break;
                                    }
                            }
                        };

                        try {
                            await request.DownloadAsync(stream);
                            //TODO: not always a .doc, change.
                            wdm.downloadFile(stream, file.Name + ".doc");

                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        
                    }
                            Console.WriteLine("{0} ({1})", file.Name, file.Size);
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
            Models.WindowsDownloadManager wdm = new WindowsDownloadManager();
            
            //these are also login params, should move to login class

            if (!_oneDriveClient.IsAuthenticated)
            {
                try
                {
                    await _oneDriveClient.AuthenticateAsync();
                    var token = _oneDriveClient.AuthenticationProvider.CurrentAccountSession.AccessToken;
                    Console.WriteLine("This succedded and Jordan is a bitch");

                    var contentStream = await _oneDriveClient.Drive.Items["8FA41A1E5CF18E2B!112"].Content.Request().GetAsync();
                    MemoryStream stream = (MemoryStream)contentStream;

                    //TODO: not always a .doc, change.
                    wdm.downloadFile(stream, "test.doc");


                }
                catch (OneDriveException e)
                {
                    Console.WriteLine(e);

                }
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
