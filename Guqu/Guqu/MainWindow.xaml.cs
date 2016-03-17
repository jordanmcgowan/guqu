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


            //Dummy data to prove scrolling works for file tree view
            //TODO figure out how data is being brought in
            //TODO figure out how make tree from that data, neither of these will prolly be bad
            MenuItem root = new MenuItem() { Title = "Menu" };
            MenuItem childItem1 = new MenuItem() { Title = "Child item #1" };
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            MenuItem childItem2 = new MenuItem() { Title = "Child item #1" };
            childItem2.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem2.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            MenuItem childItem3 = new MenuItem() { Title = "Child item #1" };

            childItem3.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem3.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem3.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem3.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            childItem2.Items.Add(childItem3);
            childItem1.Items.Add(childItem2);
            root.Items.Add(childItem1);

            root.Items.Add(new MenuItem() { Title = "Child item #3" });
            fileTreeMenu.Items.Add(root);

            //Dummy data for folderView
            List<fileOrFolder> items = new List<fileOrFolder>();
            items.Add(new fileOrFolder() { Name = "myFile1", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile2", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile3", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile4", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile5", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile6", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile7", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile8", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFile9", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilea", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFileb", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilec", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFiled", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilee", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilef", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFileg", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFileh", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilei", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilej", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilek", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilel", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilem", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilen", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFileo", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilep", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFileq", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFiler", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFiles", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilet", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFileu", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilev", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilew", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilex", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFiley", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new fileOrFolder() { Name = "myFilez", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });

            folderView.ItemsSource = items;


            //Dummy data to display path
            List<string> mylist = new List<string>(new string[] { "element1", "element2", "element3", "element1", "element2", "element3", "element1", "element1", "element2", "element3", "element1", "element2", "element3", "element1", "element2", "element3", });
            String path = generatePath(mylist);
            pathBox.Text = path;


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