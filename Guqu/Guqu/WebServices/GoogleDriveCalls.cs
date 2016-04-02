using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Guqu.Models;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using System.Web.Script.Serialization;
using Guqu.Exceptions;

namespace Guqu.WebServices
{
    class GoogleDriveCalls : ICloudCalls
    {
        private GoogleDriveCommunicationParser googleCommParser;

        public GoogleDriveCalls()
        {
            googleCommParser = new GoogleDriveCommunicationParser();
        }

        public async Task<bool> downloadFile(CommonDescriptor cd)
        {

            var _googleDriveService = InitializeAPI.googleDriveService;
            //Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            //file.Id = cd.FileID;
            //var _file = _googleDriveService.Files.Get(cd.FileID);
            
            Console.WriteLine("********");
            Console.WriteLine(cd.FileID);
            //Console.WriteLine(_file.);
            //Console.Write(_file.);
            Console.WriteLine("********");


            //TODO: THE MIMETYPE THIS IS CAN'T BE THE SAME MIMETYPE AS WHAT IT WAS SAVED. It needs to be an export type.
            //https://developers.google.com/drive/v3/web/manage-downloads#downloading_google_documents
            string extension, mimeType = "";
            //figure out the mimetype by using the extension in the file name.

            GoogleDriveCommunicationParser gdcp = new GoogleDriveCommunicationParser();
            if (cd.FileName.IndexOf('.') != -1)
            {
                //has an extension.
                extension = cd.FileName.Substring(cd.FileName.IndexOf('.')); //gets the extension.
            }
            else
            {
                //get a 'default' extension based on the format, convert to a real extension
                extension = cd.FileType;
                extension = gdcp.convertExtension(extension);
                if(extension == null)
                {
                    //still can't find a good extension
                    //not able to be downloaded, cancel download
                    //TODO: ban the user from pressing download on these kinds of files?
                    return false;
                }
            }
            mimeType = gdcp.getMimeType(extension);

            if (mimeType == null)
            {
                //user cancelled giving us a new extension, cancel the download
                return false;
            }

            var request = _googleDriveService.Files.Export(cd.FileID, mimeType);
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
                IDownloadProgress x = await request.DownloadAsync(stream);
                wdm.downloadFile(stream, cd.FileName + extension);

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
            CommonDescriptor curCD;
            StreamReader streamReader;
            
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
                    streamReader = new StreamReader(pathForFile + "\\" + curFile.Name + "_folder.json");
                    controller.createDirectory(relativeRequestPath + "\\" + curFile.Name);
                    curCD = googleCommParser.createCommonDescriptor(streamReader, relativeRequestPath);
                    streamReader.Close();
                    controller.addCommonDescriptorFile(curCD);
                    //get a stream for the file we just wrote. 

                    fetchAllMDFiles(controller, relativeRequestPath + "\\" + curFile.Name, curFile.Id);
                }
                else
                {
                    //store data for this file
                        File.WriteAllText(pathForFile + "\\" + curFile.Name + "_file.json", curFileSerialized);
                        streamReader = new StreamReader(pathForFile + "\\" + curFile.Name + "_file.json");
                        curCD = googleCommParser.createCommonDescriptor(streamReader, relativeRequestPath);
                        streamReader.Close();
                        controller.addCommonDescriptorFile(curCD);
                    
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

        public bool deleteFile(CommonDescriptor cd)
        {
            try {
                var _googleDriveService = InitializeAPI.googleDriveService;
                var request = _googleDriveService.Files.Delete(cd.FileID);
                request.Execute();

                //now that that file is gone remove the CD file and the metaData file from our records.
                MetaDataController mdc = new MetaDataController("E:\\GuquTestFolder");
                mdc.deleteCloudObjet(cd);
            }
            catch(Exception e)
            {
                //Something caused an error, delete did not occur. 
                return false;
            }
            return true;

        }

        public bool moveFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            return moveCopyFile(fileToMove, folderDestination, true);
        }

        public bool copyFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            //copying currently just creates an identical pointer to the file in the new folder.
            //this means that a copy followed by a move will remove both pointers previously created b/c they are one in the same.

            //TODO: fix depends on what the user wants.
            //a) user wants two seperate files: Need to upload a 'new file' that has the contents of the old file
            //b) user wants two pointers to one file: Need to implement removeParent/AddParent methods to remove a specified parent, not all parents.
            return moveCopyFile(fileToMove, folderDestination, false);
        }
        private bool moveCopyFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination, bool destructive)
        {
            //move a file within this account to another place within this account.

            var _googleDriveService = InitializeAPI.googleDriveService;

            //get previous parents to remove
            var parentRequest = _googleDriveService.Files.Get(fileToMove.FileID);
            parentRequest.Fields = "parents";
            var file = parentRequest.Execute();

            //this will remove all parents
            var previousParents = string.Join(",", file.Parents);

            //move file to new folder
            //var updateRequest = _googleDriveService.Files.Update(new File(), cd.FileID);
            Google.Apis.Drive.v3.Data.File temp = new Google.Apis.Drive.v3.Data.File();
            var updateRequest = _googleDriveService.Files.Update(temp, fileToMove.FileID);
            updateRequest.Fields = "id, parents";

            //TODO: switch out commented lines
            //updateRequest.AddParents = folderDestination.FileID;
            updateRequest.AddParents = "0B0F_8LaJGpURSGFMY2k5UzF0LTg";

            if (destructive)
            {
                //will only remove it from the previous parent if its a destructive call (moving), not copying.
                updateRequest.RemoveParents = previousParents;
            }

            file = updateRequest.Execute();
            return true;
        }
        //Will add a parent directory to this file
        private bool addParentToFile(CommonDescriptor fileToAdd, CommonDescriptor newParentFolder)
        {

            return true;
        }
        private bool removeParentToFile(CommonDescriptor fileToRemove, CommonDescriptor parentFolderToRemove)
        {
            return true;
        }
    }
}
