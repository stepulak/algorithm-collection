using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class CircularQueue<T> : IEnumerable<T>
    {
        private const int DefaultBufferSize = 256;

        public int Count => end - start;
        public int Capacity => buffer.Length;
        public bool Empty => Count == 0;

        private int StartIndex => start % Capacity;
        private int EndIndex => end % Capacity;

        private T[] buffer;
        private int start = 0;
        private int end = 0;
        
        public CircularQueue() : this(DefaultBufferSize)
        {

        }

        public CircularQueue(int size)
        {
            CheckSizeThrow(size);
            buffer = new T[size];
        }

        public CircularQueue(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("Given enumerable object is null");
            }
            var count = enumerable.Count();
            if (count == 0)
            {
                throw new ArgumentException("Given enumerable is empty");
            }
            buffer = new T[count * 2];
            foreach (var value in enumerable)
            {
                Enqueue(value);
            }
        }

        public void Clear()
        {
            start = 0;
            end = 0;
        }

        public bool Contains(T value)
        {
            foreach (var elem in this)
            {
                if (elem.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void Enqueue(T value)
        {
            if (end - start == Capacity)
            {
                Resize(Capacity * 2);
            }
            buffer[EndIndex] = value;
            end++;
            FixBufferIndices();
        }

        public T Dequeue()
        {
            if (end <= start)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            var index = StartIndex;
            start++;
            FixBufferIndices();
            return buffer[index];
        }
        
        public void Resize(int size)
        {
            CheckSizeThrow(size);
            var newbuffer = new T[size];
            buffer.Skip(StartIndex)
                .Take(Count > size ? size : Count)
                .ToArray()
                .CopyTo(newbuffer, 0);
            start = 0;
            end = Count > size ? size : Count;
            buffer = newbuffer;
        }

        public IEnumerator<T> GetEnumerator()
        {
            // Since StartIndex == EndIndex && start != end might happen, solve it this way...
            for (int index = start; index < end; index++)
            {
                yield return buffer[index % Capacity];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj is CircularQueue<T> queue)
            {
                if (queue.Count != Count)
                {
                    return false;
                }
                for (int i = 0; i < Count; i++)
                {
                    if (!queue.buffer[i].Equals(buffer[i]))
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
                builder.Append(value);
                builder.Append(';');
            }
            return builder.ToString();
        }

        private void FixBufferIndices()
        {
            if (end >= Capacity && start >= Capacity)
            {
                end -= Capacity;
                start -= Capacity;
            }
        }

        private void CheckSizeThrow(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("Given size must be positive");
            }
        }
    }
}
