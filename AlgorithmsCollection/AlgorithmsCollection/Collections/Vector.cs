using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Generic vector equivalent to .NET ArrayList or STL std::vector.
    /// </summary>
    /// <typeparam name="T">Element's type</typeparam>
    public class Vector<T> : ICollection<T>
    {
        private const int DefaultCapacity = 4;
        private T[] vector = null;

        /// <summary>
        /// Number of elements in vector.
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// Check whether vector is empty.
        /// </summary>
        public bool Empty => Count == 0;

        /// <summary>
        /// Number of reserved elements.
        /// </summary>
        public int Capacity => vector.Length;

        public bool IsReadOnly => false;

        /// <summary>
        /// First value in vector.
        /// </summary>
        public T Front
        {
            get
            {
                CheckVectorEmptyThrow();
                return vector[0];
            }
        }

        /// <summary>
        /// Last value in vector.
        /// </summary>
        public T Back
        {
            get
            {
                CheckVectorEmptyThrow();
                return vector[Count - 1];
            }
        }

        /// <summary>
        /// Create empty vector with default initial capacity.
        /// </summary>
        public Vector() : this(DefaultCapacity)
        {
        }

        /// <summary>
        /// Create empty vector with own initial capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public Vector(int capacity)
        {
            Reserve(capacity);
        }

        /// <summary>
        /// Create vector and fill it with given values.
        /// </summary>
        /// <param name="values">Values to insert</param>
        public Vector(IEnumerable<T> values)
        {
            var count = values.Count();
            Reserve(count != 0 ? count : DefaultCapacity);
            AddRange(values);
        }

        /// <summary>
        /// Add value at the end of vector.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void PushBack(T value)
        {
            ReserveIfFull();
            vector[Count++] = value;
        }
        
        /// <summary>
        /// Remove value from the end of vector.
        /// </summary>
        /// <returns>Removed last value</returns>
        public T PopBack()
        {
            CheckVectorEmptyThrow();
            return vector[--Count];
        }

        /// <summary>
        /// Insert value before given index. All values after and including index are automatically moved by one to the right.
        /// </summary>
        /// <param name="value">Value to insert</param>
        /// <param name="index">Index of position</param>
        public void InsertBefore(T value, int index)
        {
            CheckIndexBoundaryThrow(index);
            ReserveIfFull();
            VectorMoveForwardOneStep(index, 0);
            vector[index] = value;
            Count++;
        }

        /// <summary>
        /// Insert value after given index. All values after index are automatically moved by one to the right.
        /// </summary>
        /// <param name="value">Value to insert</param>
        /// <param name="index">Index of position</param>
        public void InsertAfter(T value, int index)
        {
            CheckIndexBoundaryThrow(index);
            ReserveIfFull();
            if (index < Count - 1)
            {
                VectorMoveForwardOneStep(index, 1);
            }
            vector[index + 1] = value;
            Count++;
        }

        /// <summary>
        /// Get/set value at given position.
        /// </summary>
        /// <param name="index">Index of value's position</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                CheckIndexBoundaryThrow(index);
                return vector[index];
            }
            set
            {
                CheckIndexBoundaryThrow(index);
                vector[index] = value;
            }
        }

        /// <summary>
        /// Add value at the end of the vector. Equal to PushBack method.
        /// </summary>
        /// <param name="value">Value to add</param>
        public void Add(T value) => PushBack(value);

        /// <summary>
        /// Append values at the end of the vector.
        /// </summary>
        /// <param name="values">Values to add</param>
        public void AddRange(IEnumerable<T> values)
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

        /// <summary>
        /// Clear vector. Remove all elements. Shrink to default capacity.
        /// </summary>
        public void Clear()
        {
            var capacity = Capacity;
            vector = new T[DefaultCapacity];
            Count = 0;
        }

        /// <summary>
        /// Check whether this vector contains given value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>True if vector contains given value, false otherwise.</returns>
        public bool Contains(T value) => FindIndex(value) != -1;

        /// <summary>
        /// Remove first value that is equal to given value from the vector.
        /// </summary>
        /// <param name="value">Value to remove</param>
        /// <returns>True if any value was removed, false otherwise</returns>
        public bool Remove(T value)
        {
            var index = FindIndex(value);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove value at given index.
        /// </summary>
        /// <param name="index">Index of value to remove</param>
        public void RemoveAt(int index)
        {
            CheckIndexBoundaryThrow(index);
            if (index < Count - 1)
            {
                vector.Skip(index + 1).ToArray().CopyTo(vector, index);
            }
            Count--;
        }

        /// <summary>
        /// Remove all values that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        public void RemoveAll(Predicate<T> predicate)
        {
            var indices = FindAllIndices(predicate);
            var counter = 0;
            foreach (var index in indices)
            {
                var i = index - counter;
                RemoveAt(index - counter);
                counter++; // Vector shrinks in each step of removing
            }
        }

        /// <summary>
        /// Find first value that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>First value that match given predicate, default value otherwise</returns>
        public T Find(Predicate<T> predicate)
        {
            CheckPredicateNullThrow(predicate);
            foreach (var value in this)
            {
                if (predicate(value))
                {
                    return value;
                }
            }
            return default(T);
        }

        /// <summary>
        /// Find all values that match given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>List of values that match given predicate</returns>
        public List<T> FindAll(Predicate<T> predicate) => FindAllIndices(predicate).Select(index => vector[index]).ToList();

        /// <summary>
        /// Find first value in the vector that is equal to given value and return it's index.
        /// </summary>
        /// <param name="value">Value to find</param>
        /// <returns>Index of first value that is equal to given value, -1 otherwise</returns>
        public int FindIndex(T value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (vector[i].Equals(value))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find all values that match given predicate and return their indices.
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>List of indices of values that match given predicate</returns>
        public List<int> FindAllIndices(Predicate<T> predicate)
        {
            CheckPredicateNullThrow(predicate);
            var result = new List<int>();
            for (int i = 0; i < Count; i++)
            {
                if (predicate(vector[i]))
                {
                    result.Add(i);
                }
            }
            return result;
        }

        /// <summary>
        /// Change vector's current size and also effectively change Count property.
        /// If the new size is less than current size, then the vector will be shrank.
        /// </summary>
        /// <param name="size">New vector's size</param>
        public void Resize(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("Given size must be positive");
            }
            if (size == Count)
            {
                return; // Skip
            }
            Count = size;
            var oldVector = vector;
            vector = new T[Count];
            oldVector.Take(Count).ToArray().CopyTo(vector, 0);
        }
        
        /// <summary>
        /// Change vector's current capacity. If the new capacity is less than current capacity,
        /// then the vector will be shrank.
        /// </summary>
        /// <param name="capacity">New vector's capacity</param>
        public void Reserve(int capacity)
        {
            if (capacity <= Count)
            {
                Resize(capacity);
                return;
            }
            if (vector != null && capacity <= Capacity)
            {
                return; // Skip
            }
            var oldVector = vector;
            vector = new T[capacity];
            oldVector?.CopyTo(vector, 0);
        }

        /// <summary>
        /// Swap two values at given positions.
        /// </summary>
        /// <param name="index1">Position of first value</param>
        /// <param name="index2">Position of second value</param>
        public void Swap(int index1, int index2)
        {
            CheckIndexBoundaryThrow(index1);
            CheckIndexBoundaryThrow(index2);
            var tmp = vector[index1];
            vector[index1] = vector[index2];
            vector[index2] = tmp;
        }

        /// <summary>
        /// Shrink vector's capacity to fit it's size. In other words, Count = Capacity.
        /// </summary>
        public void ShrinkToFit() => Resize(Count);
        
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
            Array.Copy(vector, 0, array, arrayIndex, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return vector[i];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj is Vector<T> vector)
            {
                if (Count != vector.Count)
                {
                    return false;
                }
                for (int i = 0; i < Count; i++)
                {
                    if (!vector.vector[i].Equals(this.vector[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var result = 0;
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
                builder.Append(value);
                builder.Append(';');
            }
            return builder.ToString();
        }

        private void ReserveIfFull()
        {
            if (Count >= Capacity)
            {
                Reserve(Capacity * 2);
            }
        }

        private void VectorMoveForwardOneStep(int index, int offset)
        {
            // Equal to:
            // Array.Copy(vector.ToArray(), index + offset, vector, index + 1 + offset, Count - index - offset);
            // But should be faster, doesn't need deep vector's copy for forward copying...
            for (int i = Count; i > index + offset; i--)
            {
                vector[i] = vector[i - 1];
            }
        }
        
        private void CheckVectorEmptyThrow()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Vector is empty");
            }
        }

        private void CheckIndexBoundaryThrow(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException("Index is out of range");
            }
        }

        private void CheckPredicateNullThrow(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate is null");
            }
        }
    }
}
