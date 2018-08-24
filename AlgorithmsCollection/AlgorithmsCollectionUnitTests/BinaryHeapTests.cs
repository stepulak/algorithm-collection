using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class BinaryHeapTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BinaryHeapNullComparer()
        {
            new BinaryHeap<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BinaryHeapNullComparer2()
        {
            new BinaryHeap<int>(null, new List<int> { 1, 2, 3 });
        }

        [TestMethod]
        public void BinaryHeapPushPopSimple()
        {
            var heap = new BinaryHeap<int>(Comparer<int>.Default);
            var values = new List<int> { 1, 2, 3, -1, 7 };
            foreach(var value in values)
            {
                heap.Push(value);
            }
            values.Sort();
            foreach(var value in values)
            {
                Assert.AreEqual(value, heap.Pop());
            }
        }

        [TestMethod]
        public void BinaryHeapPushRangePopSimple()
        {
            var heap = new BinaryHeap<int>(Comparer<int>.Default);
            var values = new List<int> { 1, -5, 6, 7, -6, 11, -22, 6345, -3423, 34, 23412, 12, 12, 543, 12, 1, -22 };
            heap.PushRange(values);
            values.Sort();
            foreach(var value in values)
            {
                Assert.AreEqual(value, heap.Pop());
            }
        }

        [TestMethod]
        public void BinaryHeapRoot()
        {
            var heap = new BinaryHeap<bool>(Comparer<bool>.Default);
            heap.Push(true);
            heap.Push(false);
            Assert.IsFalse(heap.Pop());
            Assert.IsTrue(heap.Pop());
        }

        [TestMethod]
        public void BinaryHeapCount()
        {
            var heap = new BinaryHeap<bool>(Comparer<bool>.Default) { false, true, true, false, false };
            Assert.AreEqual(heap.Count, 5);  
        }

        [TestMethod]
        public void BinaryHeapClear()
        {
            var heap = new BinaryHeap<bool>(Comparer<bool>.Default) { false, true, true, false, false };
            heap.Clear();
            Assert.AreEqual(heap.Count, 0);
        }

        [TestMethod]
        public void BinaryHeapReserve()
        {
            var values = new List<bool> { false, true, true, false, false };
            var heap = new BinaryHeap<bool>(Comparer<bool>.Default, values);
            heap.ReserveCapacity(100);
            heap.PushRange(values);
            heap.PushRange(values);
            Assert.AreEqual(heap.Count, 15);
        }

        [TestMethod]
        public void BinaryHeapFind()
        {
            var heap = new BinaryHeap<uint>(Comparer<uint>.Default) { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Predicate<uint> findNumberInHeap = (number) => heap.Find(v => v == number) == number;
            Assert.IsTrue(findNumberInHeap(4));
            Assert.IsTrue(findNumberInHeap(1));
            Assert.IsFalse(findNumberInHeap(11));
        }

        [TestMethod]
        public void BinaryHeapRemove()
        {
            var heap = new BinaryHeap<short>(TestHelper.DescedingComparer<short>()) { 11, 22, 22, 33, 44, 55, 66, 77, 88, 99 };
            Assert.IsTrue(heap.Remove(88));
            Assert.IsTrue(heap.Tree[1] != 88 && heap.Tree[2] != 88);
            Assert.IsTrue(heap.Remove(v => v == 66));
            Assert.AreEqual(heap.Pop(), 99);
            Assert.AreEqual(heap.Pop(), 77);
            Assert.AreEqual(heap.Pop(), 55);
            heap.Remove(33);
            heap.Remove(v => v == 22);
            Assert.AreEqual(heap.Pop(), 44);
            Assert.AreEqual(heap.Pop(), 22);
            Assert.AreEqual(heap.Pop(), 11);
            try
            {
                heap.Pop();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void BinaryHeapRemoveAll()
        {
            var heap = new BinaryHeap<float>(NumericUtilities.FloatNumberComparer<float>()) { 0.1f, 0.56f, 0.234f, 0.66f, 0.231f, 0.234f, 0.1f };
            heap.RemoveAll(0.1f);
            Assert.IsTrue(heap.Comparer.Compare(0.231f, heap.Pop()) == 0);
            heap.RemoveAll(v => heap.Comparer.Compare(0.234f, v) == 0);
            Assert.IsTrue(heap.Comparer.Compare(0.56f, heap.Pop()) == 0);
            Assert.IsTrue(heap.Comparer.Compare(0.66f, heap.Pop()) == 0);
            Assert.AreEqual(heap.Count, 0);
        }

        [TestMethod]
        public void BinaryHeapEnumerate()
        {
            var values = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var heap = new BinaryHeap<int>(Comparer<int>.Default, values);
            foreach(var value in heap)
            {
                if (!values.Remove(value))
                {
                    Assert.Fail();
                }
            }
            Assert.AreEqual(values.Count, 0);
        }
        
        [TestMethod]
        public void BinaryHeapEquals()
        {
            var heap1 = new BinaryHeap<int>(Comparer<int>.Default, Enumerable.Range(1, 6));
            var heap2 = new BinaryHeap<int>(Comparer<int>.Default, Enumerable.Range(1, 6).Reverse());
            Assert.IsTrue(heap1.Equals(heap2));
            Assert.IsFalse(heap1.Equals(heap2.Reverse()));
        }

        [TestMethod]
        public void BinaryHeapHashCode()
        {
            var heap1 = new BinaryHeap<int>(Comparer<int>.Default, Enumerable.Range(1, 6));
            var heap2 = new BinaryHeap<int>(Comparer<int>.Default, Enumerable.Range(1, 6).Reverse());
            Assert.AreEqual(heap1.GetHashCode(), heap2.GetHashCode());
            Assert.AreNotEqual(heap1.GetHashCode(), heap2.Reverse().GetHashCode());
        }

        [TestMethod]
        public void BinaryHeapToString()
        {
            var heap = new BinaryHeap<int>(Comparer<int>.Default, Enumerable.Range(1, 6));
            Assert.AreEqual(heap.ToString(), "1;2;3;4;5;6;");
        }

        [TestMethod]
        public void BinaryHeapCopyTo()
        {
            var destination = new int[5];
            new BinaryHeap<int>(Comparer<int>.Default) { 5, 2, 4, 1, 3 }.CopyTo(destination, 0);
            for (int i = 0; i < destination.Length; i++)
            {
                Assert.AreEqual(destination[i], i + 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BinaryHeapEmptyValues()
        {
            new BinaryHeap<int>(Comparer<int>.Default).PushRange(null);
        }

        [TestMethod]
        public void BinaryHeapChangeIndexNotifier()
        {
            var dic = new Dictionary<int, int>();
            var notifier = new BinaryHeap<int>.ChangeIndexNotifier((value, index) => dic[value] = index);
            var heap = new BinaryHeap<int>(Comparer<int>.Default, notifier);
            heap.Push(100);
            Assert.AreEqual(dic[100], 0);
            heap.Push(5);
            Assert.AreEqual(dic[5], 0);
            Assert.AreEqual(dic[100], 1);
            heap.Push(-5);
            heap.Push(20);
            Assert.AreEqual(dic[20], 1);
            Assert.AreEqual(dic[5], 2);
            Assert.AreEqual(dic[100], 3);
            heap.Remove(20);
            Assert.AreEqual(dic[100], 1);
        }
    }
}
