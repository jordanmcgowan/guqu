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
using GuquMysql;

namespace Guqu
{
    /// <summary>
    /// Interaction logic for cloudPicker.xaml
    /// </summary>
    public partial class cloudPicker : Window
    {
        InitializeAPI api;
        public User user { get; set; }

        public cloudPicker(User user)
        {
            InitializeComponent();
            this.user = user;
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
            string token = api.initGoogleDriveAPI(); //TODO: try catch
            Console.WriteLine("googledrive token: " + token);
            
            if (registerUserCloud(token))
            {
                Console.WriteLine("registration succeeded.");
            }
            else
            {
                Console.WriteLine("registration failed.");
            }
            
            CloudLogin.googleDriveLogin();
            //cloudLoginWindow cloudLogWin = new cloudLoginWindow("googleDrive");
            //cloudLogWin.Show();
            foreach (var wnd in Application.Current.Windows)
            {
                //TODO: removed || cloudPickerWindow for demo purposes for Iteration 1.
                //TODO: this logic needs to be redone.
                if (wnd is MainWindow)
                {
                    Console.WriteLine("Main or Cloud window open");
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                }
            }
            this.Close();
        }

        private Boolean registerUserCloud(string token)
        {
            ServerCommunicationController db = new ServerCommunicationController();
            int cloudId = 2; //Google Drive cloudId is 2
            db.InsertNewUserCloud(user.User_id, token, cloudId);

            return true; //TODO: refine it
        }
    }
    
}
