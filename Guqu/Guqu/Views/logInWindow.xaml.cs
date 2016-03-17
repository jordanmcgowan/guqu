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

            string email = textBox.Text.ToString();
            string pass = passwordBox.Password.ToString();

            if (usernameExists(email, pass))
            {    
                    MainWindow mainWin = new MainWindow();
                    mainWin.Show();
                    this.Close();

            }
            else//user name doesn't exist
            {
                errorMessage.Text = "Username does not exist.";
            }


        }
        //check if username given exists
        private bool usernameExists(string username, string pass)
        {
            ServerCommunicationController db = new ServerCommunicationController(); //TODO: make this object as global
            List<String> list = db.Select("users", username);
            if (list.Count > 0){
                if (list[1] == username)
                {
                    passwordCorrect(list[4], pass);
                    return true;
                }
                else{
                    return false;
                }
            }
            else{
                return false;
            }
        }
        //check to see if password for given username is correct
        private bool passwordCorrect(string dbPassword, string enteredPassword)
        {
            if (enteredPassword == dbPassword)
            {
                return true;
            }
            else
            {
                errorMessage.Text = "Incorrect password.";
                return false;
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
