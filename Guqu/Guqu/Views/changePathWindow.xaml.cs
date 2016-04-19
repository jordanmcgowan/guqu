using System;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Guqu.Models;
//using Microsoft.Win32.OpenFileDialog;

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


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //some metadata function call maybe not this
            MetaDataController mdc = new MetaDataController(currFolder.Text);
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            currFolder.Text = dialog.SelectedPath;
        }

    }
}
