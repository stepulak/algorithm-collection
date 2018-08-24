using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class Vector<T> : ICollection<T>
    {
        private const int DefaultCapacity = 4;
        private T[] vector = null;

        public int Count { get; private set; } = 0;
        public bool Empty => Count == 0;
        public int Capacity => vector.Length;
        public bool IsReadOnly => false;

        public T Front
        {
            get
            {
                CheckVectorEmptyThrow();
                return vector[0];
            }
        }

        public T Back
        {
            get
            {
                CheckVectorEmptyThrow();
                return vector[Count - 1];
            }
        }

        public Vector() : this(DefaultCapacity)
        {
        }

        public Vector(int capacity)
        {
            Reserve(capacity);
        }

        public Vector(IEnumerable<T> values)
        {
            var count = values.Count();
            Reserve(count != 0 ? count : DefaultCapacity);
            AddRange(values);
        }

        public void PushBack(T item)
        {
            ReserveIfFull();
            vector[Count++] = item;
        }
        
        public T PopBack()
        {
            CheckVectorEmptyThrow();
            return vector[--Count];
        }

        public void InsertBefore(T item, int index)
        {
            CheckIndexBoundaryThrow(index);
            ReserveIfFull();
            VectorMoveForwardOneStep(index, 0);
            vector[index] = item;
            Count++;
        }

        public void InsertAfter(T item, int index)
        {
            CheckIndexBoundaryThrow(index);
            ReserveIfFull();
            if (index < Count - 1)
            {
                VectorMoveForwardOneStep(index, 1);
            }
            vector[index + 1] = item;
            Count++;
        }

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

        public void Add(T item) => PushBack(item);

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

        public void Clear()
        {
            var capacity = Capacity;
            vector = new T[DefaultCapacity];
            Count = 0;
        }

        public bool Contains(T item) => FindIndex(item) != -1;

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

        public bool Remove(T item)
        {
            var index = FindIndex(item);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            CheckIndexBoundaryThrow(index);
            if (index < Count - 1)
            {
                vector.Skip(index + 1).ToArray().CopyTo(vector, index);
            }
            Count--;
        }

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

        public List<T> FindAll(Predicate<T> predicate) => FindAllIndices(predicate).Select(index => vector[index]).ToList();

        public int FindIndex(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (vector[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

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

        public void Swap(int index1, int index2)
        {
            CheckIndexBoundaryThrow(index1);
            CheckIndexBoundaryThrow(index2);
            var tmp = vector[index1];
            vector[index1] = vector[index2];
            vector[index2] = tmp;
        }

        public void ShrinkToFit() => Resize(Count);

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
