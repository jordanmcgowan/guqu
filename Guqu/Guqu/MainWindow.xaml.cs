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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Guqu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void logoutClicked(object sender, RoutedEventArgs e)
        {
            logInWindow logInWin = new logInWindow();
            logInWin.Show();
            this.Close();
        }
        private void exitClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void checkForUpdatesClicked(object sender, RoutedEventArgs e)
        {
        }
        private void manageAccountsClicked(object sender, RoutedEventArgs e)
        {
            manageCloudAccountsWindow manageAccountsWin = new manageCloudAccountsWindow();
            manageAccountsWin.Show();
        }
        private void settingsClicked(object sender, RoutedEventArgs e)
        {
            settingsWindow settingsWin = new settingsWindow();
            settingsWin.Show();
        }
        private void wikiClicked(object sender, RoutedEventArgs e)
        {
        }
    }
}
