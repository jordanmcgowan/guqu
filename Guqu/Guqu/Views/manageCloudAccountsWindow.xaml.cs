﻿using Guqu.WebServices;
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
        public User user { get; set; }

        public manageCloudAccountsWindow(List<Guqu.Models.SupportClasses.TreeNode> list) //User user when that actually works
        {
            InitializeComponent();
            this.user = user;
            //basic skelly of accounts
            //loop to add accounts to listView
            for (int i = 0; i < list.Count; i++)
            {        
                BitmapImage image = new BitmapImage();
                Image img = new Image();
                StackPanel sPanel = new StackPanel();
                CheckBox cBox = new CheckBox();
                TextBlock tBlock = new TextBlock();

                image.BeginInit();
                /*  
                Account accounts[] = new Account();
                //initialize and add accounts to the list of accounts
                if (act[i].getType.equals("box"))
                {
                    image.UriSource = new Uri("box.png", UriKind.Relative);
                }
                else if (act[i].getType.equals("box"))
                {
                    image.UriSource = new Uri("oneDrive.png", UriKind.Relative);
                }
                else
                {
                    image.UriSource = new Uri("googleDrive.png", UriKind.Relative);
                }
                */
                //image.EndInit();
                img.Width = 75;
                img.Source = image;


               
                
                tBlock.Text = list.ElementAt(i).getCommonDescriptor().FileName;
                //tBlock.Text = act[i].getUsername(); // 
                tBlock.VerticalAlignment = VerticalAlignment.Bottom;
                tBlock.Margin = new Thickness(10, 0, 10, 0); 
                
                cBox.Content = "Save Password?";
                cBox.VerticalAlignment = VerticalAlignment.Bottom;


                sPanel.Orientation = Orientation.Horizontal;
                sPanel.Children.Add(img);
                sPanel.Children.Add(tBlock);
                sPanel.Children.Add(cBox);
                this.listView.Items.Add(sPanel);
            }//end for
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            confirmationPrompt cPrompt = new confirmationPrompt();
            cPrompt.Show();
            this.Close();
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            cloudPicker cloudPick = new cloudPicker(user);
            cloudPick.Show();
            this.Close();
        }
    }
}
