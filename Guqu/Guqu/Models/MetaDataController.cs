using System;
using System.IO;
using System.Web.Script.Serialization;
namespace Guqu.Models
{
    class MetaDataController
    {
        private readonly string METADATAPATH = "\\MetaData\\";
        private readonly string COMMONDESCRIPTORPATH = "\\CommonDescriptor\\";
        private string rootStoragePath; //declares where the files are being stored

        public MetaDataController(string rootPath)
        {
            //rootpath should be defined in settng and on creation of this module, the value is passed in.
            rootStoragePath = rootPath;
            rootStoragePath = "E:\\GuquTestFolder";
        } 
        /*
        Get the StreamReader for the MetaData file
        */
        public StreamReader getMetaDataFile(string filePath)
        {
            return getStreamReader(rootStoragePath + METADATAPATH + filePath);
        }
        /*
        Deserialize the common descriptor file and return the object.
        */
        public CommonDescriptor getCommonDescriptorFile(string filePath)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string jsonCD = File.ReadAllText(rootStoragePath + COMMONDESCRIPTORPATH + filePath +".json");
            CommonDescriptor cd = jsonSerializer.Deserialize<CommonDescriptor>(jsonCD);
            return cd;
        }
        //helper method to create the stream readers
        private StreamReader getStreamReader(string filePath)
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(rootStoragePath + "\\MetaData\\" + filePath);
            }
            catch (IOException e)
            {
                Console.WriteLine("{0} IOException caught.", e);
                //todo: should we throw the exception up to the overall controller?
                //through a different exception? File Does Not Exist Exception?
            }
            if (streamReader != null)
            {
                return streamReader;
            }
            else
            {
                return null;
            }
        }
        public Boolean addMetaDataFile()
        {
            //not sure how to do this.
            //might transform into this sending a string to the download manager for where the file should be stored.
            return false;
        }
        /*
        Takes in a CommonDescriptor object, transforms it to a JSON file, and then saves it to the disk
        */
        public Boolean addCommonDescriptorFile(CommonDescriptor cd)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            try
            {
                var serializedJson = jsonSerializer.Serialize(cd);
                Console.WriteLine("JSON: " + serializedJson);
                File.WriteAllText(rootStoragePath + COMMONDESCRIPTORPATH + cd.FilePath + ".json", serializedJson);
                Console.WriteLine("Wrote file");
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("{0} InvalidOperation caught.", e);
                return false;
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("{0} ArgumentException caught.", e);
                return false;
            }
            catch(DirectoryNotFoundException e)
            {
                //TODO: Should we ever get this error? Should be handled by other procs
                Console.WriteLine("{0} DirectoryNotFoundException caught.", e);
                return false;
            }
            //TODO: other cases that need to be caught

            return true;
        }
        /*
        Removes both the CommonDescriptor file and the Actual metadata file if they exist
        */
        public Boolean removeFile(string filePath)
        {
            string mdPath = rootStoragePath + METADATAPATH + filePath + ".json";
            string cdPath = rootStoragePath + COMMONDESCRIPTORPATH + filePath +".json";

            if (File.Exists(mdPath))
            {
                File.Delete(mdPath);
            }

            if (File.Exists(cdPath))
            {
                File.Delete(cdPath);
            }
            return true;
        }

    }
}
