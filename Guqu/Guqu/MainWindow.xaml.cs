using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Guqu.Models;
using Guqu.WebServices;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using Guqu.Models.SupportClasses;

namespace Guqu
{
    public partial class MainWindow : Window
    {
        private List<Models.SupportClasses.TreeNode> roots = new List<Models.SupportClasses.TreeNode>();

        ObservableCollection<dispFolder> dF = new ObservableCollection<dispFolder>();//test for folder disp

        public User user { get; set; }

        private static WindowsUploadManager windowsUploadManager;
        private static WindowsDownloadManager windowsDownloadManager;
        private static string metaDataStorageLocation = "..\\GuquMetaDataStorageLocation";
        private static MetaDataController metaDataController;

        private Models.SupportClasses.TreeNode selectedHierarchyFolder = null;

        
        public MainWindow(User user)
        {
            this.user = user;
            InitializeComponent();
            this.Height = (SystemParameters.PrimaryScreenHeight);
            this.Width = (SystemParameters.PrimaryScreenWidth);
            this.menu1.Width = (SystemParameters.PrimaryScreenWidth);
            this.fileTreeMenu.Height = (SystemParameters.PrimaryScreenHeight) - 116; //82
            this.pathBox.Width = (SystemParameters.PrimaryScreenWidth) - 198;
            this.scrollText.Width = (SystemParameters.PrimaryScreenWidth) - 198;
            this.folderView.Width = (SystemParameters.PrimaryScreenWidth) - 193;
            this.folderView.Height = (SystemParameters.PrimaryScreenHeight) - 200;
     
            List<string> mylist = new List<string>(new string[] { "element2", "element3", "element1", "element2", "element3", });
            String path = generatePath(mylist);
            pathBox.Text = path;

            windowsDownloadManager = new WindowsDownloadManager();
            windowsUploadManager = new WindowsUploadManager();
            metaDataController = new MetaDataController(metaDataStorageLocation);
        }

        //implement this when a file/folder is clicked in services view
        private void populateListView(List<CommonDescriptor> files)
        {
            foreach (CommonDescriptor file in files)
            {
                // create new fileOrFolder Object with Checked = false but everything else from common descriptor may need to change for date and size
                dF.Add(new dispFolder() { Name = file.FileName, Type = file.FileType, Size = ""+file.FileSize, DateModified = ""+file.LastModified, Owners = "owners", Checked = false, FileID = file.FileID });
            }
            folderView.ItemsSource = dF;
        }

        private MenuItem populateMenuItem(MenuItem root, Models.SupportClasses.TreeNode node)
        {
            MenuItem newFolder;
            foreach (var ele in node.getChildren())
            {
                if (ele.getCommonDescriptor().FileType.Equals("folder"))
                {
                    newFolder = new MenuItem() { Title = ele.getCommonDescriptor().FileName , ID = ele.getCommonDescriptor().FileID};
                    roots.Add(ele);
                    newFolder.Click = new RoutedEventHandler(item_Click);
                    root.Items.Add(populateMenuItem(newFolder, ele));
                }
                else
                {
                    //root.Items.Add(new MenuItem() { Title = ele.getCommonDescriptor().FileName });
                }
            }
            return root;
        }
        public void item_Click(object sender, RoutedEventArgs e)
        {
            dF = new ObservableCollection<dispFolder>();
            TextBlock name = e.OriginalSource as TextBlock;
            String fileClicked = name.Uid;

            //badly coded 
            /*
            if (fileClicked.Equals("root"))
            {
                Models.SupportClasses.TreeNode node = roots.ElementAt(0);
                LinkedList<Models.SupportClasses.TreeNode> children = node.getChildren();
                List<CommonDescriptor> disp = new List<CommonDescriptor>();
                foreach (var item in children)
                {
                    disp.Add(item.getCommonDescriptor());
                }
                populateListView(disp);
            }
            */
            //else {
                foreach (var r in roots)
                {
                    folderDisplay(r, fileClicked);
                }
            //}
        }


        private void logoutClicked(object sender, RoutedEventArgs e)
        {
            //TODO call the function that logs out
            logInWindow logInWin = new logInWindow();
            logInWin.Show();
            this.Close();
        }
        private void moveButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO call the function that moves

        }
        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO call the function that copies

        }

        private void exitClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void checkForUpdatesClicked(object sender, RoutedEventArgs e)
        {
            //TODO call the function that makes sure things are up to date and redraws

        }


        private void manageAccountsClicked(object sender, RoutedEventArgs e)
        {
            manageCloudAccountsWindow manageAccountsWin = new manageCloudAccountsWindow(user);
            manageAccountsWin.Show();
        }


        private void changePasswordClicked(object sender, RoutedEventArgs e)
        {
            /*changePasswordWindow changePassWin = new changePasswordWindow();
            changePassWin.Show();*/
        }


        private void changePathClicked(object sender, RoutedEventArgs e)
        {
            changePathWindow changePathWin = new changePathWindow();
            changePathWin.Show();
        }


        //TODO Make actual wiki and update link 
        private void wikiClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.google.com");
        }


        private String generatePath(List<String> hierarchy)
        {
            String path = "";
            foreach (String file in hierarchy)
            {
                path = path + file + " > ";
            }
            return path;

        }

        
        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            //get the destination location
            if (selectedHierarchyFolder == null)
            {
                //can't upload without selecting
                DialogResult res = System.Windows.Forms.MessageBox.Show("Please select a folder to upload to.");
                return;
            }
            CommonDescriptor destinationLocation = selectedHierarchyFolder.getCommonDescriptor();

            //determine what controller to use (google vs one drive)
            Models.SupportClasses.TreeNode rootNode = selectedHierarchyFolder.getParent();
            while(rootNode.getParent() != null)
            {
                rootNode = rootNode.getParent();
            }
            CommonDescriptor root = rootNode.getCommonDescriptor();
            string acctType = root.FileType;


            ICloudCalls cloudCaller = null;
            //should be done with a level of obfuscation
            if (acctType.Equals("Google Drive"))
            {
                cloudCaller = new GoogleDriveCalls();
            }
            else if(acctType.Equals("One Drive"))
            {
                cloudCaller = new OneDriveCalls();
            }
            else
            {
                DialogResult res = System.Windows.Forms.MessageBox.Show("Cannot upload to this account for some reason.");
                return; //somehow nothing was set for the root node, this should be impossible.
            }
            
            //get the elements the user wants to upload
            List<UploadInfo> filesToUpload = windowsUploadManager.getUploadFiles();

            //make the calls to upload
            List<string> uploadedFileIDs;
            uploadedFileIDs = cloudCaller.uploadFiles(filesToUpload, destinationLocation);

            //now that files are uploaded

            //download the metaData from these files 
            //really bad, should have a more precise solution
            cloudCaller.fetchAllMetaData(metaDataController, root.FileName);

            //update the view
            //again a dumb solution, should be more precise
            Models.SupportClasses.TreeNode remadeRootNode = metaDataController.getRoot(root.FileName, root.FileID, root.FileType);
            fileTreeMenu.Items.Remove(rootNode);

            //attempt to 'refresh' the fileHierarchy view
            MenuItem temp = new MenuItem() { Title = root.FileName, ID = root.FileID }; //label as the account name
            roots.Remove(rootNode);
            roots.Add(remadeRootNode);
            temp = populateMenuItem(temp, remadeRootNode);
            fileTreeMenu.Items.Remove(rootNode);
            fileTreeMenu.Items.Add(remadeRootNode);

        }


        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            //get the requested files to download
            List<CommonDescriptor> filesToDownload = null;

            //get the requird controller
            ICloudCalls cloudCaller = null;

            //download the files
            foreach(CommonDescriptor curFile in filesToDownload)
            {
                cloudCaller.downloadFileAsync(curFile);
            }
            */

 
            
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.Description = "Please select a folder to download the files to.";
            //DialogResult result = fbd.ShowDialog();
            //string selectedFolderPath;
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
                //selectedFolderPath = fbd.SelectedPath;
                //MetaDataController mdc = new MetaDataController(selectedFolderPath);
                GoogleDriveCalls gdc = new GoogleDriveCalls();
                gdc.fetchAllMetaData(metaDataController, "Google Drive");

                Models.SupportClasses.TreeNode rootnode = metaDataController.getRoot("Google Drive", "root", "Google Drive");
                MenuItem root = new MenuItem() { Title = "Google Drive", ID = "root"}; //label as the account name
                root.ID = "root";
                roots.Add(rootnode);
                root = populateMenuItem(root, rootnode);
                
                

                fileTreeMenu.Items.Add(root);
           // }
            /*foreach (var hi in roots)
            {
                folderDisplay(hi, "CS564");
            }
            */

        }


        //call when a click is detected on the file hierarchy
        private void folderDisplay(Models.SupportClasses.TreeNode node, String fileID)
        {
            if (node.getCommonDescriptor() != null)
            {

                if (node.getCommonDescriptor().FileID.Equals(fileID))
                {
                    //get list of children nodes convert to a list of common discriptors and populate listView
                    LinkedList<Models.SupportClasses.TreeNode> children = node.getChildren();
                    List<CommonDescriptor> disp = new List<CommonDescriptor>();
                    selectedHierarchyFolder = node;
                    foreach (var item in children)
                    {
                        disp.Add(item.getCommonDescriptor());
                    }
                    populateListView(disp);
                }
            }
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            //TESTING CODE//
                //shareWindow shareWin = new shareWindow();
                //shareWin.Show();
                //GoogleDriveCalls gdc = new GoogleDriveCalls();
                //gdc.shareFile(cd);
        }


        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dF.Count > 0)
            {
                List<dispFolder> itemsToRemove = new List<dispFolder>();

                foreach (dispFolder file in dF)
                {
                    if (file.Checked)
                    {
                        itemsToRemove.Add(file);
                    }
                }
                foreach (dispFolder file in itemsToRemove)
                {
                    //add delete call to actual web service
                    dF.Remove(file);
                    
                }

            }
            else
            {
                System.Console.WriteLine("nothing in list");
            }

        }





        private void populateTree(Guqu.Models.SupportClasses.TreeNode treeRoot, MenuItem xamlRoot)
        {
            xamlRoot = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName , ID = treeRoot.getCommonDescriptor().FileID};
            recursiveBuildTree(treeRoot, xamlRoot);

        }

        private void recursiveBuildTree(Guqu.Models.SupportClasses.TreeNode treeRoot, MenuItem xamlRoot)
        {
            foreach (Guqu.Models.SupportClasses.TreeNode child in treeRoot.getChildren())
            {
                MenuItem currNode = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName, ID = treeRoot.getCommonDescriptor().FileID };
                recursiveBuildTree(child, currNode);
                currNode.Items.Add(new MenuItem() { Title = child.getCommonDescriptor().FileName, ID = treeRoot.getCommonDescriptor().FileID });
                xamlRoot.Items.Add(currNode);
            }
        }


    }
    public class MenuItem
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public string Title { get; set; }
        public string ID { get; set; }

        public ObservableCollection<MenuItem> Items { get; set; }
        public RoutedEventHandler Click { get; internal set; }
    }

    public class fileOrFolder
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Size { get; set; }

        public string DateModified { get; set; }

        public string Owners { get; set; }

        public bool Checked { get; set; }

    }

}