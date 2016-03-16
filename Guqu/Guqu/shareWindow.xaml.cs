using System;
using System.Collections;
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
    /// Interaction logic for shareWindow.xaml
    /// </summary>
    public partial class shareWindow : Window
    {
        public shareWindow()
        {
            InitializeComponent();
        }

        private void readWrite_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void read_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void view_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            if (emailListFormattedCorrectly(emailsToShareBox.Text))
            {
                ArrayList shareEmails = parseEmailList(emailsToShareBox.Text);
                foreach (String email in shareEmails)
                {
                    //call share functions 
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Do some sort of input validation to see if user actually inputted emails separated by commas
        private Boolean emailListFormattedCorrectly(String emailList)
        {
            return true;
        }
        private ArrayList parseEmailList(String unparsedList)
        {
            String[] parsedArray = unparsedList.Split(',').Select(sValue => sValue.Trim()).ToArray();
            ArrayList parsedList = new ArrayList();
            //return parsedList.AddRange(parsedArray);
            System.Collections.ArrayList list = new System.Collections.ArrayList(parsedArray);
            return list;
        }
    }
}
