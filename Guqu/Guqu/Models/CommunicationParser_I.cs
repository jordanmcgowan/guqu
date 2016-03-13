using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    interface CommunicationParser_I
    {
        /*
        *Create a commonDescriptor from the information available in the
        *original metadata file retrieved from the cloud storage servce.
        */
        CommonDescriptor createCommonDescriptor(string filePath);

        /*
        *Create a string that can be inserted into an API call that will
        *define the required information for the specific call as defined 
        *by the service descriptor 
        */
        string createUploadBody(ServiceDescriptor descriptor);

    }
}
