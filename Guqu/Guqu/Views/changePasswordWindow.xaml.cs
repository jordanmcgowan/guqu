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
    /// Interaction logic for settingsWindow.xaml
    /// </summary>
    public partial class changePasswordWindow : Window
    {
        public changePasswordWindow()
        {
            InitializeComponent();
        }

        private void changePasswordButton_Click(object sender, RoutedEventArgs e)
        {
               string a = passwordBox.Password;
            if (passwordBox.Password.Equals(confirmPasswordBox.Password))
            {
                if (newPasswordBox.Password.Equals(confirmNewPasswordBox.Password))
                {
                    //call some method to change password
                }
                else
                {
                    errorMessageBox.Text = "New passwords do not match";
                }
            }
            else
            {
                errorMessageBox.Text = "Old passwords do not match";
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
