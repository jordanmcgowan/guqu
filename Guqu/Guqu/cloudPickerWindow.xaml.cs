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
    /// Interaction logic for cloudPicker.xaml
    /// </summary>
    public partial class cloudPicker : Window
    {
        public cloudPicker()
        {
            InitializeComponent();
        }

        private void boxClick(object sender, RoutedEventArgs e)
        {
            cloudLoginWindow cloudLogWin = new cloudLoginWindow("box");
            cloudLogWin.Show();
            this.Close();
        }
        private void oneDriveClick(object sender, RoutedEventArgs e)
        {
            cloudLoginWindow cloudLogWin = new cloudLoginWindow("oneDrive");
            cloudLogWin.Show();
            this.Close();
        }
        private void googleDriveClick(object sender, RoutedEventArgs e)
        {
            cloudLoginWindow cloudLogWin = new cloudLoginWindow("googleDrive");
            cloudLogWin.Show();
            this.Close();
        }
    }
    
}
