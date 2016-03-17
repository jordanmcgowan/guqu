using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guqu.Models;
using Google.Apis.Download;

namespace Guqu.WebServices
{
    class GoogleDriveCalls : ICloudCalls
    {
        public async Task<bool> downloadFile(Google.Apis.Drive.v3.Data.File file)
        {
            var _googleDriveService = InitializeAPI.googleDriveService;
            var _file = file;
            
            Console.WriteLine("********");
            Console.WriteLine(file.Id);
            Console.WriteLine(_file.Name);
            Console.Write(_file.MimeType);
            Console.WriteLine("********");


            var request = _googleDriveService.Files.Export(_file.Id, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            var stream = new MemoryStream();
            WindowsDownloadManager wdm = new WindowsDownloadManager();



            request.MediaDownloader.ProgressChanged +=
            (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };

            try
            {
                await request.DownloadAsync(stream);
                //TODO: not always a .doc, change.
                wdm.downloadFile(stream, file.Name + ".doc");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }


            return true;

        }

        public bool fetchAllMetaData(MetaDataController controller)
        {
            throw new NotImplementedException();
        }

        public bool shareFile(MemoryStream stream)
        {
            throw new NotImplementedException();
        }

        public bool shareFile(Stream stream)
        {
            throw new NotImplementedException();
        }

        public bool uploadFile(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
