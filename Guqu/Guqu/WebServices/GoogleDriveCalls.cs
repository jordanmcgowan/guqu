using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guqu.Models;
using Google.Apis.Download;

using Google.Apis.Drive;
using Google.Apis.Drive.v3;
using System.Collections;
using System.Web.Script.Serialization;

namespace Guqu.WebServices
{
    class GoogleDriveCalls : ICloudCalls
    {
        public async Task<bool> downloadFile(Google.Apis.Drive.v3.Data.File file)
        {
            var _googleDriveService = InitializeAPI.googleDriveService;
            var _file = file;
            
            Console.WriteLine("********");
            Console.WriteLine(file.Id);
            Console.WriteLine(_file.Name);
            Console.Write(_file.MimeType);
            Console.WriteLine("********");


            var request = _googleDriveService.Files.Export(_file.Id, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
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

            try
            {
                await request.DownloadAsync(stream);
                //TODO: not always a .doc, change.
                wdm.downloadFile(stream, _file.Name + ".doc");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }


            return true;

        }

        public bool fetchAllMetaData(MetaDataController controller, string accountName)
        {
            string googleFolderName = "application/vnd.google-apps.folder";

            //over arching list of files
            IList<Google.Apis.Drive.v3.Data.File> folders = new List<Google.Apis.Drive.v3.Data.File>();

            //Each iteration fill the files list.
            IList<Google.Apis.Drive.v3.Data.File> files;
            //Execution for each iteration
            Google.Apis.Drive.v3.Data.FileList exec;

            var googleDriveService = InitializeAPI.googleDriveService;

            //begin loop to get all files.
            Boolean moreFolders = true;
            FilesResource.GetRequest getRequest;
            FilesResource.ListRequest listRequest;
            string nextPageToken = null;
            while (moreFolders) 
            {
                getRequest = formGetRequest("root");
                var file = getRequest.Execute();
                listRequest = googleDriveService.Files.List();
                //get the next 10 possible folders
                listRequest.PageSize = 10;
                if(nextPageToken != null)
                {
                    listRequest.PageToken = nextPageToken;
                }
                listRequest.OrderBy = "folder";
                listRequest.Fields = "nextPageToken, files";

                exec = listRequest.Execute();
                files = exec.Files;
                nextPageToken = exec.NextPageToken;
                //now we have the next page size worth of files, check the last to see if it is a folder
                
                foreach(var newFile in files)
                {
                    if (newFile.MimeType.Equals(googleFolderName))
                    {
                        //is a folder
                        if(newFile.Trashed == false)
                        {
                            //and not trashed
                            folders.Add(newFile);
                        }
                    }
                    else
                    {
                        moreFolders = false;
                    }
                }
            }



            return true;
        }
        //TODO: have controller be global, or static
        private void fetchAllMDFiles(MetaDataController controller, FilesResource.GetRequest getRequest, string relativeRequestPath)
        {

            //getRequest.Fields = "children";
            var file = getRequest.Execute();
            //don't want metadata for root
            var googleDriveService = InitializeAPI.googleDriveService;
            FilesResource.ListRequest listRequest = googleDriveService.Files.List();
            listRequest.PageSize = 25;
            //listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.OrderBy = "folder";

            //listRequest.Execute().NextPageToken;

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            
            string pathForFile;
            //string nextToken = listRequest.Execute().NextPageToken;
            Google.Apis.Drive.v3.Data.File curFile;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string curFileSerialized;
            while (files.Count != 0) //while there are elements
            {
                curFile = files.First(); //get first file 
                curFileSerialized = serializer.Serialize(curFile);
                pathForFile = controller.getAbsoluteFilePathForAddingMDFile(relativeRequestPath);
                //TODO: define this string somewhre
                if(curFile.MimeType.Equals("application/vnd.google-apps.folder"))
                {

                    //store data for the folder
                    //recurse on the folder and do its children
                    File.WriteAllText(pathForFile + curFile.Name + "_folder.json", curFileSerialized);
                    fetchAllMDFiles(controller, formGetRequest(curFile.Id), relativeRequestPath + "//" + curFile.Name);
                }
                else
                {
                    //store data for this file
                    File.WriteAllText(pathForFile + curFile.Name + "_file.json", curFileSerialized);
                    
                }
            }


            /*
            // Define parameters of request.
            var googleDriveService = InitializeAPI.googleDriveService;
            FilesResource.ListRequest listRequest = googleDriveService.Files.List();
            listRequest.PageSize = 25;
            listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.OrderBy = "folder";

            //listRequest.Execute().NextPageToken;

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

    */
        }
        private FilesResource.GetRequest formGetRequest(string fileID)
        {
            var driveService = InitializeAPI.googleDriveService;
            FilesResource.GetRequest getRequest = driveService.Files.Get(fileID);

            return getRequest;
        }

        public bool shareFile(MemoryStream stream)
        {
            throw new NotImplementedException();
        }

        public bool shareFile(Stream stream)
        {
            throw new NotImplementedException();
        }

        public bool uploadFile(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
