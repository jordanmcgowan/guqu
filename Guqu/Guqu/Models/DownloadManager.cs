using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    interface DownloadManager
    {
        /*
        Pulls up a file explorer window to prompt
        the user to choose a location to save their
        files to
        
        */
        void promptDownloadWindow();
    }
}
