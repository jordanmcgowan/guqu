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
    /// Interaction logic for moveView.xaml
    /// </summary>
    public partial class moveView : Window
    {

        //just pass the roots
        //
        //this has alot of the same code as mainWindow
        //change later to make the same methods public so we dont have repition

        //used for path
        private Guqu.Models.SupportClasses.TreeNode selected;

        //treeFolders used for easy comparisons of ids
        List<Models.SupportClasses.TreeNode> treeFolders = new List<Models.SupportClasses.TreeNode>();
        private bool ok = false;
        public bool getOK()
        {
            return ok;
        }
        public Guqu.Models.SupportClasses.TreeNode getSelected()
        {
            return selected;
        }

        public moveView(List<Guqu.Models.SupportClasses.TreeNode> rootNodes)
        {
            InitializeComponent();
            MenuItem root;
            for (int i = 0; i < rootNodes.Count; i++) {
                root = new MenuItem();
                hierarchyAdd(rootNodes.ElementAt(i));
            }


        }
        private void populateTree(Guqu.Models.SupportClasses.TreeNode treeRoot, MenuItem xamlRoot)
        {
            xamlRoot = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName, ID = treeRoot.getCommonDescriptor().FileID };
            treeFolders.Add(treeRoot);
            recursiveBuildTree(treeRoot, xamlRoot);

        }
        private MenuItem populateMenuItem(MenuItem root, Models.SupportClasses.TreeNode node, List<Models.SupportClasses.TreeNode> folders)
        {
            MenuItem newFolder;
            foreach (var ele in node.getChildren())
            {
                if (ele.getCommonDescriptor().FileType.Equals("folder"))
                {
                    newFolder = new MenuItem() { Title = ele.getCommonDescriptor().FileName, ID = ele.getCommonDescriptor().FileID };
                    folders.Add(ele);
                    treeFolders.Add(ele);
                    newFolder.Click = new RoutedEventHandler(move_Item_Click);
                    root.Items.Add(populateMenuItem(newFolder, ele, folders));
                }
                else
                {
                    //root.Items.Add(new MenuItem() { Title = ele.getCommonDescriptor().FileName });
                }
            }
            return root;
        }
        private void hierarchyAdd(Models.SupportClasses.TreeNode newRoot)
        {
            MenuItem root = new MenuItem() { Title = newRoot.getCommonDescriptor().FileName, ID = newRoot.getCommonDescriptor().FileID }; //label as the account name
            List<Models.SupportClasses.TreeNode> newList = new List<Models.SupportClasses.TreeNode>();
            newList.Add(newRoot);
            //roots.Add(newList);
            root = populateMenuItem(root, newRoot, newList);
            selectionTree.Items.Add(root);
        }
        private void recursiveBuildTree(Guqu.Models.SupportClasses.TreeNode treeRoot, MenuItem xamlRoot)
        {
            foreach (Guqu.Models.SupportClasses.TreeNode child in treeRoot.getChildren())
            {
                MenuItem currNode = new MenuItem() { Title = treeRoot.getCommonDescriptor().FileName, ID = treeRoot.getCommonDescriptor().FileID };
                recursiveBuildTree(child, currNode);
                currNode.Items.Add(new MenuItem() { Title = child.getCommonDescriptor().FileName, ID = treeRoot.getCommonDescriptor().FileID });
                xamlRoot.Items.Add(currNode);
            }
        }


        public void move_Item_Click(object sender, RoutedEventArgs e)
        {

            TextBlock name = e.OriginalSource as TextBlock;
            String fileClicked = name.Uid;
            for(int i = 0; i < treeFolders.Count; i++)
            {
                if (treeFolders.ElementAt(i).getCommonDescriptor().FileID.Equals(name.Uid))
                {
                    selected = treeFolders.ElementAt(i);
                }
            }


        }
        public void accept_Click(object sender, RoutedEventArgs e)
        {
            if (selected != null)
            {
                ok = true;
            }
            this.Close();
        }
            //add other needed methods all these methods shouldve been made in another file and made public methods.
        }
}
