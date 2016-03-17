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
        MemoryStream downloadFile();

        //returns TRUE if file upload was successful, 
        //else FALSE
        bool uploadFile(File file);

        //returns TRUE if file was shared successfully, 
        //else FALSE
        bool shareFile(File file);
        bool shareFile(MemoryStream stream);

        //returns TRUE if all meta data was fetched
        //else FALSE
        bool fetchAllMetaData(MetaDataController controller);





    }
}
