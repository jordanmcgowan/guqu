﻿using System;
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

        public async Task<bool> downloadFile(CommonDescriptor cd)
        {
            OneDriveCommunicationParser odcp = new OneDriveCommunicationParser();
            WindowsDownloadManager wdm = new WindowsDownloadManager();
            var _oneDriveClient = InitializeAPI.oneDriveClient;
            var fileId = cd.FileID;

           


            string extension = odcp.getExtension(cd.FileType);
            try {
                var contentStream = await _oneDriveClient.Drive.Items[fileId].Content.Request().GetAsync();
                wdm.downloadFile((MemoryStream)contentStream, cd.FileName + extension);
            }
            catch(Exception e)
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

        public bool shareFile(CommonDescriptor fileToShare)
        {
            throw new NotImplementedException();
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
                cd.FileSize = (long) child.Size;
                cd.LastModified = child.LastModifiedDateTime.Value.LocalDateTime;
               
               

                //need special case for folders vs. files
                if(child.File != null)
                    cd.FileType = child.File.MimeType;
                if (child.Folder != null)
                {
                    cd.FileType = "folder";

                    if(child.Folder.ChildCount > 0)
                        fetchAllMDFiles(controller, "//"+child.Name, child.Id);
                }

                controller.addCommonDescriptorFile(cd);

            }
            
        }
    }
}
