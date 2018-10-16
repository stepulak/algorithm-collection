using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class SplayTree<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        private class Node
        {
            public Node Parent { get; set; }
            public Node Left { get; set; } = null;
            public Node Right { get; set; } = null;
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node GrandParent => Parent?.Parent;
            public bool IsLeftChild => Parent?.Left == this;

            public Node(TKey key, TValue value, Node parent)
            {
                Key = key;
                Value = value;
                Parent = parent;
            }
        }

        /// <summary>
        /// Read only wrapper to Node class.
        /// </summary>
        public struct ReadOnlyNode
        {
            private Node node;

            public ReadOnlyNode? Parent => Create(node.Parent);
            public ReadOnlyNode? Left => Create(node.Left);
            public ReadOnlyNode? Right => Create(node.Right);
            public TKey Key => node.Key;
            public TValue Value
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

        private struct FindNodeResult
        {
            public Node Node { get; set; }
            public Node ParentNode { get; set; }
        }

        private Node root = null;
        private static IComparer<TKey> DefaultKeyComparer = Comparer<TKey>.Default;

        public IComparer<TKey> KeyComparer { get; private set; } = DefaultKeyComparer;
        public bool IsReadOnly => false;
        public ReadOnlyNode? Root => ReadOnlyNode.Create(root);
        public int Count { get; private set; } = 0;

        public SplayTree()
        {
        }

        public SplayTree(IComparer<TKey> keyComparer)
        {
            KeyComparer = keyComparer ?? throw new ArgumentNullException("KeyComparer is null");
        }

        public SplayTree(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("Enumerable is null");
            }
            foreach (var pair in enumerable)
            {
                Add(pair);
            }
        }

        public ReadOnlyNode? Find(TKey key)
        {
            var result = FindNodeImpl(key);
            if (result.Node != null)
            {
                Splay(result.Node);
            }
            return ReadOnlyNode.Create(result.Node);
        }
        
        public ReadOnlyNode Add(TKey key, TValue value)
        {
            if (root == null) // Insert root
            {
                root = new Node(key, value, null);
                Count = 1;
                return Root.Value;
            }
            var result = FindNodeImpl(key);
            var node = result.Node;
            if (node != null) // Key-Value already present, just overwrite current value
            {
                node.Value = value;
            }
            else
            {
                node = InsertNewNode(key, value, result.ParentNode);
            }
            return ReadOnlyNode.Create(node).Value;
        }
        
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        
        public bool Remove(TKey key) => RemoveImpl(key, null);
        public bool Remove(KeyValuePair<TKey, TValue> item) => RemoveImpl(item.Key, item.Value);

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public bool ContainsKey(TKey key) => Find(key).HasValue;

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var result = FindNodeImpl(item.Key);
            return result.Node != null && result.Node.Value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("ArrayIndex is less than zero");
            }
            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("Not enough space in array");
            }
            foreach (var pair in this)
            {
                array[arrayIndex++] = pair;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var elements = new List<KeyValuePair<TKey, TValue>>();
            if (root != null)
            {
                FetchElementsRecursive(root, elements);
            }
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
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

        private FindNodeResult FindNodeImpl(TKey key)
        {
            Node parentNode = null;
            var node = FindNodeRecursive(key, root, ref parentNode);
            return new FindNodeResult { Node = node, ParentNode = parentNode };
        }

        private Node FindNodeRecursive(TKey key, Node node, ref Node resultNodeParent)
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
            return FindNodeRecursive(key, cmpResult < 0 ? node.Left : node.Right, ref resultNodeParent);
        }

        private Node InsertNewNode(TKey key, TValue value, Node parentNode)
        {
            var node = new Node(key, value, parentNode);
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
            return node;
        }

        private bool RemoveImpl(TKey key, Optional<TValue> value)
        {
            var result = FindNodeImpl(key);
            if (result.Node == null || (value != null && value.HasValue && !result.Node.Value.Equals(value.Value)))
            {
                return false;
            }
            Splay(result.Node);
            CreateTreeFromSubtrees(result.Node.Left, result.Node.Right);
            Count--;
            return true;
        }

        private void CreateTreeFromSubtrees(Node left, Node right)
        {
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
                root = right; // No left subtree, root is first node from right tree
                root.Parent = null;
            }
            else
            {
                root = null;
            }
        }

        private void FetchElementsRecursive(Node node, List<KeyValuePair<TKey, TValue>> elements)
        {
            if (node.Left != null)
            {
                FetchElementsRecursive(node.Left, elements);
            }
            elements.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
            if (node.Right != null)
            {
                FetchElementsRecursive(node.Right, elements);
            }
        }
    }
}
