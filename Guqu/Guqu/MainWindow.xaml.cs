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

namespace Guqu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        CommonDescriptor cd;
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





            List <string> accountServices = new List<String>();
            accountServices.Add("driveDrive");
            accountServices.Add("boxBook");
            accountServices.Add("cloudFace");

            List<string> accountNames = new List<String>();
            accountNames.Add("myUsername");
            accountNames.Add("myOtherUsername");
            accountNames.Add("myOtherOtherUsername");

            MetaDataController mdc = new MetaDataController("C:\\guquTestFolder");
            TreeNode rootnode = mdc.getRoot("test");
            MenuItem root = new MenuItem() { Title = "test" }; //label as the account name
            root = populateMenuItem(root, rootnode);
            
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
            fileTreeMenu.Items.Add(root);
            
            //Dummy data to display path
            List<string> mylist = new List<string>(new string[] { "element1", "element2", "element3", "element1", "element2", "element3", "element1", "element1", "element2", "element3", "element1", "element2", "element3", "element1", "element2", "element3", });
            String path = generatePath(mylist);
            pathBox.Text = path;


        }
        private MenuItem populateMenuItem(MenuItem root, TreeNode node)
        {
            MenuItem newFolder;
            foreach (var ele in node.getChildren())
            {
                if (ele.getCommonDescriptor().FileType.Equals("folder"))
                {
                    newFolder = new MenuItem() { Title = ele.getCommonDescriptor().FileName };
                    root.Items.Add(populateMenuItem(newFolder, ele));
                }
                else
                {
                    root.Items.Add(new MenuItem() { Title = ele.getCommonDescriptor().FileName });
                }
            }
            return root;
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
                path = path  + file + " > ";
            }
            return path;

        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            shareWindow shareWin = new shareWindow();
            shareWin.Show();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MetaDataController mdc = new MetaDataController("C:\\guquTestFolder");
            GoogleDriveCalls gdc = new GoogleDriveCalls();
            gdc.fetchAllMetaData(mdc, "test");
            //TreeNode rootNode = mdc.getRoot("test");
        }
        private void populateTree(TreeNode treeRoot, MenuItem xamlRoot)
        {
            xamlRoot = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName };
            recursiveBuildTree(treeRoot, xamlRoot);

        }

        private void recursiveBuildTree(TreeNode treeRoot, MenuItem xamlRoot)
        {
            foreach (TreeNode child in treeRoot.getChildren())
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
        }
    
    public class fileOrFolder
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Size { get; set; }

        public string DateModified { get; set; }

    }

}