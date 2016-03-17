using System;
using System.IO;
using System.Web.Script.Serialization;

using System.Collections.Generic;

using TreeNode = Guqu.Models.SupportClasses.TreeNode;
using System.Linq;

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
        public StreamReader getMetaDataFile(string relativeFilePath)
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(rootStoragePath + METADATAPATH + relativeFilePath);
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
        /*
        Deserialize the common descriptor file and return the object.
        */
        public CommonDescriptor getCommonDescriptorFile(string filePath)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string jsonCD = File.ReadAllText(rootStoragePath + COMMONDESCRIPTORPATH + filePath + ".json");
            CommonDescriptor cd = jsonSerializer.Deserialize<CommonDescriptor>(jsonCD);
            return cd;
        }

        /*
        Returns the absolute file path for a file to be saved at. If the path leads to a place that does not exist. 
        Path should result in a accessible location. If not the directory will be created here. 
        */
        public string getAbsoluteFilePathForAddingMDFile(string relativeFilePath)
        {
            string toReturn = rootStoragePath + METADATAPATH + relativeFilePath;
            if (!Directory.Exists(toReturn))
            {
                //create directory
                createDirectory(relativeFilePath);
            }

            /*
            //TODO: move the following to a diff module - also in WindowsDownloadManager
            char[] forbiddenCharacters = new char[] { '\\', '/', '*', '"', ':', '?', '>', '<', '|' };
            foreach (char curChar in forbiddenCharacters)
            {
                if (toReturn.Contains(curChar))
                {
                    //TODO: uncomment after errorprompt is working
                    //if any of the forbidden characters are found, return false
                    //isValid = false;

                    //temp fix, replace all bad characters with '-'
                    //fileName.Replace(curChar, '-');
                    toReturn = toReturn.Replace(curChar, '-');
                }
            }
            */
            return toReturn;
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
                File.WriteAllText(rootStoragePath + COMMONDESCRIPTORPATH + cd.FilePath + ".json", serializedJson);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("{0} InvalidOperation caught.", e);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0} ArgumentException caught.", e);
                return false;
            }
            catch (DirectoryNotFoundException e)
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
            string cdPath = rootStoragePath + COMMONDESCRIPTORPATH + filePath + ".json";

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

        
        
        public TreeNode getRoot(string account)
        {

        TreeNode root = new TreeNode(null, null);
        string rootPath = rootStoragePath + METADATAPATH + account;
        return createTree(root, rootPath);

        }

    private TreeNode createTree(TreeNode rootNode, string rootFilePath)
    {
        var folderQueue = new Queue<TreeNode>();
        //get everything in this directory. Should be only folders and CD.json files
        string[] subDirs = Directory.GetFiles(rootFilePath, "*.json", SearchOption.TopDirectoryOnly);
        FileAttributes curAttr;
        CommonDescriptor curfileCD;
        TreeNode childNode;

        foreach (string file in subDirs)
        {
            curAttr = File.GetAttributes(file);
                //is a CD.json file 
                if (!curAttr.HasFlag(FileAttributes.Directory))
                {
                    //add as children
                    curfileCD = getCommonDescriptorFile(file);
                    childNode = new TreeNode(rootNode, curfileCD);

                    //TODO: ensure that all CD store fiile type for folders as 'folder'
                    //Because each actual file is terminated with _file.json, we can be sure
                    //that the directory (which is not terminated) can be found by removing .json from the string
                    if (curfileCD.FileType.Equals("folder"))
                    {
                        //only will recurse upon directories
                        rootNode.addChild(createTree(rootNode, file.Replace(".json", "")));
                    }
                    else
                    {
                        //only add the normal files to the rootNode now.
                        rootNode.addChild(childNode);
                    }
                }

      }
            return rootNode;
   }
        /*
        Will delete both the CD and MD directory at a given relative path. If the directory does not exist, then this function won't do anything.
        */
        public Boolean removeDirectory(string relativeDirectoryPath)
        {
            string mdPath = rootStoragePath + METADATAPATH + relativeDirectoryPath;
            string cdPath = rootStoragePath + COMMONDESCRIPTORPATH + relativeDirectoryPath;

            if (Directory.Exists(mdPath))
            {
                Directory.Delete(mdPath);
            }
            if (Directory.Exists(cdPath))
            {
                Directory.Delete(cdPath);
            }
            return true;
        }
        /*
        Will attempt to create the requested directory (for CD and MD) and will recrsively create all nested directories if applicable.
        If the directry already exists, then this will not do anything. 
        */
        public Boolean createDirectory(string relativeDirectoryPath)
        {
            string mdPath = rootStoragePath + METADATAPATH + relativeDirectoryPath;
            string cdPath = rootStoragePath + COMMONDESCRIPTORPATH + relativeDirectoryPath;

            if (!Directory.Exists(mdPath))
            {
                Directory.CreateDirectory(mdPath);
            }
            if (!Directory.Exists(cdPath))
            {
                Directory.CreateDirectory(cdPath);
            }
            return true;
        }
    }
}

