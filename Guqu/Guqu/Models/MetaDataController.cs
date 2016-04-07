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
        private static char[] forbiddenCharacters = new char[] { '\\', '/', '*', '"', ':', '?', '>', '<', '|' };
        private readonly string METADATAPATH = "\\MetaData\\";
        private readonly string COMMONDESCRIPTORPATH = "\\CommonDescriptor\\";
        private string rootStoragePath; //declares where the files are being stored

      
        public MetaDataController(string rootPath)
        {
            //rootpath should be defined in settng and on creation of this module, the value is passed in.
            rootStoragePath = rootPath;
            //create MetaData/Common descriptor folders if they don't exist.
            createDirectory("");
            //rootStoragePath = "E:\\GuquTestFolder";
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
        private CommonDescriptor getCommonDescriptorFile(string relativeFilePath)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string filePath = rootStoragePath + COMMONDESCRIPTORPATH + relativeFilePath;
            string jsonCD = File.ReadAllText(filePath);
            CommonDescriptor cd = jsonSerializer.Deserialize<CommonDescriptor>(jsonCD);
            return cd;
        }

        private string getAbsoluteFilePathForAddingMDFile(string relativeFilePath)
        {
            string path = rootStoragePath + METADATAPATH + relativeFilePath;     
            createDirectory(relativeFilePath);
            return path;
        }
        private string getAbsoluteFilePathForAddingCDFile(string relativeFilePath)
        {
            string path = rootStoragePath + COMMONDESCRIPTORPATH + relativeFilePath;
            createDirectory(relativeFilePath);
            return path;

        }
        public string addMetaDataFile(string fileJsonData, string relativeFilePath, string fileName)
        {
            fileName = replaceProhibitedCharacters(fileName);
            string fileLocation = getAbsoluteFilePathForAddingMDFile(relativeFilePath);
            File.WriteAllText(fileLocation + "\\" + fileName + "_file.json", fileJsonData);
            return fileName;
        }
        public string addMetaDataFolder(string folderJsonData, string relativeFilePath, string folderName)
        {
            folderName = replaceProhibitedCharacters(folderName);
            string folderDirectory = getAbsoluteFilePathForAddingMDFile(relativeFilePath);
            createDirectory(relativeFilePath + "\\" + folderName);
            File.WriteAllText(folderDirectory + "\\" + folderName + "_folder.json", folderJsonData);
            return folderName;
        }
        /*
        Takes in a CommonDescriptor object, transforms it to a JSON file, and then saves it to the disk
        */
        public void addCommonDescriptorFile(CommonDescriptor cd)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string filePath = getAbsoluteFilePathForAddingCDFile(cd.FilePath); //this filePath should be cleansed from CD creation.
            var serializedJson = jsonSerializer.Serialize(cd);
            if (cd.FileType.Equals("folder"))
            {
                File.WriteAllText(filePath + "\\" + replaceProhibitedCharacters(cd.FileName) + "_folder.json", serializedJson);
            }
            else
            {
                File.WriteAllText(filePath + "\\" + replaceProhibitedCharacters(cd.FileName) + "_file.json", serializedJson);
            }
        }
        public Boolean deleteCloudObjet(CommonDescriptor cd)
        {
            if (cd.FileType.Equals("folder"))
            {
                return removeDirectory(cd.FilePath);
            }
            else
            {
                return removeFile(cd.FilePath, cd.FileName);
            }
        }

        /*
        Removes both the CommonDescriptor file and the Actual metadata file if they exist
        */
        public Boolean removeFile(string filePath, string fileName)
        {
            string mdPath = rootStoragePath + METADATAPATH + filePath + "\\" + fileName + "_file.json";
            string cdPath = rootStoragePath + COMMONDESCRIPTORPATH + filePath + "\\" + fileName + "_file.json";

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

        
        
        public TreeNode getRoot(string accountName, string rootID, string cloudType)
        {
            //root has no parent, and a bastardized CD.
            CommonDescriptor rootDescriptor = new CommonDescriptor(accountName, cloudType, accountName, rootID, new DateTime(),  0);
            TreeNode root = new TreeNode(null, rootDescriptor);
            string rootPath = rootStoragePath + COMMONDESCRIPTORPATH + accountName;
            return createTree(root, rootPath);

        }

    private TreeNode createTree(TreeNode rootNode, string rootFilePath)
    {
        //get everything in this directory. Should be only folders and CD.json files
        string[] subDirs = Directory.GetFiles(rootFilePath, "*.json", SearchOption.TopDirectoryOnly);
        CommonDescriptor curfileCD;
        TreeNode childNode;
        foreach (string file in subDirs)
        {
                //only reading .json files

                curfileCD = getCDforTreeCreation(file);
                //add as a child of rootNode
                childNode = new TreeNode(rootNode, curfileCD);
                   
                //find directory by removing the _folder.json from the name of the file.
                if (curfileCD.FileType.Equals("folder"))
                {
                    //TODO: do I get the folder object in the tree?
                    //only will recurse upon directories
                    rootNode.addChild(createTree(childNode, file.Replace("_folder.json", "")));
                }
                else
                {
                    //only add the normal files to the rootNode now.
                    rootNode.addChild(childNode);
                }
                

      }
            return rootNode;
   }
        private CommonDescriptor getCDforTreeCreation(string filePath)
        {
            string reducedFilePath = filePath.Replace(rootStoragePath + COMMONDESCRIPTORPATH, "");
            return getCommonDescriptorFile(reducedFilePath);
        }
        /*
        Will delete both the CD and MD directory at a given relative path. If the directory does not exist, then this function won't do anything.
        */
        private Boolean removeDirectory(string relativeDirectoryPath)
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
        private Boolean createDirectory(string relativeDirectoryPath)
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

        private static string replaceProhibitedCharacters(string path)
        {
            foreach (char curChar in forbiddenCharacters)
            {
                if (path.Contains(curChar))
                {
                    //the character to replace the forbidden character ostensibly doesn't matter, just needs to be consistent.
                    path = path.Replace(curChar, '_');
                }
            }
            return path;
        }
    }
}

