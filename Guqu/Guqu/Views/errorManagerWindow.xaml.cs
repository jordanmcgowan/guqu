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
    /// Interaction logic for userInput.xaml
    /// </summary>
    public partial class errorManagerWindow : Window
    {
        public errorManagerWindow(String[] prompts)
        {
            InitializeComponent();
            //take in arraylist and display all elements in an
            //ArrayList aList = new ArrayList();
            //alist.count
            this.error.Text = prompts[0];
            this.error.TextAlignment = TextAlignment.Center;
            for (int i = 1; i < prompts.Length; i++)
            {
                TextBlock tBlock = new TextBlock();
                tBlock.Text = prompts[i];  //aList[i].getmessage();
                ListViewItem listViewItem = new ListViewItem();
                ListViewItem listViewItem1 = new ListViewItem();


                TextBox tBox = new TextBox();
                //tBox.Margin.Left = 40;
                tBox.Width = 100;
                tBox.Height = 20;



                listViewItem.Content = tBlock;

                listViewItem1.Content = tBox;

                this.responses.Items.Add(listViewItem1);
                this.prompts.Items.Add(listViewItem);

            }
        }

        private void acceptClick(object sender, RoutedEventArgs e)
        {
            //finish this stuff change login window back
            //String[] answers = new String[this.answers.chi];

        }
    }
}
