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
                //do fancy manipulation
                


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
