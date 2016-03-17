using System;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Guqu
{
    /// <summary>
    /// Interaction logic for changePathWindow.xaml
    /// </summary>
    public partial class changePathWindow : Window
    {
        public changePathWindow()
        {
            InitializeComponent();
        }

      private void chooseFolder_Click(object sender, RoutedEventArgs e)
        {
            /*var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose Folder";
            dlg.IsFolderPicker = true;
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                String folder = dlg.FileName;
                //textBox.Text = folder;
            }*/
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
