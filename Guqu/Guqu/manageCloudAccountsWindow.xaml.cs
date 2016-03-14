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
    /// Interaction logic for manageCloudAccountsWindow.xaml
    /// </summary>
    public partial class manageCloudAccountsWindow : Window
    {
        public manageCloudAccountsWindow()
        {
            //act1img.SetValue()
            InitializeComponent();
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            confirmationPrompt cPrompt = new confirmationPrompt();
            cPrompt.Show();
            this.Close();
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            cloudPicker cloudPick = new cloudPicker();
            cloudPick.Show();
            this.Close();
        }
    }
}
