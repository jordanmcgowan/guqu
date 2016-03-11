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
            createAccountWindow createWin = new createAccountWindow();
            createWin.Show();
         //   if (accountCreate())
         //   {
                this.Close();

        //    }
        }
        private void haveAnAccountClick(object sender, RoutedEventArgs e)
        {
            logInWindow logInWin = new logInWindow();
            logInWin.Show();
            this.Close();
        }

        
    }
}
