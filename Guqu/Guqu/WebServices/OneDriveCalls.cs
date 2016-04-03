using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.OneDrive.Sdk;
using Guqu.Models;

namespace Guqu.WebServices
{
    class OneDriveCalls : ICloudCalls
    {

        public Task<bool> downloadFile(CommonDescriptor cd)
        {
            throw new NotImplementedException();
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
            await _oneDriveClient.AuthenticateAsync();
            var parent = _oneDriveClient.Drive.Items[parentID];

            bool t = _oneDriveClient.IsAuthenticated;
            bool moreItemsExist = true;

            List<Item> allChildren = new List<Item>();
            IChildrenCollectionPage children;

            //try
            //{
                while (moreItemsExist)
                {
                    children = await parent.Children.Request().Top(2).GetAsync();
                    allChildren.AddRange(children);

                    if (children.NextPageRequest == null)
                    {
                        moreItemsExist = false;
                    }
                    else
                    {

                        children.InitializeNextPageRequest(_oneDriveClient, children.NextPageRequest.GetHttpRequestMessage().RequestUri.ToString());
                    }

                }
           // }
           // catch (Exception e)
           // {
           //     Console.WriteLine(e.StackTrace);
           // }

            int x = 0;

            List<String> names = new List<String>();
            foreach (var child in allChildren)
            {

                if (child.File != null) //for files
                {
                    



                }
                else //for folders
                {
                    var rar = child.Folder;

                    if (rar.ChildCount != null)
                    {
                        var id = child.Id;



                    }
                    //do thing for child.Folder

                }





            }


            

        }
    }
}
