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
using System.Windows.Shapes;

using Guqu.WebServices;

namespace Guqu
{
    /// <summary>
    /// Interaction logic for cloudPicker.xaml
    /// </summary>
    public partial class cloudPicker : Window
    {
        InitializeAPI api;
        public cloudPicker()
        {
            InitializeComponent();
            api = new InitializeAPI();
        }

        private void boxClick(object sender, RoutedEventArgs e)
        {
            //InitializeAPI api = new InitializeAPI();
            
            //cloudLoginWindow cloudLogWin = new cloudLoginWindow("box");
            //cloudLogWin.Show();
            this.Close();
        }
        private void oneDriveClick(object sender, RoutedEventArgs e)
        {

            api.initOneDriveAPI();
            CloudLogin.oneDriveLogin();
            //cloudLoginWindow cloudLogWin = new cloudLoginWindow("oneDrive");
            //cloudLogWin.Show();
            this.Close();
        }
        private void googleDriveClick(object sender, RoutedEventArgs e)
        {
            api.initGoogleDriveAPI();
            CloudLogin.googleDriveLogin();
            //cloudLoginWindow cloudLogWin = new cloudLoginWindow("googleDrive");
            //cloudLogWin.Show();
            foreach (var wnd in Application.Current.Windows)
            {
                if (wnd is MainWindow || wnd is cloudPicker)
                {
                    Console.WriteLine("Main or Cloud window open");
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
            this.Close();
        }
    }
    
}
