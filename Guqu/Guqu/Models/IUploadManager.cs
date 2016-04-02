using Guqu.Models.SupportClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    interface IUploadManager
    {
        List<UploadInfo> getUploadFiles();
    }
}
