using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models.SupportClasses
{
    public class TreeNode
    {
        private CommonDescriptor commonDescriptor;
        private TreeNode parent;
        private LinkedList<TreeNode> children;

        public TreeNode(TreeNode parent, CommonDescriptor cd)
        {
            this.parent = parent;
            commonDescriptor = cd;
            children = new LinkedList<TreeNode>();
            children.Clear();
        }
        public TreeNode getParent()
        {
            return parent;
        }
        public LinkedList<TreeNode> getChildren()
        {
            return children;
        }
        public void addChild(TreeNode child)
        {
            children.AddLast(child);
        }
        public CommonDescriptor getCommonDescriptor()
        {
            return commonDescriptor;
        }

    }
}
