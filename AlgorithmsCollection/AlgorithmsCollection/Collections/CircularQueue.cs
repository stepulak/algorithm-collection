using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Templated circular queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularQueue<T> : IEnumerable<T>
    {
        private const int DefaultBufferSize = 256;

        /// <summary>
        /// Number of elements in queue.
        /// </summary>
        public int Count => end - start;

        /// <summary>
        /// Reserved capacity of queue.
        /// </summary>
        public int Capacity => buffer.Length;
        public bool Empty => Count == 0;

        private int StartIndex => start % Capacity;
        private int EndIndex => end % Capacity;

        private T[] buffer;
        private int start = 0;
        private int end = 0;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public CircularQueue() : this(DefaultBufferSize)
        {

        }

        /// <summary>
        /// Create queue with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CircularQueue(int capacity)
        {
            CheckSizeThrow(capacity);
            buffer = new T[capacity];
        }

        /// <summary>
        /// Create queue and fill it with given values.
        /// </summary>
        /// <param name="enumerable">Values to fill with the queue</param>
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

        /// <summary>
        /// Clear the queue and remove all elements.
        /// </summary>
        public void Clear()
        {
            start = 0;
            end = 0;
        }

        /// <summary>
        /// Check if queue contains given value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>True, if queue contains given value, otherwise false.</returns>
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

        /// <summary>
        /// Add value into queue.
        /// </summary>
        /// <param name="value">Value to add</param>
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

        /// <summary>
        /// Return removed first value from queue.
        /// </summary>
        /// <returns>Removed first value.</returns>
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
        
        /// <summary>
        /// Resize queue. If the new size is lower than current size, then the queue will be shrinked.
        /// </summary>
        /// <param name="size">New size</param>
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
                throw new ArgumentOutOfRangeException("Given size/capacity must be positive");
            }
        }
    }
}
