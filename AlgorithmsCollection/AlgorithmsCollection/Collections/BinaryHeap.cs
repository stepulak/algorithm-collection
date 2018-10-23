using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Standard generic binary heap with automatic notification of change of value's position in heap.
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    public class BinaryHeap<T> : ICollection<T>
    {
        private List<T> tree = new List<T>();
        private readonly ChangeIndexNotifier indexNotifier = null;

        /// <summary>
        /// Delegate for user notification when value change it's position in heap.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="newIndex"></param>
        public delegate void ChangeIndexNotifier(T value, int newIndex);

        /// <summary>
        /// Tree structure property.
        /// </summary>
        public ReadOnlyCollection<T> Tree => new ReadOnlyCollection<T>(tree);

        public int Count => tree.Count;
        public T Root => tree.First();
        public bool IsReadOnly => false;

        /// <summary>
        /// Value comparer.
        /// </summary>
        public IComparer<T> Comparer { get; }
        
        /// <summary>
        /// Create heap with given comparer.
        /// </summary>
        /// <param name="comparer">Value comparer</param>
        public BinaryHeap(IComparer<T> comparer)
        {
            Comparer = comparer ?? throw new ArgumentNullException("Comparer is null");
        }

        /// <summary>
        /// Create heap with given comparer and initial values.
        /// </summary>
        /// <param name="comparer">Value comparer</param>
        /// <param name="initialValues">Inserted values into heap</param>
        public BinaryHeap(IComparer<T> comparer, IEnumerable<T> initialValues) : this(comparer)
        {
            PushRange(initialValues);
        }
        
        /// <summary>
        /// Create heap with given comparer and own change index notifier.
        /// </summary>
        /// <param name="comparer">Value comparer</param>
        /// <param name="notifier">User's own change index notifier</param>
        public BinaryHeap(IComparer<T> comparer, ChangeIndexNotifier notifier) : this(comparer)
        {
            indexNotifier = notifier ?? throw new ArgumentNullException("Index notifier is null");
        }

        /// <summary>
        /// Clear heap, remove all elements.
        /// </summary>
        public void Clear() => tree.Clear();

        /// <summary>
        /// Reserve capacity in heap. If the given capacity is lower, then the tree will be shrinked.
        /// </summary>
        /// <param name="n">Number of elements.</param>
        public void ReserveCapacity(int n) => tree.Capacity = n;
        
        /// <summary>
        /// Add values into heap.
        /// </summary>
        /// <param name="values">Values to add</param>
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

        /// <summary>
        /// Add value into heap. Equal to Push method.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void Add(T value) => Push(value);

        /// <summary>
        /// Add value into heap. Equal to Add method.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void Push(T value)
        {
            tree.Add(value);
            indexNotifier?.Invoke(value, Count - 1);
            BubbleUp(Count - 1);
        }

        /// <summary>
        /// Return removed first element from heap.
        /// </summary>
        /// <returns>First element from heap</returns>
        public T Pop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Binary heap is empty");
            }
            return RemoveAt(0);
        }
        
        /// <summary>
        /// Notify the heap that value on given index has changed (externally by user).
        /// Need to be called for having a valid heap structure.
        /// </summary>
        /// <param name="index">Changed value's index</param>
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

        /// <summary>
        /// Check, if heap contains given value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>True if heap contains given value, false otherwise</returns>
        public bool Contains(T value) => tree.Contains(value);

        /// <summary>
        /// Find first value that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>First value that fulfull given predicate</returns>
        public T Find(Predicate<T> predicate) => tree.Find(predicate);

        /// <summary>
        /// Find all values that fulfull given predicate.
        /// </summary>
        /// <param name="predicate">Predicate</param>
        public List<T> FindAll(Predicate<T> predicate) => tree.FindAll(predicate);

        /// <summary>
        /// Remove first occurence of given value in heap.
        /// </summary>
        /// <param name="value">Value to remove</param>
        /// <returns>True if values was removed, false otherwise</returns>
        public bool Remove(T value) => Remove(v => Comparer.Compare(v, value) == 0);
        
        /// <summary>
        /// Remove first value that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>True if any value was removed, false otherwise</returns>
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

        /// <summary>
        /// Remove all values from heap.
        /// </summary>
        /// <param name="value">Value to remove</param>
        public void RemoveAll(T value) => RemoveAll(v => Comparer.Compare(v, value) == 0);
        
        /// <summary>
        /// Remove all values that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate</param>
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
