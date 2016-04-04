using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaDataController = Guqu.Models.MetaDataController;

using Guqu.Models;

namespace Guqu.WebServices
{
    class WebCallerController
    {
        public WebCallerController()
        {

        }
        public string getFileHierarchy(string accountName)
        {
            //instantiate the metadata controller, give it the root path.
            //TODO: how to access the root path, parameter, or lookup from a file?
            //TODO: if lookup from a file, then pass nothing to mdc, have the mdc do it locally.
            MetaDataController controller = new MetaDataController("todo");
            //have the metadata controller walk through the saved files, build a path for us.


            return null;
        }
    }
}
