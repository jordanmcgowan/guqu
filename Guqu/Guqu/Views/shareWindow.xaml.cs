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
using Guqu.Models;
using Guqu.WebServices;

namespace Guqu
{
    /// <summary>
    /// Interaction logic for shareWindow.xaml
    /// </summary>
    public partial class shareWindow : Window
    {
        bool read = false;
        bool comment = false;
        bool write = false;
        List<dispFolder> filesToShare;
        public shareWindow(List<dispFolder> files)
        {
            InitializeComponent();
            filesToShare = files;
        }

        private void read_Checked(object sender, RoutedEventArgs e)
        {
            read = true;
            comment = false;
            write = false;
        }

        private void comment_Checked(object sender, RoutedEventArgs e)
        {
            read = false;
            comment = true;
            write = false;
        }

        private void write_Checked(object sender, RoutedEventArgs e)
        {
            read = false;
            comment = false;
            write = true;
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            if (emailListFormattedCorrectly(emailsToShareBox.Text))
            {
                ICloudCalls cloudCaller = null;
                ArrayList shareEmails = parseEmailList(emailsToShareBox.Text);
                if (filesToShare.First().CD.AccountType == "Google Drive")
                {
                    cloudCaller = new GoogleDriveCalls();
                }
                else if (filesToShare.First().CD.AccountType == "One Drive")
                {
                    //not implemented yet
                    cloudCaller = new OneDriveCalls();
                }
                else
                {
                    throw new InvalidOperationException();
                }
                foreach (String email in shareEmails)
                {

                    foreach (dispFolder file in filesToShare)
                    {
                           if(read){
                            cloudCaller.shareFile(file.CD,"reader", email, optionalMessageBox.Text);
                           }
                           else if(write){
                               cloudCaller.shareFile(file.CD, "writer", email, optionalMessageBox.Text);
                           }
                           else if(comment){
                               cloudCaller.shareFile(file.CD, "commenter", email, optionalMessageBox.Text);
                           }
                           else
                           {
                               throw new InvalidOperationException();
                           }
                    }
                }
            }
            this.Close();
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
