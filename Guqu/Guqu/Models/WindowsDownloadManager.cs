using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Guqu.Models
{
    class WindowsDownloadManager: DownloadManager
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

                //try
                //commented out try catch to see what errors we recieve.
                //{
                    stream.WriteTo(fstream);
                    fstream.Close();
                    stream.Close();
                //}
                //catch (Exception e)
                //{
                    //todo?
                //}
            }

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
            string[] headers = new string[1];
            headers[0] = newFileNameHeader;

            //Launch error prompt,

            //if cancel clicked
            //fixedFileName = fileName;
            //return false;

            //if ok clicked
            //fixedFileName = errorPrompt.getresponse[0];
            //return true;

            fixedFileName = fileName.Replace('/', '-');
            return true;

        }

        private bool isValid(string fileName)
        {

            bool isValid = true;
            foreach (char curChar in forbiddenCharacters)
            {
                if (fileName.Contains(curChar))
                {
                    //TODO: uncomment after errorprompt is working
                    //if any of the forbidden characters are found, return false
                    //isValid = false;

                    //temp fix, replace all bad characters with '-'
                    //fileName.Replace(curChar, '-');
                }
            }
            //if none are found, return true
            return true;
        }
    }
}
