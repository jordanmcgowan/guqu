using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guqu.Models;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using System.Web.Script.Serialization;
using Guqu.Models.SupportClasses;
using Google.Apis.Requests;
using Google.Apis.Drive.v3.Data;
using System.IO;

namespace Guqu.WebServices
{
    public class GoogleDriveCalls : ICloudCalls
    {

        private GoogleDriveCommunicationParser googleCommParser;


        public GoogleDriveCalls()
        {
            googleCommParser = new GoogleDriveCommunicationParser();
        }

        public async Task<List<string>> uploadFilesAsync(List<UploadInfo> toUpload, CommonDescriptor folderDestination)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> downloadFileAsync(CommonDescriptor cd)
        {

            var _googleDriveService = InitializeAPI.googleDriveService;

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

        public async Task<bool> fetchAllMetaData(MetaDataController controller, string accountName)
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
            FileList exec;

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

            Google.Apis.Drive.v3.Data.File curFile;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string curFileSerialized;
            string cleansedName;
            CommonDescriptor curCD;
            
            while (allFiles.Count != 0) //while there are elements
            {
                curFile = allFiles.First(); //get first file 
                curFileSerialized = serializer.Serialize(curFile);
                //pathForFile = controller.getAbsoluteFilePathForAddingMDFile(relativeRequestPath);
          
                
                if (curFile.MimeType.Equals(googleFolderName)) //folder
                {
                    //For each folder add the metaDataFolder, the CD, and then recurse.
                    cleansedName = controller.addMetaDataFolder(curFileSerialized, relativeRequestPath, curFile.Name);
                    curCD = googleCommParser.createCommonDescriptor(relativeRequestPath, curFileSerialized);
                    controller.addCommonDescriptorFile(curCD);
                    fetchAllMDFiles(controller, relativeRequestPath + "\\" + cleansedName, curFile.Id);
                }
                else  //file
                {
                    //For each file add the metadatafile, and the CD.                    

                    //cleansedName is the 'clean' name of the curFile.Name, don't need to use it cause this terminates.
                    cleansedName = controller.addMetaDataFile(curFileSerialized, relativeRequestPath, curFile.Name);
                    curCD = googleCommParser.createCommonDescriptor(relativeRequestPath,curFileSerialized);
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

        public bool shareFile(CommonDescriptor fileToShare , string role , string email, string optionalMessage)
        {
            
            try {
                Console.WriteLine("STARTED SHARING");
                var _googleDriveService = InitializeAPI.googleDriveService;
                var batch = new BatchRequest(_googleDriveService);
                BatchRequest.OnResponse<Permission> callback = delegate (
                    Permission permission,
                    RequestError error,
                    int index,
                    System.Net.Http.HttpResponseMessage message)
                {
                    if (error != null)
                    {
                    // Handle error
                    Console.WriteLine(error.Message);
                    }
                    else
                    {
                        Console.WriteLine("Permission ID: " + permission.Id);
                    }

                };

                //TODO: launch the share window view, get the email address
                //shareWindow window = new shareWindow();
                //window.Show();
                //get the informaiton


                //TODO: replace these permissions with the permissions entered on the shareWindow
                Permission userPermission = new Permission();
                userPermission.Type = "user";
                userPermission.Role = role; //TODO. pick the correct role
                userPermission.EmailAddress = email; //TODO, enter the email address

                var request = _googleDriveService.Permissions.Create(userPermission, fileToShare.FileID);
                request.Fields = "id";
                request.EmailMessage = optionalMessage; //TODO enter message
                batch.Queue(request, callback);              

                var task = batch.ExecuteAsync();
            }
            catch(Exception e)
            {
                //caught a bug
                Console.WriteLine(e.Message);
            }


            return true;
        }

        public List<string> uploadFiles(List<UploadInfo> toUpload, CommonDescriptor folderDestination)
        {
            List<string> newFileIDs = new List<string>();
            var _googleDriveService = InitializeAPI.googleDriveService;
            GoogleDriveCommunicationParser gdcp = new GoogleDriveCommunicationParser();
            FilesResource.CreateMediaUpload request;
            string mimeType, fileName;

            foreach(UploadInfo uInfo in toUpload)
            {
                //get the fileType using the gdcp conversion method.
                fileName = uInfo.getFileName();
                mimeType = gdcp.getMimeType(fileName.Substring(fileName.IndexOf('.')));

                Google.Apis.Drive.v3.Data.File fileMetaData = new Google.Apis.Drive.v3.Data.File();
                fileMetaData.Name = fileName;

                fileMetaData.Parents = new List<string> {folderDestination.FileID};

                request = _googleDriveService.Files.Create(fileMetaData, uInfo.getFileStream(), mimeType);
                request.Fields = "id";
                request.Upload();
                uInfo.getFileStream().Close();
                newFileIDs.Add(request.ResponseBody.Id);
            }
            
            return newFileIDs;

        }

        public bool deleteFile(CommonDescriptor cd)
        {
            try {
                var _googleDriveService = InitializeAPI.googleDriveService;
                var request = _googleDriveService.Files.Delete(cd.FileID);
                request.Execute();

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

            
            updateRequest.AddParents = folderDestination.FileID;
            

            if (destructive)
            {
                //will only remove it from the previous parent if its a destructive call (moving), not copying.
                updateRequest.RemoveParents = previousParents;
            }

            file = updateRequest.Execute();
            return true;
        }
        //Following two methods are for option B in comments on line 277
        private bool addParentToFile(CommonDescriptor fileToAdd, CommonDescriptor newParentFolder)
        {

            return true;
        }
        private bool removeParentToFile(CommonDescriptor fileToRemove, CommonDescriptor parentFolderToRemove)
        {
            return true;
        }
        

        public Task<bool> shareFileAsync(CommonDescriptor fileToShare)
        {
            throw new NotImplementedException();
        }

        public Task<bool> deleteFileAsync(CommonDescriptor cd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> moveFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            throw new NotImplementedException();
        }

        public Task<bool> copyFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            throw new NotImplementedException();
        }
    }
}
