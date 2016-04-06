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
    /// Interaction logic for createAccountWindow.xaml
    /// </summary>
    public partial class createAccountWindow : Window
    {
        ServerCommunicationController db;

        public createAccountWindow()
        {
            InitializeComponent();

            //Create DB connection if email is valid
            db = new ServerCommunicationController();
        }

        private void createAccountClick(object sender, RoutedEventArgs e)
        {
            String email;
            String emailConfirm;
            String password;
            String passwordConfirm;

            email = this.email.GetLineText(0);
            emailConfirm = this.emailConfirm.GetLineText(0);
            password = this.password.Password.ToString();
            passwordConfirm = this.passwordConfirm.Password.ToString();
            if (validInput(email, emailConfirm, password, passwordConfirm))
            {
                //create account
                User user = db.SelectUser(email);
                cloudPicker cPick = new cloudPicker(user);
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

            if (email == "" || emailConfirm == "")
            {
                errorMessage.Text = "Please enter Email.";
                return false;
            }
            else if (password == "" || passwordConfirm == "")
            {
                errorMessage.Text = "Please enter Password.";
                return false;
            }
            else
            {
                //Checks if emails are different or invalid (don't contain @ + .)
                if (!email.Equals(emailConfirm))
                {
                    this.errorMessage.Text = "Please confirm email correctly.";
                    return false;
                }
                else if (!emailExists(email))
                {
                    this.errorMessage.Text = "Incorrect email format.";
                    return false;
                }
                else
                {

                    if (!db.emailExists(email))
                    {
                        //Checks if passwords are different or invalid (less than 8 chars)
                        if (password.Length < 8)
                        {
                            this.errorMessage.Text = "Password should be 8 characters minimum.";
                            return false;
                        }
                        else if (!password.Equals(passwordConfirm))
                        {
                            this.errorMessage.Text = "Please confirm password correctly.";
                            return false;
                        }
                        else
                        {
                            //NEW MCG WORK: Adds in salt and key (password) 
                            byte[] salt, key;
                            string encodedSalt, encodedKey;
                            // specify that we want to randomly generate a 20-byte salt
                            using (var deriveBytes = new Rfc2898DeriveBytes(password, 20))
                            {
                                salt = deriveBytes.Salt;
                                key = deriveBytes.GetBytes(20);  // derive a 20-byte key

                                encodedSalt = Convert.ToBase64String(salt);
                                encodedKey = Convert.ToBase64String(key);
                                Console.WriteLine((encodedSalt) + " --- " + (encodedKey));
                            }


                            //DB INSERT
                            db.Insert("users", email, encodedKey, encodedSalt);
                            Console.WriteLine(email + " has been added successfully.");
                            return true;
                        }
                    }
                    else
                    {
                        errorMessage.Text = "Email already exists. Please try again.";
                        return false;
                    }


                    /*
                    //Check DB to see if email is already in use
                    if (!db.emailExists(email))
                    {
                        //Checks if passwords are different or invalid (less than 8 chars)
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
                    //Email is already in DB, cant create another account with that email
                    else
                    {
                        this.errorMessage.Text = "Email already exists. Please try again!";
                        return false;
                    }
                    */
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
