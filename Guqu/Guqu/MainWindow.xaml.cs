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
        private List<List<Models.SupportClasses.TreeNode>> roots = new List<List<Models.SupportClasses.TreeNode>>();

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

            mimicLogin();
        }

        //implement this when a file/folder is clicked in services view
        private void populateListView(List<CommonDescriptor> files)
        {
            foreach (CommonDescriptor file in files)
            {
                // create new fileOrFolder Object with Checked = false but everything else from common descriptor may need to change for date and size
                dF.Add(new dispFolder() { Name = file.FileName, Type = file.FileType, Size = ""+file.FileSize, DateModified = ""+file.LastModified, Owners = "owners", Checked = false, FileID = file.FileID, CD = file});
            }
            folderView.ItemsSource = dF;
        }
        private async void mimicLogin()
        {
            InitializeAPI temp = new InitializeAPI();
            try
            {
                temp.initGoogleDriveAPI();
                await CloudLogin.googleDriveLogin();
                temp.initOneDriveAPI();
                await CloudLogin.oneDriveLogin(user);

                GoogleDriveCalls gdc = new GoogleDriveCalls();
                OneDriveCalls odc = new OneDriveCalls();
                bool goog = await gdc.fetchAllMetaData(metaDataController, "Google Drive");
                bool one = await odc.fetchAllMetaData(metaDataController, "One Drive");
            }
            catch (Exception e)
            {

            }
            finally
            {

                Models.SupportClasses.TreeNode googleRootnode = metaDataController.getRoot("Google Drive", "googleRoot", "Google Drive");
                Models.SupportClasses.TreeNode oneDriveRootnode = metaDataController.getRoot("One Drive", "driveRoot", "One Drive");
                hierarchyAdd(googleRootnode);
                hierarchyAdd(oneDriveRootnode);
            }
        }

        private MenuItem populateMenuItem(MenuItem root, Models.SupportClasses.TreeNode node, List<Models.SupportClasses.TreeNode> folders)
        {
            MenuItem newFolder;
            foreach (var ele in node.getChildren())
            {
                if (ele.getCommonDescriptor().FileType.Equals("folder"))
                {
                    newFolder = new MenuItem() { Title = ele.getCommonDescriptor().FileName, ID = ele.getCommonDescriptor().FileID };
                    folders.Add(ele);
                    newFolder.Click = new RoutedEventHandler(item_Click);
                    root.Items.Add(populateMenuItem(newFolder, ele, folders));
                }
                else
                {
                    //root.Items.Add(new MenuItem() { Title = ele.getCommonDescriptor().FileName });
                }
            }
            return root;
        }

        private void hierarchyAdd(Models.SupportClasses.TreeNode newRoot)
        {
            MenuItem root = new MenuItem() { Title = newRoot.getCommonDescriptor().FileName, ID = newRoot.getCommonDescriptor().FileID }; //label as the account name
            List<Models.SupportClasses.TreeNode> newList = new List<Models.SupportClasses.TreeNode>();
            newList.Add(newRoot);
            roots.Add(newList);
            root = populateMenuItem(root, newRoot, newList);
            fileTreeMenu.Items.Add(root);
        }
        private void hierarchyDelete(Models.SupportClasses.TreeNode root)
        {
            MenuItem rootToRemove = null;
            foreach (var item in fileTreeMenu.Items)
            {
                if (item.GetType() == typeof(MenuItem))
                {
                    MenuItem file = (MenuItem)item;
                    if (file.ID.Equals(root.getCommonDescriptor().FileID))
                    {
                        rootToRemove = file;
                    }
                }
            }
            if (rootToRemove != null)
            {
                //deleteMenuItems(rootToRemove);
                //roots = new List<Models.SupportClasses.TreeNode>();
                fileTreeMenu.Items.Remove(rootToRemove);
                for (int j = 0; j < roots.Count; j++)
                {
                    if (roots.ElementAt(j).ElementAt(0).getCommonDescriptor().FileID.Equals(rootToRemove.ID))
                    {
                        roots.RemoveAt(j);
                    }
                }
            }
        }

        /* public void deleteMenuItems(MenuItem item)
         {
             foreach (var menuItem in item.Items)
             {
                 if (menuItem.Items.Count != 0)
                 {
                     deleteMenuItems(menuItem);
                 }
                 //List<MenuItem> menList = new List<MenuItem>();
                 for(int i = 0; i < menuItem.Items.Count; i++)
                 {
                     //  menList.Add(child);
                     for(int x = 0; x < roots.Count; x++)
                     {
                         if (menuItem.Items.ElementAt(i).ID.Equals(roots.ElementAt(i).getCommonDescriptor().FileID))
                         {
                             roots.RemoveAt(x);
                         }
                     }
                 }
             }
         }*/
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
            foreach (var list in roots)
            {
                foreach (var r in list)
                {
                    folderDisplay(r, fileClicked);
                }
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
            List<Guqu.Models.SupportClasses.TreeNode> move = new List<Models.SupportClasses.TreeNode>();
            for ( int i = 0; i < roots.Count; i++){
                move.Add(roots.ElementAt(i).ElementAt(0));
            }
            Models.SupportClasses.TreeNode selected;
            moveView mv = new moveView(move);
            mv.ShowDialog();
            if (mv.getOK())
            {
                selected  = mv.getSelected();
            }
            
            //do move logic here
        }
        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO call the function that copies
            List<Guqu.Models.SupportClasses.TreeNode> move = new List<Models.SupportClasses.TreeNode>();
            for (int i = 0; i < roots.Count; i++)
            {
                move.Add(roots.ElementAt(i).ElementAt(0));
            }
            Models.SupportClasses.TreeNode selected;
            moveView mv = new moveView(move);
            mv.ShowDialog();
            if (mv.getOK())
            {
                selected = mv.getSelected();
            }
            //do copy logic here
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
            Process.Start("https://github.com/jordanmcgowan/guqu/wiki");
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
            Models.SupportClasses.TreeNode rootNode = selectedHierarchyFolder;
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

            //attempt to 'refresh' the fileHierarchy view
            MenuItem temp = new MenuItem() { Title = root.FileName, ID = root.FileID }; //label as the account name

            hierarchyDelete(rootNode);
            hierarchyAdd(remadeRootNode);

        }


        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {

            //get selected items to download
            List<CommonDescriptor> filesToDownload = new List<CommonDescriptor>();
            //get the controller
            ICloudCalls cloudCaller = null;
            //download
            foreach (dispFolder file in dF)
            {
                if (file.Checked)
                {
                    filesToDownload.Add(file.CD);
                }
            }

            if(filesToDownload.First().AccountType.Equals("Google Drive"))
            {
                cloudCaller = new GoogleDriveCalls();
            }
            else
            {
                cloudCaller = new OneDriveCalls();
            }

            foreach (CommonDescriptor curFile in filesToDownload)
            {
                cloudCaller.downloadFileAsync(curFile);
            }
            



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
            /*
            ICloudCalls cloudCaller = null;
            if (dF.Count > 0)
            {
                List<dispFolder> itemsToShare = new List<dispFolder>();
                if (itemsToShare.First().CD.AccountType == "Google Drive")
                {
                    cloudCaller = new GoogleDriveCalls();
                }
                else if (itemsToShare.First().CD.AccountType == "One Drive")
                {
                    cloudCaller = new OneDriveCalls();
                }
                else
                {
                    //failure
                    return;
                }

                foreach (dispFolder file in dF)
                {
                    if (file.Checked)
                    {
                        itemsToShare.Add(file);
                    }
                }

                foreach (dispFolder file in itemsToShare)
                {
                    //add delete call to actual web service
                    dF.Remove(file);
                    //cloudCaller.deleteFile(file.CommonDescriptor);

                }

            }
            else
            {
                Console.WriteLine("nothing in list");
            }
        */

        }


        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            ICloudCalls cloudCaller = null;
            

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

                if(itemsToRemove.First().CD.AccountType == "Google Drive")
                {
                    cloudCaller = new GoogleDriveCalls();
                }
                else if(itemsToRemove.First().CD.AccountType == "One Drive")
                {
                    cloudCaller = new OneDriveCalls();
                }
                else
                {
                    //failure
                    return;
                }

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
                    cloudCaller.deleteFile(file.CD);
                }

            }
            else
            {
                Console.WriteLine("nothing in list");
            }


            CommonDescriptor cd;
            Models.SupportClasses.TreeNode originalrootNode = selectedHierarchyFolder;
            
            while (originalrootNode.getParent() != null)
            {
                originalrootNode = originalrootNode.getParent();
            }
            cd = originalrootNode.getCommonDescriptor();
            cloudCaller.fetchAllMetaData(metaDataController, cd.FileName);
            Models.SupportClasses.TreeNode remadeRootNode = metaDataController.getRoot(cd.FileName, cd.FileID, cd.AccountType);
            hierarchyDelete(originalrootNode);
            hierarchyAdd(remadeRootNode);
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