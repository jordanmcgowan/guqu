using Guqu.Models;
using Guqu.Models.SupportClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.WebServices
{
    interface ICloudCalls
    {

        //returns Memory Stream of file data
        //must be written to
        Task<bool> downloadFileAsync(CommonDescriptor cd);

        //returns TRUE if file upload was successful, 
        //else FALSE
        List<String> uploadFiles(List<UploadInfo> toUpload, CommonDescriptor folderDestination);
        Task<List<string>> uploadFilesAsync(List<UploadInfo> toUpload, CommonDescriptor folderDestination);

        //returns TRUE if file was shared successfully, 
        //else FALSE
        //TODO: pass in the list of emails to share with, AND the type of permission they should have
        bool shareFile(CommonDescriptor fileToShare);
        Task<bool> shareFileAsync(CommonDescriptor fileToShare);

        bool deleteFile(CommonDescriptor cd);
        Task<bool> deleteFileAsync(CommonDescriptor cd);

        bool moveFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination);
        Task<bool> moveFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination);

        bool copyFile(CommonDescriptor fileToMove, CommonDescriptor folderDestination);
        Task<bool> copyFileAsync(CommonDescriptor fileToMove, CommonDescriptor folderDestination);

        //returns TRUE if all meta data was fetched
        //else FALSE
        Task<bool> fetchAllMetaData(MetaDataController controller, string accountName);

        
       





    }
}