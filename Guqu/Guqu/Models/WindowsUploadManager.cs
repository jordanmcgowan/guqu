using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guqu.Models.SupportClasses;

namespace Guqu.Models
{
    class WindowsUploadManager : IUploadManager
    {
        OpenFileDialog fileDialog;

        public WindowsUploadManager()
        {
            fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.RestoreDirectory = true;
        }

        public List<UploadInfo> getUploadFiles()
        {
            List<UploadInfo> filesToUpload = new List<UploadInfo>();            
            DialogResult result = fileDialog.ShowDialog();
            string[] fileNames;
            if(result == DialogResult.OK)
            {
                UploadInfo curFileInfo;
                Stream curStream;
                string curFileName;
                fileNames = fileDialog.FileNames;
                for (int x = 0; x < fileNames.Length; x++)
                {
                    curStream = new FileStream(fileNames[x], FileMode.Open, FileAccess.Read);
                    curFileName = fileNames[x].Substring(fileNames[x].LastIndexOf("\\")+1);
                    curFileInfo = new UploadInfo(curStream, curFileName);
                    filesToUpload.Add(curFileInfo);
                }
            }


            return filesToUpload;
        }

    }
}
