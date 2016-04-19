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
using GuquMysql;
using Guqu.WebServices;
using System.Security.Cryptography;

namespace Guqu
{
    
    
    /// <summary>
    /// Interaction logic for logInWindow.xaml
    /// </summary>
    public partial class logInWindow : Window
    {   

        public logInWindow()
        {
            InitializeComponent();
            setup();
        }

        private void setup()
        {
            textBox.TabIndex = 1;
            passwordBox.TabIndex = 2;
        }
        
        private void loginClick(object sender, RoutedEventArgs e)
        {
            string email = textBox.Text.ToString();
            string pass = passwordBox.Password.ToString();

            if (email == "")
            {
                errorMessage.Text = "Please enter Email.";
            }
            else if (pass == "")
            {
                errorMessage.Text = "Please enter Password.";
            }
            else
            {
                //Query DB users table for username entered
                ServerCommunicationController db = new ServerCommunicationController();

                if (db.emailExists(email))
                {
                    User user = db.SelectUser(email);
                    db.UpdateLastLogin(email);
                    //List<String> list = db.SelectUser(email);
                    byte[] salt, key;
                    // load salt and key from database
                    salt = Convert.FromBase64String(user.Pass_salt);
                    key = Convert.FromBase64String(user.Pass_hash);

                    using (var deriveBytes = new Rfc2898DeriveBytes(pass, salt))
                    {
                        byte[] newKey = (deriveBytes.GetBytes(20));  // derive a 20-byte key
                        //Checks to see if keys are NOT the same
                        if (!newKey.SequenceEqual(key))
                            errorMessage.Text = "Incorrect Password.";
                        //If keys are the same
                        else
                        {
                            List<UserCloud> userClouds;
                            //Ensure user has user clouds
                            if (db.doesUserCloudExist(user.User_id))
                            {
                                //Gather and display user clouds
                                userClouds = db.SelectUserClouds(user.User_id);
                                foreach (UserCloud cloud in userClouds)
                                {
                                    string type = "";
                                    if (cloud.Cloud_id == 1)
                                    {
                                        type = "One Drive";
                                    }
                                    else if (cloud.Cloud_id == 2)
                                    {
                                        type = "Google Drive";
                                    }
                                    Console.WriteLine("Cloud token (" + type + ") printed from logInWindow: " + cloud.Cloud_token);
                                    Console.WriteLine("Refresh token (" + type + ") printed from logInWindow: " + cloud.Refresh_token);

                                }
                            }
                            MainWindow mainWin = new MainWindow(user);
                            mainWin.Show();
                            this.Close();
                        }
                    }
                }
                else
                {
                    errorMessage.Text = "Username does not exist.";
                }
            }
        }

        private void createAccountClick(object sender, RoutedEventArgs e)
        {
            createAccountWindow createWin = new createAccountWindow();
            createWin.Show();
            this.Close();
        }
    }
}
