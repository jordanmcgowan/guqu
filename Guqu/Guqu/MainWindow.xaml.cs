using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Guqu.Models;
using Guqu.WebServices;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
using Guqu.Models.SupportClasses;
using System.Windows.Forms;

namespace Guqu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private CommonDescriptor cd;
        private List<Models.SupportClasses.TreeNode> roots = new List<Models.SupportClasses.TreeNode>();
        
        ObservableCollection<dispFolder> dF = new ObservableCollection<dispFolder>();//test for folder disp


        public MainWindow()
        {

            InitializeComponent();
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth);
            this.menu1.Width = (System.Windows.SystemParameters.PrimaryScreenWidth);
            this.fileTreeMenu.Height = (System.Windows.SystemParameters.PrimaryScreenHeight) - 116; //82
            this.pathBox.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) - 198;
            this.scrollText.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) - 198;
            this.folderView.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) - 193;
            this.folderView.Height = (System.Windows.SystemParameters.PrimaryScreenHeight) - 200;

            dynamicPrompt dp = new dynamicPrompt(new String[] { "here","there","where","hi3ed3edd3d3d3d3d3ed3d3d3d3edxedxdxexexeddexexexeexexedxed"});
            dp.Show();

            //how to add to the folder view
            /*for (int i = 0; i < 10; i++)
            {
                dF.Add(new dispFolder() { Name = "File", Type = ".pdf", Size = "11", DateModified = "1/11/11", Owners = "Bill", Checked = false });
            }

            folderView.ItemsSource = dF;
            */



            /*
            MetaDataController mdc = new MetaDataController("C:\\guquTestFolder");
            TreeNode rootnode = mdc.getRoot("test");
            MenuItem root = new MenuItem() { Title = "test" }; //label as the account name
            root = populateMenuItem(root, rootnode);
            */

            /*
            foreach (var ele in rootnode.getChildren())
            {
                if (ele.getCommonDescriptor().FileType.Equals("folder"))
                {
                    newFolder = new MenuItem() { Title = ele.getCommonDescriptor().FileName };
                    root.Items.Add(newFolder);
                }
                else
                {
                    root.Items.Add(new MenuItem() { Title = ele.getCommonDescriptor().FileName });
                }
            }
            */
            //fileTreeMenu.Items.Add(root);


            //populateListView();//pass the list of common descriptors
            //Dummy data to display path
            List<string> mylist = new List<string>(new string[] { "element2", "element3", "element1", "element2", "element3", });
            String path = generatePath(mylist);
            pathBox.Text = path;
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


        private MenuItem populateMenuItem(MenuItem root, Guqu.Models.SupportClasses.TreeNode node)
        {
            MenuItem newFolder;
            foreach (var ele in node.getChildren())
            {
                if (ele.getCommonDescriptor().FileType.Equals("folder"))
                {
                    newFolder = new MenuItem() { Title = ele.getCommonDescriptor().FileName };
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
            //MenuItem name = e.OriginalSource as MenuItem;
            TextBlock name = e.OriginalSource as TextBlock;
            String tmp = name.Text;

            foreach(var root in roots)
            {
                folderDisplay(root, tmp); 
            }
        }


        private void logoutClicked(object sender, RoutedEventArgs e)
        {
            //TODO call the function that logs out
            logInWindow logInWin = new logInWindow();
            logInWin.Show();
            this.Close();
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
            manageCloudAccountsWindow manageAccountsWin = new manageCloudAccountsWindow();
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

        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (dF.Count > 0)
            {
                List<dispFolder> itemsToDownload = new List<dispFolder>();

                foreach (dispFolder file in dF)
                {
                    if (file.Checked)
                    {
                        itemsToDownload.Add(file);
                    }
                }
                foreach (dispFolder file in itemsToDownload)
                {
                    //add download logic here using file.FileID


                }
            }
            */



                FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select a folder to download the files to.";
            DialogResult result = fbd.ShowDialog();
            string selectedFolderPath;
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                selectedFolderPath = fbd.SelectedPath;
                MetaDataController mdc = new MetaDataController(selectedFolderPath);
                GoogleDriveCalls gdc = new GoogleDriveCalls();
                gdc.fetchAllMetaData(mdc, "Google Drive");

                Models.SupportClasses.TreeNode rootnode = mdc.getRoot("Google Drive");
                MenuItem root = new MenuItem() { Title = "Google Drive" }; //label as the account name
                root = populateMenuItem(root, rootnode);
                roots.Add(rootnode);
                fileTreeMenu.Items.Add(root);
            }
            /*foreach (var hi in roots)
            {
                folderDisplay(hi, "CS564");
            }
            */


            //TreeNode rootNode = mdc.getRoot("test");
        }


        //call when a click is detected on the file hierarchy
        private void folderDisplay(Models.SupportClasses.TreeNode node, String FileName)
        {
            if (node.getCommonDescriptor() != null)
            {
                if (node.getCommonDescriptor().FileName.Equals(FileName))
                {
                    //get list of children nodes convert to a list of common discriptors and populate listView
                    LinkedList<Models.SupportClasses.TreeNode> children = node.getChildren();
                    List<CommonDescriptor> disp = new List<CommonDescriptor>();
                    foreach (var item in children)
                    {
                        //if (!(item.getCommonDescriptor().FileType.Equals("folder")))
                        //{
                            disp.Add(item.getCommonDescriptor());
                        //}
                    }
                    populateListView(disp);
                }
                /*
                else {
                    foreach (var ele in node.getChildren())
                    {
                        if (ele.getCommonDescriptor().FileType.Equals("folder"))
                        {
                            folderDisplay(ele, FileName);
                        }
                    }
                }
                */
            }
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {

            shareWindow shareWin = new shareWindow();
            shareWin.Show();
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
            xamlRoot = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName };
            recursiveBuildTree(treeRoot, xamlRoot);

        }

        private void recursiveBuildTree(Guqu.Models.SupportClasses.TreeNode treeRoot, MenuItem xamlRoot)
        {
            foreach (Guqu.Models.SupportClasses.TreeNode child in treeRoot.getChildren())
            {
                MenuItem currNode = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName };
                recursiveBuildTree(child, currNode);
                currNode.Items.Add(new MenuItem() { Title = child.getCommonDescriptor().FileName });
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