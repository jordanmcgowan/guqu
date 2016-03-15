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
               // this.errorMessage.Content = "Error incorrect emails or passwords!!!!!";
            }          
        }


        private bool emailExists(String email)
        {
            //add call to DB to check?
            return false;
        }

        private bool validInput(String email, String emailConfirm, String password, String passwordConfirm)
        {
            if (!email.Contains(".") || !email.Contains("@") || !email.Equals(emailConfirm)) //||eamilExists(email)
            {
                this.errorMessage.Content = "Error incorrect email.";
                return false;
            }
            else
            {
                if (password.Length < 8 || !password.Equals(passwordConfirm))
                {
                    
                    this.errorMessage.Content = "Error incorrect password.";
                    return false;
                }
         
                else
                {
                    return true;
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
