using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection.Collections
{
    public class SplayTree<Key, Value> : ICollection<KeyValuePair<Key, Value>>
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

        public ReadOnlyNode? Find(Key key)
        {
            Node parentNode = null;
            return ReadOnlyNode.Create(FindImplementation(key, root, ref parentNode));
        }
        
        public ReadOnlyNode Insert(Key key, Value value)
        {

        }

        public void Add(KeyValuePair<Key, Value> item) => Insert(item.Key, item.Value);

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Key key) => Find(key).HasValue;

        public bool Contains(KeyValuePair<Key, Value> item)
        {
            Node parentNode = null;
            var result = FindImplementation(item.Key, root, ref parentNode);
            return result != null && result.Value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Key, Value> item)
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

        private void Splay(Node node)
        {

        }

        private Node FindImplementation(Key key, Node node, ref Node resultNodeParent)
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
            return FindImplementation(key, cmpResult < 0 ? node.Left : node.Right, ref resultNodeParent);
        }
    }
}
