using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Guqu.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Linq;

namespace Guqu.WebServices
{
    class OneDriveCalls : ICloudCalls
    {

        private static char[] forbiddenCharacters = new char[] { '\\', '/', '*', '"', ':', '?', '>', '<', '|' };

        
        private static string replaceProhibitedCharacters(string path)
        {
            foreach (char curChar in forbiddenCharacters)
            {
                if (path.Contains(curChar))
                {
                    //the character to replace the forbidden character ostensibly doesn't matter, just needs to be consistent.
                    path = path.Replace(curChar, '_');
                }
            }
            return path;
        }

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
            throw new NotImplementedException();
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
            
            var newParentId = folderDestination.FileID;
            var fileId = fileToMove.FileID;
            var updateItem = new Item { ParentReference = new ItemReference { Id = newParentId } };
            var itemWithUpdates = await _oneDriveClient
                                            .Drive
                                            .Items[fileId]
                                            .Request()
                                            .UpdateAsync(updateItem);

            //Move to Google Drive location
            throw new NotImplementedException();
            
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
            string curFileSerialized;

            foreach (var child in allChildren)
            {

                child.Name = replaceProhibitedCharacters(child.Name);
                curFileSerialized = serializer.Serialize(child);
                if(child.File == null) //folder
                {
                    //For each folder add the metaDataFolder, the CD, and then recurse.
                    controller.addMetaDataFolder(curFileSerialized, relativeRequestPath, child.Name);
                    curCD = oneDriveCommParser.createCommonDescriptor(relativeRequestPath, curFileSerialized);
                    controller.addCommonDescriptorFile(curCD);
                    await fetchAllMDFiles(controller, relativeRequestPath + "\\" + child.Name, child.Id);
                }
                else  //file
                {
                    //For each file add the metadatafile, and the CD.
                    controller.addMetaDataFile(curFileSerialized, relativeRequestPath, child.Name);
                    curCD = oneDriveCommParser.createCommonDescriptor(relativeRequestPath, curFileSerialized);
                    controller.addCommonDescriptorFile(curCD);

                    var s = child.File.MimeType;

                    //var a = child.File.MimeType;

                }

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
