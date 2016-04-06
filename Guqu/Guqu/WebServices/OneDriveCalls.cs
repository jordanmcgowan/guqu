using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.OneDrive.Sdk;
using Guqu.Models;
using System.IO;

namespace Guqu.WebServices
{
    class OneDriveCalls : ICloudCalls
    {

        public async Task<bool> downloadFileAsync(CommonDescriptor cd)
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            WindowsDownloadManager wdm = new WindowsDownloadManager();
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            var fileId = cd.FileID;




            string extension = odcp.getExtension(cd.FileType); //TODO: ensure OneDriveComParser returns the proper extension
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
            List<string> newFileIDs = new List<string>();

            return newFileIDs;
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
            throw new NotImplementedException();
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

            //throw new NotImplementedException();
        }

        public async Task<bool> moveFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            var _oneDriveClient = InitializeAPI.oneDriveClient;

            //Move within 1D
            //Need to retrieve new location, so this is wrong
            var newParentId = "6AD";
            var updateItem = new Item { ParentReference = new ItemReference { Id = newParentId } };
            var itemWithUpdates = await _oneDriveClient
                                            .Drive
                                            .Items[fileToMove.FileID]
                                            .Request()
                                            .UpdateAsync(updateItem);

            //Move to Google Drive location
            throw new NotImplementedException();
        }



        public async void fetchAllMetaData(MetaDataController controller, string accountName)
        {
            string rootId = "root";
            fetchAllMDFiles(controller, accountName, rootId);
        }

        private async void fetchAllMDFiles(MetaDataController controller, string relativeRequestPath, string parentID)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            bool t = _oneDriveClient.IsAuthenticated;
            var parent = _oneDriveClient.Drive.Items[parentID];

            List<Item> allChildren = new List<Item>();
            IChildrenCollectionPage children;

            children = await _oneDriveClient.Drive.Items[parentID].Children.Request().GetAsync();
            allChildren.AddRange(children);


            while (children.NextPageRequest != null)
            {


                string nextPageLinkString = children.NextPageRequest.GetHttpRequestMessage().RequestUri.ToString(); //this gives URL with next page token

                children.InitializeNextPageRequest(_oneDriveClient, nextPageLinkString);
                children = await children.NextPageRequest.GetAsync();
                allChildren.AddRange(children);
            }

            foreach (var child in allChildren)
            {
                CommonDescriptor cd = new CommonDescriptor();
                cd.FileID = child.Id;
                cd.FileName = child.Name;
                cd.FilePath = relativeRequestPath;
                cd.FileSize = (long)child.Size;
                cd.LastModified = child.LastModifiedDateTime.Value.LocalDateTime;



                //need special case for folders vs. files
                if (child.File != null)
                    cd.FileType = child.File.MimeType;
                if (child.Folder != null)
                {
                    cd.FileType = "folder";

                    if (child.Folder.ChildCount > 0)
                        fetchAllMDFiles(controller, "//" + child.Name, child.Id);
                }

                controller.addCommonDescriptorFile(cd);

            }

        }

        public Task<bool> shareFileAsync(CommonDescriptor fileToShare)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> copyFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            
            var newItemName = fileToMove.FileName;
            var itemId = fileToMove.FileID;
            var copyLocationId = folderDestination.FileID;
            

            /* Hard coded snippet = worked with testing!!
            var newItemName = "Copied File.docx";
            var itemId = "8FA41A1E5CF18E2B!1130";
            var copyLocationId = "8FA41A1E5CF18E2B!118";
            */

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
            throw new NotImplementedException();
        }

        public bool moveFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            throw new NotImplementedException();
        }
        public bool copyFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination)
        {
            throw new NotImplementedException();
        }
    }
}
