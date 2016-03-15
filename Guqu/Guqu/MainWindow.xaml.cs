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
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
namespace Guqu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            List<string> accountServices = new List<String>();
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
            List<User> items = new List<User>();
            items.Add(new User() { Name = "myFile1", Type = ".pdf", Size = "11 kb" , DateModified = "1/1/11"});
            items.Add(new User() { Name = "myFile2", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile3", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile4", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile5", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile6", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile7", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile8", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFile9", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilea", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFileb", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilec", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFiled", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilee", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilef", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFileg", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFileh", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilei", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilej", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilek", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilel", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilem", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilen", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFileo", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilep", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFileq", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFiler", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFiles", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilet", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFileu", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilev", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilew", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilex", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFiley", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
            items.Add(new User() { Name = "myFilez", Type = ".pdf", Size = "11 kb", DateModified = "1/1/11" });
          
            folderView.ItemsSource = items;
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
        private void settingsClicked(object sender, RoutedEventArgs e)
        {
            settingsWindow settingsWin = new settingsWindow();
            settingsWin.Show();
        }
        //TODO Make actual wiki and update link 
        private void wikiClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.google.com");
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
}
public class User
{
    public string Name { get; set; }

    public string Type { get; set; }

    public string Size { get; set; }

    public string DateModified { get; set; }

}