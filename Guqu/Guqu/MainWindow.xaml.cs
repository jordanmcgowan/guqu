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
            DateTime dt = new DateTime(1994, 2, 9);
            cd = new CommonDescriptor("TestFile", "doc", "GoogleDrive\\NewFile", dt, 1234);
        }

        private void logoutClicked(object sender, RoutedEventArgs e)
        {
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
        private void wikiClicked(object sender, RoutedEventArgs e)
        {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            InitializeAPI api = new InitializeAPI();
            CloudLogin.googleDriveLogin();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MetaDataController controller = new MetaDataController("Shouldn't matter");
            CommonDescriptor newDesc = controller.getCommonDescriptorFile(cd.FilePath);
            
            controller.removeFile(cd.FilePath);
        }
    }
}
