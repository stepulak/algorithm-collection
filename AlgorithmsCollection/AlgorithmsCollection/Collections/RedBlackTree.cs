using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class RedBlackTree<T> : ICollection<T>
    {
        public enum NodeColor
        {
            Red,
            Black
        };

        private class Node
        {
            public T Value { get; set; }

            public NodeColor Color { get; set; }
            public Node Parent { get; set; } = null;
            public Node Left { get; set; } = null;
            public Node Right { get; set; } = null;
            public Node Grandparent => Parent?.Parent;
            public Node Uncle => IsParentLeftSided() ? Grandparent?.Left : Grandparent?.Right;
            public Node Sibling => IsLeftSided() ? Parent?.Right : Parent?.Left;

            public Node(T value, Node parent)
            {
                Value = value;
                Parent = parent;
                Color = parent == null ? NodeColor.Black : NodeColor.Red;
            }
            
            public bool IsParentLeftSided()
            {
                if (Parent == null || Grandparent == null)
                {
                    return false;
                }
                return Grandparent.Left == Parent;
            }

            public bool IsLeftSided() => Parent?.Left == this;
        }

        public struct ReadOnlyNode
        {
            private Node node;

            public ReadOnlyNode? Parent => Create(node.Parent);
            public ReadOnlyNode? Left => Create(node.Left);
            public ReadOnlyNode? Right => Create(node.Right);
            public NodeColor Color => node.Color;
            public T Value => node.Value;

            public static ReadOnlyNode? Create(object node)
            {
                if (node != null)
                {
                    return new ReadOnlyNode { node = (Node)node };
                }
                return null;
            }
        }

        private Node root = null;

        public ReadOnlyNode? Tree => ReadOnlyNode.Create(root);
        public int Count { get; private set; } = 0;
        public bool IsReadOnly => false;
        public IComparer<T> Comparer { get; }

        public RedBlackTree(IComparer<T> comparer)
        {
            Comparer = comparer ?? throw new ArgumentNullException("Comparer is null");
        }

        public RedBlackTree(IComparer<T> comparer, IEnumerable<T> initialValues) : this(comparer)
        {
            if (initialValues == null)
            {
                throw new ArgumentNullException("Initial values are null");
            }
            foreach (var value in initialValues)
            {
                Add(value);
            }
        }

        public void Add(T value)
        {
            var node = InsertValueIntoTree(value);
            BalanceTree(node);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private Node InsertValueIntoTree(T value)
        {
            Count++;
            if (root == null)
            {
                root = new Node(value, null);
                return root;
            }
            var nextNode = root;
            Node nodeToInsert;
            int lastCompareResult;
            do
            {
                nodeToInsert = nextNode;
                lastCompareResult = Comparer.Compare(value, nextNode.Value);
                nextNode = lastCompareResult < 0 ? nextNode.Left : nextNode.Right;
            } while (nextNode != null);
            if (lastCompareResult < 0)
            {
                nodeToInsert.Left = new Node(value, nodeToInsert);
                return nodeToInsert.Left;
            }
            nodeToInsert.Right = new Node(value, nodeToInsert);
            return nodeToInsert.Right;
        }

        private void BalanceTree(Node node)
        {
            if (node == root)
            {
                node.Color = NodeColor.Black;
                return;
            }
            if (node.Parent.Color != NodeColor.Red)
            {
                return; // balanced
            }
            var uncle = node.Uncle;
            if (uncle != null && uncle.Color == NodeColor.Red)
            {
                BalanceTreeParentAndUncleRed(node, uncle);
                return;
            }
            // Parent red, uncle black
            if (node.IsParentLeftSided())
            {
                BalanceTreeLeftSide(node);
            }
            else
            {
                BalanceTreeRightSide(node);
            }
        }

        private void BalanceTreeParentAndUncleRed(Node node, Node uncle)
        {
            uncle.Color = NodeColor.Black;
            node.Parent.Color = NodeColor.Black;
            BalanceTree(node.Parent.Parent);
        }

        private void BalanceTreeLeftSide(Node node)
        {
            if (node.IsLeftSided())
            {
                BalanceTreeLeftLeftSide(node);
            }
            else
            {
                BalanceTreeLeftRightSide(node);
            }
        }

        private void BalanceTreeRightSide(Node node)
        {
            if (node.IsLeftSided())
            {
                BalanceTreeRightLeftSide(node);
            }
            else
            {
                BalanceTreeRightLeftSide(node);
            }
        }

        private void BalanceTreeLeftLeftSide(Node node)
        {
            var grandpa = node.Grandparent;
            var greatGrandpa = grandpa.Parent;
            var parent = node.Parent;
            // Swap colors
            grandpa.Color = NodeColor.Red;
            parent.Color = NodeColor.Black;
            if (greatGrandpa != null)
            {
                if (greatGrandpa.Left == grandpa)
                {
                    greatGrandpa.Left = parent;
                }
                else
                {
                    greatGrandpa.Right = parent;
                }
            }
            parent.Parent = greatGrandpa;
            grandpa.Left = parent.Right;
            parent.Right = grandpa;
        }

        private void BalanceTreeLeftRightSide(Node node)
        {
            var grandpa = node.Grandparent;
            var parent = node.Parent;
            grandpa.Left = node;
            node.Parent = grandpa;
            parent.Parent = node;
            parent.Right = node.Left;
            node.Left = parent;
            BalanceTreeLeftLeftSide(node);
        }

        private void BalanceTreeRightLeftSide(Node node)
        { }

        private void BalanceTreeRightRightSide(Node node)
        { }
    }
}
