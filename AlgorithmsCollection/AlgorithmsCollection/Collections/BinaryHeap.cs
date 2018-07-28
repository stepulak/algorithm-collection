using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class BinaryHeap<T> : ICollection<T>
    {
        private List<T> tree = new List<T>();
        private ChangeIndexNotifier indexNotifier = null;

        public delegate void ChangeIndexNotifier(T value, int newIndex);

        public ReadOnlyCollection<T> Tree => new ReadOnlyCollection<T>(tree);
        public int Count => tree.Count;
        public T Root => tree.First();
        public bool IsReadOnly => false;

        public IComparer<T> Comparer { get; }
        
        public BinaryHeap(IComparer<T> comparer)
        {
            Comparer = comparer ?? throw new ArgumentNullException("Comparer is null");
        }

        public BinaryHeap(IComparer<T> comparer, IEnumerable<T> initialValues) : this(comparer)
        {
            PushRange(initialValues);
        }
        
        public BinaryHeap(IComparer<T> comparer, ChangeIndexNotifier notifier) : this(comparer)
        {
            indexNotifier = notifier ?? throw new ArgumentNullException("Index notifier is null");
        }

        public void Clear() => tree.Clear();
        public void ReserveCapacity(int n) => tree.Capacity = n;
        
        public void PushRange(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("Values are null");
            }
            foreach(var value in values)
            {
                Push(value);
            }
        }

        public void Add(T value) => Push(value);

        public void Push(T value)
        {
            tree.Add(value);
            indexNotifier?.Invoke(value, Count - 1);
            BubbleUp(Count - 1);
        }

        public T Pop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Binary heap is empty");
            }
            return RemoveAt(0);
        }
        
        public void ReplacedOnIndex(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException("Invalid index");
            }
            var parent = GetParentIndex(index);
            if (parent != index && Comparer.Compare(tree[index], tree[parent]) < 0)
            {
                BubbleUp(index);
            }
            else
            {
                BubbleDown(index);
            }
        }

        public bool Contains(T value) => tree.Contains(value);
        public T Find(Predicate<T> predicate) => tree.Find(predicate);
        public List<T> FindAll(Predicate<T> predicate) => tree.FindAll(predicate);
        public bool Remove(T value) => Remove(v => Comparer.Compare(v, value) == 0);
        
        public bool Remove(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            int index = tree.FindIndex(predicate);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAll(T value) => RemoveAll(v => Comparer.Compare(v, value) == 0);
        
        public void RemoveAll(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
            for (int index; (index = tree.FindIndex(predicate)) != -1;)
            {
                RemoveAt(index);
            }
        }
        
        public IEnumerator<T> GetEnumerator() => tree.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
            foreach (var value in this.QuickSortLinq(Comparer))
            {
                array[arrayIndex++] = value;
            }
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

        public override bool Equals(object obj)
        {
            if (obj is BinaryHeap<T> heap)
            {
                return this.QuickSortLinq(Comparer).SequenceEqual(heap.QuickSortLinq(heap.Comparer));
            }
            return false;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var value in this.QuickSortLinq(Comparer))
            {
                builder.Append(value.ToString());
                builder.Append(';');
            }
            return builder.ToString();
        }

        private T RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentException("Index out of range");
            }
            var removedValue = tree[index];
            tree[index] = tree[Count - 1];
            indexNotifier?.Invoke(tree[index], index);
            tree.RemoveAt(Count - 1);
            BubbleDown(index);
            return removedValue;
        }

        private void BubbleDown(int index)
        {
            while (index < Count)
            {
                int parent = index;
                int left = GetLeftChild(index);
                int right = GetRightChild(index);
                bool leftLowerThanParent = left < Count && Comparer.Compare(tree[left], tree[index]) < 0;
                bool rightLowerThanParent = right < Count && Comparer.Compare(tree[right], tree[index]) < 0;
                
                if (leftLowerThanParent && rightLowerThanParent)
                {
                    index = Comparer.Compare(tree[left], tree[right]) < 0 ? left : right;
                }
                else if (leftLowerThanParent)
                {
                    index = left;
                }
                else if (rightLowerThanParent)
                {
                    index = right;
                }
                else
                {
                    break;
                }
                SwapElements(index, parent);
            }
        }

        private void BubbleUp(int index)
        {
            int parent = GetParentIndex(index);
            while (parent < index && Comparer.Compare(tree[index], tree[parent]) < 0)
            {
                SwapElements(index, parent);
                index = parent;
                parent = GetParentIndex(parent);
            }
        }

        private void SwapElements(int index1, int index2)
        {
            var tmp = tree[index1];
            tree[index1] = tree[index2];
            tree[index2] = tmp;
            indexNotifier?.Invoke(tree[index1], index1);
            indexNotifier?.Invoke(tree[index2], index2);
        }

        private int GetParentIndex(int childIndex)
        {
            return childIndex <= 0 ? 0 : (childIndex - 1) / 2;
        }

        private int GetLeftChild(int parent)
        {
            return parent * 2 + 1;
        }

        private int GetRightChild(int parent)
        {
            return parent * 2 + 2;
        }
    }
}
