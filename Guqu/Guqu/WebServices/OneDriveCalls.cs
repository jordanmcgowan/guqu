using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Guqu.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Linq;
using System.Windows;

namespace Guqu.WebServices
{
    public class OneDriveCalls : ICloudCalls
    {

        OneDriveCommunicationParser oneDriveCommParser;
        public OneDriveCalls()
        {
            oneDriveCommParser = new OneDriveCommunicationParser();
        }

        

        public async Task<bool> downloadFileAsync(CommonDescriptor cd)
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            WindowsDownloadManager wdm = new WindowsDownloadManager();
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            _oneDriveClient.AuthenticateAsync();

            var fileId = cd.FileID;
            
            string extension = odcp.getExtension(cd.FileType); 
            try
            {
                var contentStream = await _oneDriveClient.Drive.Items[fileId].Content.Request().GetAsync();
                wdm.downloadFile((MemoryStream)contentStream, cd.FileName + extension);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;


        }

        public List<string> uploadFiles(List<Models.SupportClasses.UploadInfo> toUpload, CommonDescriptor folderDestination)
        {
            uploadFilesAsync(toUpload, folderDestination);
            return null;
           
        }

        public async Task<List<string>> uploadFilesAsync(List<Models.SupportClasses.UploadInfo> toUpload, CommonDescriptor folderDestination)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            List<string> newFileIDs = new List<string>();
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            string fileName, mimeType;



            foreach (Models.SupportClasses.UploadInfo ui in toUpload)
            {
                FileStream fileStream = (FileStream)ui.getFileStream();

                fileName = ui.getFileName();
                string fullPath = folderDestination.FilePath + fileName;


                //need file path
                var uploadedItem = await _oneDriveClient.Drive.Root.ItemWithPath(fullPath).Content.Request().PutAsync<Item>(fileStream);
                fileStream.Close();
                newFileIDs.Add(uploadedItem.Id);


            }

            return newFileIDs;
        }

        public bool shareFile(CommonDescriptor fileToShare)
        {
            shareFileAsync(fileToShare);
            return true;
        }
        public bool shareFile(CommonDescriptor fileToShare, string email, string role, string message)
        {
            shareFileAsync(fileToShare);
            return true;
        }


        public async Task<bool> deleteFileAsync(CommonDescriptor cd)
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            var fileId = cd.FileID;

            

            try
            {
                await _oneDriveClient.Drive.Items[fileId].Request().DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
            
        }

        public async Task<bool> moveFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
 
            //Move within 1D
            
            var newParentId = folderDestination.FileID;
            var fileId = fileToMove.FileID;
            var updateItem = new Item { ParentReference = new ItemReference { Id = newParentId } };
            try {
                var itemWithUpdates = await _oneDriveClient
                                                .Drive
                                                .Items[fileId]
                                                .Request()
                                                .UpdateAsync(updateItem);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
            

            
            
        }



        public async Task<bool> fetchAllMetaData(MetaDataController controller, string accountName)
        {
            string rootId = "root";
            try {
                await fetchAllMDFiles(controller, accountName, rootId);   
                return true;
            }
            catch(Exception e)
            {
                //TODO: what do we do with errors?
                Console.WriteLine(e.StackTrace);
            }
            return false;
        }

        private async Task fetchAllMDFiles(MetaDataController controller, string relativeRequestPath, string parentID)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;

            //will hold all of the children from this directory (parentID)
            List<Item> allChildren = new List<Item>();

            //will hold the current children gathered from an iteration, oneDrive returns 200 children by default
            IChildrenCollectionPage curChildren;
            curChildren = await _oneDriveClient.Drive.Items[parentID].Children.Request().GetAsync();


            allChildren.AddRange(curChildren);
            //NextPageRequest is not null iff there are more children to fetch
            while (curChildren.NextPageRequest != null)
            {
                //Get the nextPageRequest, will be null if there isn't anymore.
                string nextPageLinkString = curChildren.NextPageRequest.GetHttpRequestMessage().RequestUri.ToString();
                curChildren.InitializeNextPageRequest(_oneDriveClient, nextPageLinkString);
                curChildren = await curChildren.NextPageRequest.GetAsync();
                allChildren.AddRange(curChildren);
            }

            //Have all of the children, now iterate through them
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CommonDescriptor curCD;
            string curFileSerialized, cleansedName;

            foreach (var child in allChildren)
            {
                
                curFileSerialized = serializer.Serialize(child);
                if(child.File == null) //folder
                {
                    //For each folder add the metaDataFolder, the CD, and then recurse.
                    cleansedName = controller.addMetaDataFolder(curFileSerialized, relativeRequestPath, child.Name);
                    curCD = oneDriveCommParser.createCommonDescriptor(relativeRequestPath, curFileSerialized);
                    controller.addCommonDescriptorFile(curCD);
                    await fetchAllMDFiles(controller, relativeRequestPath + "\\" + cleansedName, child.Id);
                }
                else  //file
                {
                    //For each file add the metadatafile, and the CD.
                    cleansedName = controller.addMetaDataFile(curFileSerialized, relativeRequestPath, child.Name);
                    curCD = oneDriveCommParser.createCommonDescriptor(relativeRequestPath, curFileSerialized);
                    controller.addCommonDescriptorFile(curCD);

                    var s = child.File.MimeType;

                    //var a = child.File.MimeType;

                }

            }

        }

        public async Task<bool> shareFileAsync(CommonDescriptor fileToShare)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            var itemID = fileToShare.FileID;
            try {
                var request = _oneDriveClient.Drive.Items[itemID].CreateLink("edit").Request();
                var result = await request.PostAsync();
                var shareLink = result.Link;
                //the ability to send invitations is only in 'beta' and is not included in the SDK.
                //the only thing we can do is create a link that the user can send to people.
                //the link works, and they 'stack', that is if shared multiple times, many links (all valid)
                //will be created. The only way to remove them is to go into the one drive website and delete it manually.

                
                string message = "Here is the link to share the file " + fileToShare.FileName + ":\n " + shareLink.WebUrl;
                MessageBox.Show(message);
                
                //prompt is not showing the message
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> copyFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            
            var newItemName = fileToMove.FileName;
            var itemId = fileToMove.FileID;
            var copyLocationId = folderDestination.FileID;
            
            try
            {
                var request = await _oneDriveClient.Drive.
                    Items[itemId]
                    .Copy(newItemName, new ItemReference { Id = copyLocationId }).Request().PostAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;

        }

        public bool deleteFile(CommonDescriptor cd)
        {
            deleteFileAsync(cd);
            return true;
        }

        public bool moveFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            moveFileAsync(fileToMove, folderDestination);
            return true;
        }
        public bool copyFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            copyFileAsync(fileToMove, folderDestination);
            return true;
        }
    }
}
