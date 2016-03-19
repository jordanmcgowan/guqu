using Guqu.Models;
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
        Task<bool> downloadFile(CommonDescriptor cd);

        //returns TRUE if file upload was successful, 
        //else FALSE
        bool uploadFile(Stream stream);

        //returns TRUE if file was shared successfully, 
        //else FALSE
        bool shareFile(Stream stream);
        bool shareFile(MemoryStream stream);

        //returns TRUE if all meta data was fetched
        //else FALSE
        bool fetchAllMetaData(MetaDataController controller, string accountName);





    }
}