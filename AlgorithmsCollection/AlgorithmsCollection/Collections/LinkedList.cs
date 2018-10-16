using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Generic double-linked list. Very similar to .NET linked list.
    /// </summary>
    /// <typeparam name="T">Element's type</typeparam>
    public class LinkedList<T> : ICollection<T>, IEnumerable<T>
    {
        private class Node
        {
            public Node Previous { get; set; } = null;
            public Node Next { get; set; } = null;
            public T Value { get; set; }

            public Node(T value)
            {
                Value = value;
            }
        };

        /// <summary>
        /// Read only wrapper to LinkedList's Node class.
        /// </summary>
        public struct ReadOnlyNode
        {
            private Node node;

            public ReadOnlyNode? Previous => Create(node.Previous);
            public ReadOnlyNode? Next => Create(node.Next);
            public T Value
            {
                get { return node.Value; }
                set { node.Value = value; }
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
        };

        private Node frontNode = null;
        private Node backNode = null;
        
        public bool IsReadOnly => false;
        public int Count { get; private set; } = 0;
        public bool Empty => Count == 0;

        /// <summary>
        /// Value of front node.
        /// </summary>
        public T FrontValue => frontNode.Value;

        /// <summary>
        /// Value of back node.
        /// </summary>
        public T BackValue => backNode.Value;
        
        public ReadOnlyNode? FrontNode => ReadOnlyNode.Create(frontNode);
        public ReadOnlyNode? BackNode => ReadOnlyNode.Create(backNode);
        
        /// <summary>
        /// Create empty list.
        /// </summary>
        public LinkedList()
        {
        }

        /// <summary>
        /// Create list and fill it with values
        /// </summary>
        /// <param name="values">Values to fill</param>
        public LinkedList(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("Values are null");
            }
            foreach(var value in values)
            {
                PushBack(value);
            }
        }

        /// <summary>
        /// Clear linked list and remove all elements.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            frontNode = backNode = null;
        }

        /// <summary>
        /// Add value at the end of the linked list. Equal to PushBack method.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void Add(T value) => PushBack(value);

        /// <summary>
        /// Add value at the beginning of the linked list.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void PushFront(T value)
        {
            if (frontNode == null)
            {
                AddFirstNode(value);
                return;
            }
            var node = new Node(value)
            {
                Next = frontNode
            };
            frontNode.Previous = node;
            frontNode = node;
            Count++;
        }

        /// <summary>
        /// Add value at the end of the linked list.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void PushBack(T value)
        {
            if (backNode == null)
            {
                AddFirstNode(value);
                return;
            }
            var node = new Node(value)
            {
                Previous = backNode
            };
            backNode.Next = node;
            backNode = node;
            Count++;
        }
        
        /// <summary>
        /// Return node at given index.
        /// </summary>
        /// <param name="index">Index of node to return</param>
        /// <returns>Node at given index</returns>
        public ReadOnlyNode NodeAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException("Index is out of range");
            }
            var node = frontNode;
            for (int i = 0; i < index && node != null; i++)
            {
                node = node.Next;
            }
            if (node == null)
            {
                throw new InvalidOperationException("Linked list is corrupted");
            }
            return ReadOnlyNode.Create(node).Value;
        }
        
        /// <summary>
        /// Get/set node's value at given index
        /// </summary>
        /// <param name="index">Value's index</param>
        /// <returns>Value at given index</returns>
        public T this[int index]
        {
            get
            {
                return NodeAt(index).Value;
            }
            set
            {
                (NodeAt(index).ThisNode as Node).Value = value;
            }
        }
        
        /// <summary>
        /// Insert value before given node.
        /// </summary>
        /// <param name="node">Insert before this node</param>
        /// <param name="value">Value to insert</param>
        public void InsertBefore(ReadOnlyNode node, T value)
        {
            if (node.ThisNode == frontNode)
            {
                PushFront(value);
                return;
            }
            var nextNode = (Node)node.ThisNode;
            var newNode = new Node(value)
            {
                Previous = nextNode.Previous,
                Next = nextNode
            };
            nextNode.Previous = newNode;
            newNode.Previous.Next = newNode;
            Count++;
        }

        /// <summary>
        /// Insert value before node at given index.
        /// </summary>
        /// <param name="index">Insert before node at this index</param>
        /// <param name="value">Value to insert</param>
        public void InsertBefore(int index, T value) => InsertBefore(NodeAt(index), value);
        
        /// <summary>
        /// Insert value after given node.
        /// </summary>
        /// <param name="node">Insert after this node</param>
        /// <param name="value">Value to insert</param>
        public void InsertAfter(ReadOnlyNode node, T value)
        {
            if (node.ThisNode == backNode)
            {
                PushBack(value);
                return;
            }
            var previousNode = (Node)node.ThisNode;
            var newNode = new Node(value)
            {
                Previous = previousNode,
                Next = previousNode.Next
            };
            previousNode.Next = newNode;
            newNode.Next.Previous = newNode;
            Count++;
        }

        /// <summary>
        /// Insert value after node at given index.
        /// </summary>
        /// <param name="index">Insert after node at this index</param>
        /// <param name="value">Value to insert</param>
        public void InsertAfter(int index, T value) => InsertAfter(NodeAt(index), value);

        /// <summary>
        /// Remove first node from list and return it's value.
        /// </summary>
        /// <returns>Value from removed first node</returns>
        public T PopFront()
        {
            if (Empty)
            {
                throw new InvalidOperationException("List is empty");
            }
            var value = FrontValue;
            frontNode = frontNode.Next;
            if (frontNode != null)
            {
                frontNode.Previous = null;
            }
            else
            {
                backNode = null;
            }
            Count--;
            return value;
        }

        /// <summary>
        /// Remove last node from list and return it's value.
        /// </summary>
        /// <returns>Value from removed last node</returns>
        public T PopBack()
        {
            if (Empty)
            {
                throw new InvalidOperationException("List is empty");
            }
            var value = BackValue;
            backNode = backNode.Previous;
            if (backNode != null)
            {
                backNode.Next = null;
            }
            else
            {
                frontNode = null;
            }
            Count--;
            return value;
        }

        /// <summary>
        /// Remove node with first occurence of given value.
        /// </summary>
        /// <param name="value">Value to delete</param>
        /// <returns>True if any node was deleted</returns>
        public bool Remove(T value)
        {
            var node = Find(v => v.Equals(value));
            if (node != null)
            {
                RemoveNode(node.Value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove node at given index.
        /// </summary>
        /// <param name="index">Index of node to delete</param>
        public void RemoveAt(int index) => RemoveNode(NodeAt(index));

        /// <summary>
        /// Remove node from list.
        /// </summary>
        /// <param name="node">Node to remove</param>
        public void RemoveNode(ReadOnlyNode node)
        {
            if (node.ThisNode == frontNode)
            {
                PopFront();
                return;
            }
            if (node.ThisNode == backNode)
            {
                PopBack();
                return;
            }
            var removeNode = (Node)node.ThisNode;
            removeNode.Previous.Next = removeNode.Next;
            removeNode.Next.Previous = removeNode.Previous;
            Count--;
        }
        
        /// <summary>
        /// Remove all nodes with values that match given predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        public void RemoveAll(Predicate<T> predicate)
        {
            var list = FindAll(predicate);
            foreach(var node in list)
            {
                RemoveNode(node);
            }
        }

        /// <summary>
        /// Return first node that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Node with value that match given predicate, otherwise null</returns>
        public ReadOnlyNode? Find(Predicate<T> predicate) => ReadOnlyNode.Create(FindFrom(predicate, frontNode));

        /// <summary>
        /// Find all nodes with values that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>List of nodes that match given predicate</returns>
        public List<ReadOnlyNode> FindAll(Predicate<T> predicate)
        {
            var list = new List<ReadOnlyNode>();
            var node = FindFrom(predicate, frontNode);
            while (node != null)
            {
                list.Add(ReadOnlyNode.Create(node).Value);
                node = FindFrom(predicate, node.Next);
            }
            return list;
        }

        /// <summary>
        /// Check whether this list contains given value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>True if this list contains given value, otherwise false</returns>
        public bool Contains(T value) => Find(v => v.Equals(value)) != null;

        /// <summary>
        /// Swap values at nodes with given indices.
        /// </summary>
        /// <param name="indexA">Index of first node</param>
        /// <param name="indexB">Index of second node</param>
        public void Swap(int indexA, int indexB) => Swap(NodeAt(indexA), NodeAt(indexB));

        /// <summary>
        /// Swap values at given nodes.
        /// </summary>
        /// <param name="nodeA">First node</param>
        /// <param name="nodeB">Second node</param>
        public void Swap(ReadOnlyNode nodeA, ReadOnlyNode nodeB)
        {
            var swapNodeA = (Node)nodeA.ThisNode;
            var swapNodeB = (Node)nodeB.ThisNode;
            var tmpValue = swapNodeA.Value;
            swapNodeA.Value = swapNodeB.Value;
            swapNodeB.Value = tmpValue;
        }

        /// <summary>
        /// Sort values in list according to given comparer.
        /// </summary>
        /// <param name="comparer">Comparer</param>
        public void Sort(IComparer<T> comparer)
        {
            var sorted = this.QuickSortLinq(comparer);
            Clear();
            foreach (var value in sorted)
            {
                PushBack(value);
            }
        }

        /// <summary>
        /// Reverse list.
        /// </summary>
        public void Reverse()
        {
            var newFrontNode = backNode;
            var newBackNode = frontNode;
            for (var node = backNode; node != null; node = node.Next)
            {
                var next = node.Next;
                node.Next = node.Previous;
                node.Previous = next;
            }
            frontNode = newFrontNode;
            backNode = newBackNode;
        }

        /// <summary>
        /// Insert values at the end of list.
        /// </summary>
        /// <param name="values">Values to insert</param>
        public void AppendValues(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("Values are null");
            }
            foreach (var value in values)
            {
                PushBack(value);
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            for (var node = frontNode; node != null; node = node.Next)
            {
                yield return node.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(T[] array, int arrayIndex)
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
            foreach (var value in this)
            {
                array[arrayIndex++] = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is LinkedList<T> list)
            {
                return this.SequenceEqual(list);
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            int result = 0;
            foreach (var value in this)
            {
                result ^= value.GetHashCode();
            }
            return result;
        }
    
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var value in this)
            {
                builder.Append(value.ToString());
                builder.Append(';');
            }
            return builder.ToString();
        }

        private Node FindFrom(Predicate<T> predicate, Node startNode)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            for (var node = startNode; node != null; node = node.Next)
            {
                if (predicate(node.Value))
                {
                    return node;
                }
            }
            return null;
        }

        private void AddFirstNode(T value)
        {
            frontNode = backNode = new Node(value);
            Count = 1;
        }
    }
}
