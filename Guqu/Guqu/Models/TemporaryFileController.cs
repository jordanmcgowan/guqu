using Guqu.Models.SupportClasses;
using System;
using System.Collections.Generic;
using System.IO;

namespace Guqu.Models
{
    class TemporaryFileController
    {
        /*
        This class is designed to store downloaded files for subsequent calls that may use them. 
        When a file needs to be moved to another service it will be downloaded then reuploaded.
        The downloaded file will be temprarily stored here and can be called upon again for 
        Continued usage, without having to redownload the file.
        */

        //holds all of the currently cached files.
        private static Dictionary<string, TemporaryFileInformation> cachedFiles;
        //the amount of time (in minutes) that a downloaded file is valid.
        private readonly int TIMEVALIDMINUTES = 10;
        //
        private string rootPath;
        public TemporaryFileController(string rootPath)
        {
            cachedFiles = new Dictionary<string, TemporaryFileInformation>();
            this.rootPath = rootPath;
        }
        /*
        Will check to see if cachedFiles contains a valid instance of the requested file.
        Returns true if it does, if false, will remove the entry in the cache, and on the disk.
        */
        public bool containsValidInstanceOfFile(string fileID)
        {
            TemporaryFileInformation curFileInfo = null;
            cachedFiles.TryGetValue(fileID, out curFileInfo);
            if(curFileInfo == null)
            {
                //was not present in the cache
                return false;
            }
            DateTime curTime = DateTime.Now;
            TimeSpan timeElapsed = DateTime.Now.Subtract(curFileInfo.TimeCreated);
            if(timeElapsed.TotalMinutes <= TIMEVALIDMINUTES)
            {
                //file is still 'fresh' enough
                return true;
            }
            return false;
        }
        public string getFileContents(string fileID)
        {
            string fileData;
            TemporaryFileInformation curFile;
            cachedFiles.TryGetValue(fileID, out curFile);
            if(curFile == null)
            {
                //file was not present
                return null;
            }
            else
            {
                try {
                    fileData = File.ReadAllText(curFile.AbsoluteFilePath);
                }
                catch(Exception e)
                {
                    //could not read from disk at the path, remove the path from the cache, attempt to clear it on disk.
                    Console.WriteLine(e.StackTrace);
                    cachedFiles.Remove(fileID);

                    //TODO: attempt to remove from disk

                    return null;
                }
                return fileData;
            }
        }
        public void downLoadFile(string id, MemoryStream fileToDownload)
        {
            string absoluteFilePath = rootPath + "\\" + id;
            FileStream fstream = new FileStream(absoluteFilePath, FileMode.Create);
            fileToDownload.WriteTo(fstream);
            fstream.Close();
            fileToDownload.Close();
            TemporaryFileInformation curFile = new TemporaryFileInformation(DateTime.Now, absoluteFilePath);
            cachedFiles.Add(id, curFile);
        }
    }
}
