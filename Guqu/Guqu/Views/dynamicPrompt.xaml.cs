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
    /// Interaction logic for dynamicPrompt.xaml
    /// </summary>
    public partial class dynamicPrompt : Window
    {
        private String[] ret;         //responses

        public String[] getRet()
        {
            return this.ret;
        }



        public dynamicPrompt(String[] prompts) //change to take in an array
        {
            InitializeComponent();
            //take in a String array and display all elements in an

            /*
                        //arr[0] will be overall prompt
                        //arr[1-n] what ever messages you want for each input 
                        String[] arr = new String[6];
                        arr[0] = "fill in stuff";
                        for (int i = 1; i < arr.Length; i++)
                        {
                            arr[i] = "" + i;
                        }

                        dynamicPrompt dp = new dynamicPrompt(arr);
                        dp.ShowDialog();
                        arr = dp.getRet();
            */


            for (int i = 1; i < prompts.Length; i++)
            {

                TextBlock tBlock = new TextBlock();
                tBlock.Text = prompts[i];  //prompts[i].getmessage();
                ListViewItem listViewItem = new ListViewItem();
                StackPanel sPanel = new StackPanel();

                TextBox tBox = new TextBox();

                //tBox.Margin.Left = 40;
                tBox.Width = 200;
                tBox.Height = 20;
                sPanel.Children.Add(tBlock);
                sPanel.Children.Add(tBox);
                listViewItem.Content = sPanel;
                this.question.Text = prompts[0];
                this.list.Items.Add(listViewItem);


            }
        }


        public void acceptClick(object sender, RoutedEventArgs e)
        {
            String[] answers = new String[this.list.Items.Count];
            //need to return array of answers somehow 
            StackPanel sP;
            ListViewItem lVI;
            TextBox tB = new TextBox();
            int i = 0;
            for (int x = 0; x < answers.Length; x++)        //traverse the list of items
            {
                lVI = (ListViewItem)this.list.Items.GetItemAt(x);
                sP = (StackPanel)lVI.Content;
                //sP.Children;

                foreach (FrameworkElement element in sP.Children)        //traverse the stackpanel
                {
                    if (element.GetType().Equals(tB.GetType()))
                    {
                        tB = (TextBox)element;

                        answers[i] = tB.Text;
                        i++;
                    }
                }
                ret = answers; //ret to be accessed by caller
                this.Close();
            }
        }

    }
}
