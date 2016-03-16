using System;
using System.IO;
using System.Windows.Forms;

namespace Guqu.Models
{
    class WindowsDownloadManager: DownloadManager
    {
        public WindowsDownloadManager()
        {

        }
        public Boolean downloadFile(MemoryStream stream, string fileName)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select a folder to download to";
            DialogResult result = fbd.ShowDialog();
            string selectedFolderPath;
            if(result == DialogResult.OK)
            {
                selectedFolderPath = fbd.SelectedPath;
                FileStream fstream = new FileStream(selectedFolderPath + "\\" + fileName, FileMode.Create);

                try
                {
                    stream.WriteTo(fstream);
                    fstream.Close();
                    stream.Close();
                }
                catch(Exception e)
                {
                    //todo?
                }
                //File.WriteAllBytes(selectedFolderPath + "\\"+ fileName, stream.ToArray());
            }

            //odf.ShowDialog();
            //StreamReader sr = new StreamReader(odf.FileName);
            //MessageBox.Show(sr.ReadToEnd());
            return false;
        }
    }
}
