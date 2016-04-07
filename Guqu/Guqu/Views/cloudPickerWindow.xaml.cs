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
    /// 
    

    public partial class cloudPicker : Window
    {
        InitializeAPI api;
        public User user { get; set; }
        int cloudId;

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
            cloudId = 1;
            api.initOneDriveAPI();
            CloudLogin.oneDriveLogin(user);
            //cloudLoginWindow cloudLogWin = new cloudLoginWindow("oneDrive");
            //cloudLogWin.Show();
            bool main = false;//check to see if there is a main open
            foreach (var wnd in Application.Current.Windows)
            {
                //TODO: removed || cloudPickerWindow for demo purposes for Iteration 1.
                //TODO: this logic needs to be redone.
                if (wnd is MainWindow)
                {
                    Console.WriteLine("Main or Cloud window open");
                    main = true;
                }
                //does a mainWindow exist?
                if (main == false)
                {
                    //only if this was on new guqu account
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                }
            }
            this.Close();
        }
        private void googleDriveClick(object sender, RoutedEventArgs e)
        {
            cloudId = 2;
            List<string> token = api.initGoogleDriveAPI(); //TODO: try catch
            var accessToken = token[0];
            var refreshToken = token[1];
            Console.WriteLine("googledrive token: " + token);
            
            if (registerUserCloud(accessToken, cloudId, refreshToken))
            {
                Console.WriteLine("Registration succeeded for Google Drive.");
            }
            else
            {
                Console.WriteLine("Registration failed for Google Drive.");
            }
            
            CloudLogin.googleDriveLogin();
            //cloudLoginWindow cloudLogWin = new cloudLoginWindow("googleDrive");
            //cloudLogWin.Show();
            /*foreach (var wnd in Application.Current.Windows)
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
            }*/

            bool main = false;//check to see if there is a main open
            foreach (var wnd in Application.Current.Windows)
            {
                //TODO: removed || cloudPickerWindow for demo purposes for Iteration 1.
                //TODO: this logic needs to be redone.
                if (wnd is MainWindow)
                {
                    Console.WriteLine("Main or Cloud window open");
                    main = true;
                }
                //does a mainWindow exist?
                if (main == false)
                {
                    //only if this was on new guqu account
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                }
            }
            this.Close();
        }

        private Boolean registerUserCloud(string token, int cloudID, string refreshToken)
        {
            ServerCommunicationController db = new ServerCommunicationController();
            db.InsertNewUserCloud(user.User_id, token, cloudId, refreshToken);
            return true; //TODO: refine it
        }
    }
    
}
