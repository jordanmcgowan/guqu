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
            
        }
        private void createAccountClick(object sender, RoutedEventArgs e)
        {
            //if (!acceptableEmailAddress())
            //{
            // error message  
            //}
            //if (!acceptablePassord())
            //{
            // error message
            //}
            createAccountWindow createWin = new createAccountWindow();
            createWin.Show();
            this.Close();
        }
        
    }
}
