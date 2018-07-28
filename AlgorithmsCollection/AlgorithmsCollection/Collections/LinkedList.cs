using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
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
            public object ThisNode => node; // so that you don't have to make Node class public

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

        public T FrontValue => frontNode.Value;
        public T BackValue => backNode.Value;
        public ReadOnlyNode? FrontNode => ReadOnlyNode.Create(frontNode);
        public ReadOnlyNode? BackNode => ReadOnlyNode.Create(backNode);
        
        public LinkedList()
        {
        }

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

        public void Clear()
        {
            Count = 0;
            frontNode = backNode = null;
        }

        public void Add(T value) => PushBack(value);

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

        public void InsertBefore(int index, T value) => InsertBefore(NodeAt(index), value);
        
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

        public void InsertAfter(int index, T value) => InsertAfter(NodeAt(index), value);

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

        public void RemoveAt(int index) => RemoveNode(NodeAt(index));

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
        
        public void RemoveAll(Predicate<T> predicate)
        {
            var list = FindAll(predicate);
            foreach(var node in list)
            {
                RemoveNode(node);
            }
        }

        public ReadOnlyNode? Find(Predicate<T> predicate) => ReadOnlyNode.Create(FindFrom(predicate, frontNode));

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

        public bool Contains(T value)
        {
            return Find(v => v.Equals(value)) != null;
        }

        public void Swap(int indexA, int indexB) => Swap(NodeAt(indexA), NodeAt(indexB));

        public void Swap(ReadOnlyNode nodeA, ReadOnlyNode nodeB)
        {
            var swapNodeA = (Node)nodeA.ThisNode;
            var swapNodeB = (Node)nodeB.ThisNode;
            var tmpValue = swapNodeA.Value;
            swapNodeA.Value = swapNodeB.Value;
            swapNodeB.Value = tmpValue;
        }

        public void Sort(IComparer<T> comparer)
        {
            var sorted = this.QuickSortLinq(comparer);
            Clear();
            foreach (var value in sorted)
            {
                PushBack(value);
            }
        }

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

        public void AppendList(LinkedList<T> list) => AppendValues(list);

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
