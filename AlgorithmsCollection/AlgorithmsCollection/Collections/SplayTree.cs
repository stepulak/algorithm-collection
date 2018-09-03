using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class SplayTree<Key, Value> : ICollection<KeyValuePair<Key, Value>>
    {
        private class Node
        {
            public Node Parent { get; set; }
            public Node Left { get; set; } = null;
            public Node Right { get; set; } = null;
            public Key Key { get; set; }
            public Value Value { get; set; }
            public Node GrandParent => Parent?.Parent;
            public bool IsLeftChild => Parent?.Left == this;

            public Node(Key key, Value value, Node parent)
            {
                Key = key;
                Value = value;
                Parent = parent;
            }
        }
        
        public struct ReadOnlyNode
        {
            private Node node;

            public ReadOnlyNode? Parent => Create(node.Parent);
            public ReadOnlyNode? Left => Create(node.Left);
            public ReadOnlyNode? Right => Create(node.Right);
            public Key Key => node.Key;
            public Value Value
            {
                get
                {
                    return node.Value;
                }
                set
                {
                    node.Value = value;
                }
            }
            public object ThisNode => node;

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
        private static IComparer<Key> DefaultKeyComparer = Comparer<Key>.Default;

        public IComparer<Key> KeyComparer { get; private set; } = DefaultKeyComparer;
        public bool IsReadOnly => false;
        public ReadOnlyNode? Root => ReadOnlyNode.Create(root);
        public int Count { get; private set; } = 0;

        public SplayTree()
        {
        }

        public ReadOnlyNode? Find(Key key)
        {
            Node parentNode = null;
            var node = FindNodeImpl(key, root, ref parentNode);
            if (node != null)
            {
                Splay(node);
            }
            return ReadOnlyNode.Create(node);
        }
        
        public ReadOnlyNode Add(Key key, Value value)
        {
            if (root == null)
            {
                root = new Node(key, value, null);
                Count = 1;
                return ReadOnlyNode.Create(root).Value;
            }
            Node parentNode = null;
            var node = FindNodeImpl(key, root, ref parentNode);
            if (node != null) // Key-Value already present, just overwrite current value
            {
                node.Value = value;
                return ReadOnlyNode.Create(node).Value;
            }
            node = new Node(key, value, parentNode);
            var result = KeyComparer.Compare(key, parentNode.Key);
            if (KeyComparer.Compare(key, parentNode.Key) < 0)
            {
                parentNode.Left = node;
            }
            else
            {
                parentNode.Right = node;
            }
            Splay(node);
            Count++;
            return ReadOnlyNode.Create(node).Value;
        }

        public void Add(KeyValuePair<Key, Value> item) => Add(item.Key, item.Value);

        public bool Remove(Key key) => RemoveImpl(key, null);
        public bool Remove(KeyValuePair<Key, Value> item) => RemoveImpl(item.Key, item.Value);

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Key key) => Find(key).HasValue;

        public bool Contains(KeyValuePair<Key, Value> item)
        {
            Node parentNode = null;
            var result = FindNodeImpl(item.Key, root, ref parentNode);
            return result != null && result.Value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private Node MostLeftNode(Node node) => node.Left != null ? MostLeftNode(node.Left) : node;
        private Node MostRightNode(Node node) => node.Right != null ? MostRightNode(node.Right) : node;

        private void LeftRotation(Node node)
        {
            if (node.Parent == null)
            {
                throw new InvalidOperationException("Unable to perform left rotation on node with no parent");
            }
            var parent = node.Parent;
            var nodeLeft = node.Left;
            node.Left = parent;
            parent.Right = nodeLeft;
            if (nodeLeft != null)
            {
                nodeLeft.Parent = parent;
            }
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
            if (nodeRight != null)
            {
                nodeRight.Parent = parent;
            }
            SwapGrandparent(node, parent);
        }

        private void SwapGrandparent(Node node, Node parent)
        {
            var grandParent = parent.Parent;
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
            else
            {
                root = node;
            }
            parent.Parent = node;
        }

        private void Splay(Node node)
        {
            while (node.Parent != null)
            {
                var parent = node.Parent;
                if (parent.Parent == null) // Zig step
                {
                    if (node.IsLeftChild)
                    {
                        RightRotation(node);
                    }
                    else
                    {
                        LeftRotation(node);
                    }
                }
                else if (node.IsLeftChild && parent.IsLeftChild) // Zig-zig step
                {
                    RightRotation(parent);
                    RightRotation(node);
                }
                else if (!node.IsLeftChild && !parent.IsLeftChild) // Zig-zig step
                {
                    LeftRotation(parent);
                    LeftRotation(node);
                }
                else if (node.IsLeftChild && !parent.IsLeftChild) // Zig-zag step
                {
                    RightRotation(node);
                    LeftRotation(node);
                }
                else // Zig-zag step
                {
                    LeftRotation(node);
                    RightRotation(node);
                }
            }
        }

        private Node FindNodeImpl(Key key, Node node, ref Node resultNodeParent)
        {
            if (node == null)
            {
                return null;
            }
            var cmpResult = KeyComparer.Compare(key, node.Key);
            if (cmpResult == 0)
            {
                return node;
            }
            resultNodeParent = node; // Set parent, we continue searching
            return FindNodeImpl(key, cmpResult < 0 ? node.Left : node.Right, ref resultNodeParent);
        }

        private bool RemoveImpl(Key key, Optional<Value> value)
        {
            Node resultParent = null;
            var node = FindNodeImpl(key, root, ref resultParent);
            if (node == null || (value != null && value.HasValue && !node.Value.Equals(value.Value)))
            {
                return false;
            }
            Splay(node);
            var left = node.Left;
            var right = node.Right;
            if (left != null)
            {
                left.Parent = null;
                if (left.Right != null)
                {
                    var max = MostRightNode(left.Right);
                    Splay(max);
                    root = max;
                }
                else
                {
                    root = left;
                }
                if (right != null)
                {
                    root.Right = right;
                    right.Parent = root;
                }
            }
            else if (right != null)
            {
                root = right;
                root.Parent = null;
            }
            else
            {
                root = null;
            }
            Count--;
            return true;
        }
    }
}
