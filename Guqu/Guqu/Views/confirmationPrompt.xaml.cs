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
    /// Interaction logic for confirmationPrompt.xaml
    /// </summary>
    public partial class confirmationPrompt : Window
    {
        private bool ret = false;
        public confirmationPrompt()
        {
            InitializeComponent();
        }

        public bool getRet()
        {
            return ret;
        }
        private void yesClick(object sender, RoutedEventArgs e)
        {
            ret = true;
            this.Close();
        }

        private void noClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
