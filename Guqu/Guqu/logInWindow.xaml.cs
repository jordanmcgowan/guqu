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
        }

        private void loginClick(object sender, RoutedEventArgs e)
        {
            if (usernameExists(textBox.Text.ToString()))
            {
                if (passwordCorrect(passwordBox.Password.ToString()))
                {
                    MainWindow mainWin = new MainWindow();
                    mainWin.Show();
                    this.Close();
                }
                else//passwordIncorect
                {
                    errorMessage.Text = "Incorrect password.";
                }
            }
            else//user name doesn't exist
            {
                errorMessage.Text = "Username does not exist.";
            }


        }
        //check if username given exists
        private bool usernameExists(String username)
        {
            return true;
        }
        //check to see if password for given username is correct
        private bool passwordCorrect(String password)
        {
            return true;
        }

        private void createAccountClick(object sender, RoutedEventArgs e)
        {
            //if (!acceptableEmailAddress())
            //{
            // error message  
            //}
            //if (!acceptablePassword())
            //{
            // error message
            //}
            createAccountWindow createWin = new createAccountWindow();
            createWin.Show();
            this.Close();
        }
    }
}
