using Guqu.WebServices;
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
        List<Guqu.Models.SupportClasses.TreeNode> list;
        public manageCloudAccountsWindow(List<Guqu.Models.SupportClasses.TreeNode> list, User user) //User user when that actually works
        {
            this.list = list;
            InitializeComponent();
            this.user = user;
            //basic skelly of accounts
            //loop to add accounts to listView
            for (int i = 0; i < list.Count; i++)
            {        
                BitmapImage image = new BitmapImage();
                Image img = new Image();
                StackPanel sPanel = new StackPanel();
                CheckBox cB = new CheckBox();
                
                TextBlock tBlock = new TextBlock();
                //MenuItem item = new MenuItem();
                image.BeginInit();
                  
                //Account accounts[] = new Account();
                //initialize and add accounts to the list of accounts
                if (list.ElementAt(i).getCommonDescriptor().AccountType.Equals("box"))
                {
                    image.UriSource = new Uri("../Res/box.png", UriKind.Relative);
                }
                else if (list.ElementAt(i).getCommonDescriptor().AccountType.Equals("One Drive"))
                {
                    image.UriSource = new Uri("../Res/oneDrive.png", UriKind.Relative);
                }
                else if (list.ElementAt(i).getCommonDescriptor().AccountType.Equals("Google Drive"))
                {
                    image.UriSource = new Uri("../Res/googleDrive.png", UriKind.Relative);
                }
                
                image.EndInit();
                img.Width = 50;
                img.Source = image;


               
                
                tBlock.Text = list.ElementAt(i).getCommonDescriptor().FileName;
                //tBlock.Text = act[i].getUsername(); // 
                tBlock.VerticalAlignment = VerticalAlignment.Center;
                tBlock.Margin = new Thickness(50, 0, 10, 0);
                tBlock.TextAlignment = TextAlignment.Center;

                //cBox.Content = "Save Password?";
                //cBox.VerticalAlignment = VerticalAlignment.Bottom;
                cB.Margin = new Thickness(10, 0, 50, 0);
                cB.VerticalAlignment = VerticalAlignment.Center;
                sPanel.Orientation = Orientation.Horizontal;
                sPanel.Children.Add(cB);
                sPanel.Children.Add(img);
                sPanel.Children.Add(tBlock);
                //sPanel.Click = new RoutedEventHandler(account_Item_Click);
                //sPanel.Children.Add(cBox);
                this.listView.Items.Add(sPanel);
            }//end for
        }

        public void account_Item_Click(object sender, RoutedEventArgs e)
        {

        }
        private void delete_click(object sender, RoutedEventArgs e)
        {
            List<string> deleteList = new List<string>(); 
            confirmationPrompt cPrompt = new confirmationPrompt();
            cPrompt.ShowDialog();
            if (cPrompt.getRet())
            {
                List<StackPanel> listRemove = new List<StackPanel>();
                foreach (var ele in listView.Items)
                {
                    StackPanel sp = (StackPanel)ele;
                    foreach(var child in sp.Children)
                    {
                        if (child is CheckBox)
                        {
                            CheckBox cB = (CheckBox)child;
                            if (cB.IsChecked == true)
                            { 
                                foreach (var ch in sp.Children)
                                {
                                    if (ch is TextBlock)
                                    {
                                        TextBlock tb = (TextBlock)ch;
                                        deleteList.Add(tb.Text);
                                        Console.WriteLine("do stuff");
                                        listRemove.Add(sp);
                                    }
                                }
                             }
                        }
                    }
                }
                for (int x = 0; x < listRemove.Count; x++)
                {

                    listView.Items.Remove(listRemove.ElementAt(x));
                    //add logic to actually delete the items
                }

            }
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            cloudPicker cloudPick = new cloudPicker(user);
            cloudPick.Show();
            this.Close();
        }
    }
}
