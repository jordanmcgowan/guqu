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
    /// Interaction logic for cloudLoginWindow.xaml
    /// </summary>
    public partial class cloudLoginWindow : Window
    {
        public cloudLoginWindow(String accountType)
        {
            InitializeComponent();

            if (accountType.Equals("box"))
            {
                //login to box and add to accounts
            }
            else if (accountType.Equals("oneDrive"))
            {
                //login to oneDrive and add to accounts
            }
            else if (accountType.Equals("googleDrive"))
            {
                //login to googleDrive and add to accounts
            }
        }
    }
}
