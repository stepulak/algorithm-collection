using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection.Collections
{
    public class SplayTree<Key, Value>
    {
        private class Node
        {
            public Node Parent { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Key Key { get; set; }
            public Value Value { get; set; }
            public Node GrandParent => Parent?.Parent;
            public bool IsLeftChild => Parent?.Left == this;
        }

        private void LeftRotation(Node node)
        {
            if (node.Parent == null)
            {
                throw new InvalidOperationException("Unable to perform left rotation on node with no parent");
            }
            var parent = node.Parent;
            var nodeLeft = node.Right;
            node.Left = parent;
            parent.Right = nodeLeft;
            SwapGrandparent(node, parent);
        }

        private void RightRotation(Node node)
        {
            if (node.Parent == null)
            {
                throw new InvalidOperationException("Unable to perform right rotation on node with no parent");
            }
            var parent = node.Parent;
            var nodeRight = node.Right;
            node.Right = parent;
            parent.Left = nodeRight;
            SwapGrandparent(node, parent);
        }

        private void SwapGrandparent(Node node, Node parent)
        {
            var grandParent = parent.GrandParent;
            node.Parent = grandParent;
            if (grandParent != null)
            {
                if (parent.IsLeftChild)
                {
                    grandParent.Left = node;
                }
                else
                {
                    grandParent.Right = node;
                }
            }
            parent.Parent = node;
        }
    }
}
