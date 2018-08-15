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
            var queue = new CircularQueue<int>();
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
        public void CircularQueueResizeExplicit()
        {

        }

        [TestMethod]
        public void CircularQueueResizeImplicit()
        {

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
        }
    }
}
