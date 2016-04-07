using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Guqu.Models
{
    class WindowsDownloadManager: IDownloadManager
    {
        private static char[] forbiddenCharacters = new char[] { '\\', '/', '*', '"', ':', '?', '>', '<', '|' };
        public WindowsDownloadManager()
        {

        }
        public Boolean downloadFile(MemoryStream stream, string fileName)
        {
            while (!isValid(fileName))
            {


                if(!getValidFileName(fileName, out fileName))
                {
                    //if the user did not want to rename, we exit.
                    return false;
                }
                //if the user renames, we recheck the filename and continue.

            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select a folder to download the file " + fileName + " to.";
            DialogResult result = fbd.ShowDialog();
            string selectedFolderPath;
            if (result == DialogResult.OK)
            {
                selectedFolderPath = fbd.SelectedPath;
                FileStream fstream = new FileStream(selectedFolderPath + "\\" + fileName, FileMode.Create);
                stream.WriteTo(fstream);
                fstream.Close();
                stream.Close();
            }
            //release resources
            fbd.Dispose();
            return true;
        }

        private bool getValidFileName(string fileName, out string fixedFileName)
        {
            //TODO: launch the error prompt.
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.AppendLine("Windows files cannot have the following characters in them: \\ / * ? | < > \" :");
            errorMessage.AppendLine("The file you are trying to download, " + fileName + " cannot be used because it contains one or more of these characters.");
            errorMessage.AppendLine("Please pick a new name for this file, Note: the name of this file will NOT be changed on the cloud");

            string newFileNameHeader = "New name for the file:";
            string[] headers = new string[2];
            headers[0] = errorMessage.ToString();
            headers[1] = newFileNameHeader;

            dynamicPrompt errorPrompt = new dynamicPrompt(headers);

            //blocks: TODO change to return a ResultDialog type
            errorPrompt.ShowDialog();
            if (errorPrompt.getOK())
            {
                //user accepted
                string[] responses = errorPrompt.getRet();
                fixedFileName = responses[0];
                return true;
            }
            else
            {
                fixedFileName = null;
                return false;
            }

        }

        /*
        Searches through a filename and determines if it has any forbidden characters
        */
        private bool isValid(string fileName)
        {

            foreach (char curChar in forbiddenCharacters)
            {
                if (fileName.Contains(curChar))
                {
                    return false;
                }
            }
            //if none are found, return true
            return true;
        }
    }
}
