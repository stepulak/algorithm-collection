using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class CircularQueueTests
    {
        [TestMethod]
        public void CircularQueueEmptyConstructor()
        {
            var queue = new CircularQueue<int>();
            Assert.AreEqual(queue.Count, 0);
            Assert.IsTrue(queue.Capacity > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CircularQueueConstructorNegativeSize()
        {
            new CircularQueue<int>(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CircularQueueConstructorNullEnumerable()
        {
            new CircularQueue<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CircularQueueEmptyEnumerable()
        {
            new CircularQueue<int>(new LinkedList<int>());
        }

        [TestMethod]
        public void CircularQueueConstructorPositiveSize()
        {
            var queue = new CircularQueue<int>(100);
            Assert.AreEqual(queue.Count, 0);
            Assert.AreEqual(queue.Capacity, 100);
        }

        [TestMethod]
        public void CircularQueueSingleEnqueueDequeue()
        {
            var queue = new CircularQueue<int>();
            queue.Enqueue(100);
            Assert.AreEqual(queue.Count, 1);
            Assert.IsTrue(queue.Capacity > 0);
            Assert.AreEqual(queue.Dequeue(), 100);
            Assert.AreEqual(queue.Count, 0);
        }

        [TestMethod]
        public void CircularQueueMultipleEnqueueDequeue()
        {
            var queue = new CircularQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(10);
            queue.Enqueue(15);
            Assert.AreEqual(queue.Count, 3);
            Assert.AreEqual(queue.Dequeue(), 5);
            Assert.AreEqual(queue.Count, 2);
            Assert.AreEqual(queue.Dequeue(), 10);
            Assert.AreEqual(queue.Count, 1);
            Assert.AreEqual(queue.Dequeue(), 15);
            Assert.AreEqual(queue.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CircularQueueMoreDequeueThanEnqueue()
        {
            var queue = new CircularQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();
            queue.Dequeue();
        }

        [TestMethod]
        public void CircularQueueContains()
        {
            var queue = new CircularQueue<int>(13);
            for (int i = 0; i < 13; i++)
            {
                queue.Enqueue(i);
            }
            Assert.IsTrue(queue.Contains(5));
            Assert.IsTrue(queue.Contains(10));
            Assert.IsFalse(queue.Contains(13));
        }

        [TestMethod]
        public void CircularQueueClear()
        {
            var queue = new CircularQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.AreEqual(queue.Count, 2);
            queue.Clear();
            Assert.AreEqual(queue.Count, 0);
            Assert.IsFalse(queue.Contains(1));
            Assert.IsFalse(queue.Contains(2));
        }

        [TestMethod]
        public void CircularQueueResizeExplicitEnlarge()
        {
            var queue = new CircularQueue<int>(2);
            queue.Enqueue(1);
            queue.Resize(20);
            Assert.AreEqual(queue.Count, 1);
            Assert.AreEqual(queue.Capacity, 20);
            Assert.IsTrue(queue.Contains(1));
        }

        [TestMethod]
        public void CircularQueueResizeExplicitShrink()
        {
            var queue = new CircularQueue<int>(4);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Resize(2);
            Assert.AreEqual(queue.Count, 2);
            Assert.AreEqual(queue.Capacity, 2);
            Assert.IsTrue(queue.Contains(1));
            Assert.IsTrue(queue.Contains(2));
            Assert.IsFalse(queue.Contains(3));
            Assert.IsFalse(queue.Contains(4));
        }

        [TestMethod]
        public void CircularQueueResizeImplicit()
        {
            var queue = new CircularQueue<int>(2);
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.AreEqual(queue.Count, 2);
            Assert.AreEqual(queue.Capacity, 2);
            queue.Enqueue(3);
            Assert.AreEqual(queue.Count, 3);
            Assert.IsTrue(queue.Capacity > 2);
            Assert.IsTrue(queue.Contains(1));
            Assert.IsTrue(queue.Contains(2));
            Assert.IsTrue(queue.Contains(3));
        }

        [TestMethod]
        public void CircularQueueConstructorEnumerable()
        {
            var values = new LinkedList<char> { 'a', 'b', 'c', 'd' };
            var queue = new CircularQueue<char>(values);
            Assert.AreEqual(queue.Count, 4);
            foreach (var value in values)
            {
                Assert.IsTrue(queue.Contains(value));
            }
        }

        [TestMethod]
        public void CircularQueueGetEnumerator()
        {
            var values = new LinkedList<string>
            {
                "Hello", "World", "Unit", "Tests"
            };
            var queue = new CircularQueue<string>();
            foreach (var value in values)
            {
                queue.Enqueue(value);
            }
            foreach (var elem in queue)
            {
                Assert.IsTrue(values.Remove(elem));
            }
            Assert.AreEqual(values.Count, 0);
        }

        [TestMethod]
        public void CircularQueueHeavyLoad()
        {
            var queue = new CircularQueue<int>(5);
            for (int repeats = 1; repeats <= 100; repeats++)
            {
                var values = Enumerable.Range(120, 100).Select(val => val % repeats).ToList();
                foreach (var value in values)
                {
                    queue.Enqueue(value);
                }
                Assert.AreEqual(queue.Count, values.Count);
                Assert.IsTrue(queue.Capacity >= values.Count);
                foreach (var value in values)
                {
                    Assert.AreEqual(queue.Dequeue(), value);
                }
                queue.Clear();
                queue.Resize(repeats % 10 + 1);
            }
        }

        [TestMethod]
        public void CircularQueueEquals()
        {
            var queue = new CircularQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.IsTrue(queue.Equals(queue));
            Assert.IsFalse(queue.Equals(null));
            var queue2 = new CircularQueue<int>();
            Assert.IsFalse(queue.Equals(queue2));
            queue2.Enqueue(1);
            queue2.Enqueue(2);
            Assert.IsTrue(queue2.Equals(queue));
        }

        [TestMethod]
        public void CircularQueueHashCode()
        {
            var queue = new CircularQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            var queue2 = new CircularQueue<int>();
            queue2.Enqueue(4556);
            var queue3 = new CircularQueue<string>();
            Assert.AreNotEqual(queue.GetHashCode(), queue2.GetHashCode());
            Assert.AreNotEqual(queue.GetHashCode(), queue3.GetHashCode());
            queue2.Dequeue();
            queue2.Enqueue(1);
            queue2.Enqueue(2);
            Assert.AreEqual(queue2.GetHashCode(), queue.GetHashCode());
        }

        [TestMethod]
        public void CircularQueueToString()
        {
            var queue = new CircularQueue<string>();
            queue.Enqueue("Hello");
            queue.Enqueue("World");
            Assert.AreEqual(queue.ToString(), "Hello;World;");
            var queue2 = new CircularQueue<int>();
            queue2.Enqueue(1);
            queue2.Enqueue(2);
            queue2.Enqueue(4);
            Assert.AreEqual(queue2.ToString(), "1;2;4;");
        }
    }
}
