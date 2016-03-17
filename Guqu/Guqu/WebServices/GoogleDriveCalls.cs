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
            fetchAllMDFiles(controller, accountName, "root");
            return true;
        }
        //TODO: have controller be global, or static
        private void fetchAllMDFiles(MetaDataController controller, string relativeRequestPath, string parentID)
        {

            string googleFolderName = "application/vnd.google-apps.folder";
            int numItemsPerFetch = 1000;

            //over arching list of files in this directory
            IList<Google.Apis.Drive.v3.Data.File> allFiles = new List<Google.Apis.Drive.v3.Data.File>();

            //Each iteration fill the files list.
            IList<Google.Apis.Drive.v3.Data.File> iterationFiles;

            //Execution for each iteration
            Google.Apis.Drive.v3.Data.FileList exec;

            var googleDriveService = InitializeAPI.googleDriveService;
            FilesResource.ListRequest listRequest = googleDriveService.Files.List();
            
            string nextPageToken = null;

            Boolean moreFilesExist = true;

            //get all files in this directory
            while (moreFilesExist)
            {
                listRequest = googleDriveService.Files.List();
                listRequest.Fields = "nextPageToken, files";
                listRequest.PageSize = numItemsPerFetch; //get 20 things each pass
                listRequest.Q = "'" + parentID + "' in parents";
                //get the next 10 possible folders
                if (nextPageToken != null)
                {
                    listRequest.PageToken = nextPageToken;
                }
                exec = listRequest.Execute();
                nextPageToken = exec.NextPageToken;
                iterationFiles = exec.Files;

                //files has first 20 items.
                foreach(var cur in iterationFiles)
                {
                    if(cur.Trashed != true)
                    {
                        //not trash
                        allFiles.Add(cur);
                    }
                    
                }
                if(iterationFiles.Count != numItemsPerFetch)
                {
                    //we did not have max items, no more items to fetch
                    moreFilesExist = false;
                }
            }


            //now we have all files/folders in the current directory

            string pathForFile;
            //string nextToken = listRequest.Execute().NextPageToken;
            Google.Apis.Drive.v3.Data.File curFile;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string curFileSerialized;

            while (allFiles.Count != 0) //while there are elements
            {
                curFile = allFiles.First(); //get first file 
                curFileSerialized = serializer.Serialize(curFile);
                pathForFile = controller.getAbsoluteFilePathForAddingMDFile(relativeRequestPath);
          
                if (curFile.MimeType.Equals(googleFolderName))
                {
                    //store data for the folder
                    //recurse on the folder and do its children
                    File.WriteAllText(pathForFile + "\\" + curFile.Name + "_folder.json", curFileSerialized);
                    fetchAllMDFiles(controller, relativeRequestPath + "\\" + curFile.Name, curFile.Id);
                }
                else
                {
                    //store data for this file
                    File.WriteAllText(pathForFile + "\\" + curFile.Name + "_file.json", curFileSerialized);
                }
                allFiles.RemoveAt(0); //remove this element
            }

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
