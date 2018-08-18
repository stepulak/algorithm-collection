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

        public int Count { get; private set; }
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
            vector.Skip(index).ToArray().CopyTo(vector, index + 1);
            vector[index] = item;
        }

        public void InsertAfter(T item, int index)
        {
            CheckIndexBoundaryThrow(index);
            ReserveIfFull();
            vector.Skip(index + 1).ToArray().CopyTo(vector, index + 2);
            vector[index + 1] = item;
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
            vector = new T[capacity];
            Count = 0;
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

        public void RemoveAt(int index)
        {

        }

        public void RemoveAll(Predicate<T> predicate)
        {

        }

        public T Find(Predicate<T> predicate)
        {

        }

        public List<T> FindAll(Predicate<T> predicate)
        {

        }

        public int FindIndex(T item)
        {

        }

        public void Resize(int size)
        {

        }
        
        public void Reserve(int size)
        {

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

        private void ReserveIfFull()
        {
            if (Count >= Capacity)
            {
                Reserve(Capacity * 2);
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
    }
}
