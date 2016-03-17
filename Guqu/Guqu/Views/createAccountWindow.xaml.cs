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
    /// Interaction logic for createAccountWindow.xaml
    /// </summary>
    public partial class createAccountWindow : Window
    {
        public createAccountWindow()
        {
            InitializeComponent();
        }

        private void createAccountClick(object sender, RoutedEventArgs e)
        {
            String email;
            String emailConfirm;
            String password;
            String passwordConfirm;


            email = this.email.GetLineText(0);
            emailConfirm = this.emailConfirm.GetLineText(0);
            password = this.password.GetLineText(0);
            passwordConfirm = this.passwordConfirm.GetLineText(0);
            if (validInput(email, emailConfirm, password, passwordConfirm))
            {
                //create account
                cloudPicker cPick = new cloudPicker();
                cPick.Show();
                this.Close();
            }
            else
            {
               // this.errorMessage.Content = "Error incorrect email or password";
            }          
        }


        private bool emailExists(String email)
        {
            
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool validInput(String email, String emailConfirm, String password, String passwordConfirm)
        {

            if (!email.Equals(emailConfirm) || !emailExists(email))
            {
                this.errorMessage.Text = "Error incorrect email.";
                return false;
            }
            else
            {
                ServerCommunicationController db = new ServerCommunicationController(); //TODO: make this object as global
                if (!db.emailExists(email))
                {

                    if (password.Length < 8 || !password.Equals(passwordConfirm))
                    {

                        this.errorMessage.Text = "Error incorrect password.";
                        return false;
                    }

                    else
                    {
                        //DB INSERT
                        db.Insert("users", email, password, "salt");
                        return true;
                    }
                }
                else
                {
                    this.errorMessage.Text = "Email already exists. Please try again!";
                    return false;
                }
            }          
        } 


        private void haveAnAccountClick(object sender, RoutedEventArgs e)
        {
            logInWindow logInWin = new logInWindow();
            logInWin.Show();
            this.Close();
        }

        private void alreadyHaveAccountClick(object sender, RoutedEventArgs e)
        {
            logInWindow logInWin = new logInWindow();
            logInWin.Show();
            this.Close();
        }
    }
}
