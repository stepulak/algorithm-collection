using System;
using System.Linq;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    // We cannot use using statement with whole System.Collections.Generic namespace
    // Becase our LinkedList would be in name collision with .Net's LinkedList.
    using ListInt = System.Collections.Generic.List<int>;
    using ListFloat = System.Collections.Generic.List<float>;

    [TestClass]
    public class LinkedListTests
    {
        [TestMethod]
        public void LinkedListPushFrontSimple()
        {
            var list = new LinkedList<int>();
            list.PushFront(42);
            CheckSingleElementList(list, 42);
        }

        [TestMethod]
        public void LinkedListPushBackSimple()
        {
            var list = new LinkedList<int>();
            list.PushBack(42);
            CheckSingleElementList(list, 42);
        }

        [TestMethod]
        public void LinkedListPushFrontAdvanced()
        {
            var list = new LinkedList<int>();
            list.PushFront(16);
            list.PushFront(7);
            list.PushFront(13);
            CheckList_13_7_16(list);
        }

        [TestMethod]
        public void LinkedListPushBackAdvanced()
        {
            var list = new LinkedList<int>();
            list.PushBack(13);
            list.PushBack(7);
            list.PushBack(16);
            CheckList_13_7_16(list);
        }

        [TestMethod]
        public void LinkedListAt()
        {
            var values = new ListInt { 1, 2, 3, 4, 5, 6 };
            var list = new LinkedList<int>(values);
            for (int i = 0; i < values.Count; i++)
            {
                Assert.AreEqual(list[i], values[i]);
                Assert.AreEqual(list.NodeAt(i).Value, values[i]);
            }
            Assert.AreEqual(list.NodeAt(0).ThisNode, list.FrontNode.Value.ThisNode);
            Assert.AreEqual(list.NodeAt(5).ThisNode, list.BackNode.Value.ThisNode);
            list[0] = 100;
            list[1] = 200;
            Assert.AreEqual(list[0], 100);
            Assert.AreEqual(list[1], 200);
        }

        [TestMethod]
        public void LinkedListPushMixed()
        {
            var list = new LinkedList<byte>();
            list.PushBack(8);
            list.PushFront(6);
            list.PushBack(7);
            list.PushBack(13);
            Assert.AreEqual(list[0], 6);
            Assert.AreEqual(list[1], 8);
            Assert.AreEqual(list[2], 7);
            Assert.AreEqual(list[3], 13);
            Assert.AreEqual(list.FrontNode.Value.Value, 6);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Value, 8);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Next.Value.Value, 7);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Next.Value.Next.Value.Value, 13);
            Assert.AreEqual(list.BackNode.Value.Value, 13);
            Assert.AreEqual(list.BackNode.Value.Previous.Value.Value, 7);
            Assert.AreEqual(list.BackNode.Value.Previous.Value.Previous.Value.Value, 8);
            Assert.AreEqual(list.BackNode.Value.Previous.Value.Previous.Value.Previous.Value.Value, 6);
        }

        [TestMethod]
        public void LinkedListEmpty()
        {
            var list = new LinkedList<int>();
            CheckEmptyList(list);
        }

        [TestMethod]
        public void LinkedListClear()
        {
            var list = new LinkedList<float> { 1.2f, 2.5f, 6.9f, 4.2f };
            Assert.AreEqual(list.Count, 4);
            Assert.IsFalse(list.Empty);
            list.Clear();
            CheckEmptyList(list);
        }

        [TestMethod]
        public void LinkedListInsertBefore()
        {
            var list = new LinkedList<int>();
            list.PushBack(100);
            list.InsertBefore(0, 42);
            Assert.AreEqual(list[0], 42);
            Assert.AreEqual(list[1], 100);
            Assert.AreEqual(list.FrontNode.Value.Value, 42);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Value, 100);
            list.InsertBefore(list.NodeAt(1), 13);
            Assert.AreEqual(list[1], 13);
            Assert.AreEqual(list.NodeAt(1).Previous.Value.Value, 42);
            Assert.AreEqual(list.NodeAt(1).Next.Value.Value, 100);
        }

        [TestMethod]
        public void LinkedListInsertAfter()
        {
            var list = new LinkedList<int>();
            list.PushBack(100);
            list.InsertAfter(0, 42);
            Assert.AreEqual(list[0], 100);
            Assert.AreEqual(list[1], 42);
            Assert.AreEqual(list.BackNode.Value.Value, 42);
            Assert.AreEqual(list.BackNode.Value.Previous.Value.Value, 100);
            list.InsertAfter(list.NodeAt(0), 13);
            Assert.AreEqual(list[1], 13);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Value, 13);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Next.Value.Value, 42);
        }

        [TestMethod]
        public void LinkedListPopFront()
        {
            var values = new ListInt { 1, 2, 3, 4 };
            var list = new LinkedList<int>(values);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(values[i], list.PopFront());
                Assert.AreEqual(list.Count, 4 - i - 1);
            }
            Assert.IsTrue(list.Empty);
            Assert.IsFalse(list.FrontNode.HasValue);
            Assert.IsFalse(list.BackNode.HasValue);
        }

        [TestMethod]
        public void LinkedListPopBack()
        {
            var values = new ListInt { 1, 2, 3, 4 };
            var list = new LinkedList<int>(values.AsEnumerable().Reverse());
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(values[i], list.PopBack());
                Assert.AreEqual(list.Count, 4 - i - 1);
            }
            Assert.IsTrue(list.Empty);
            Assert.IsFalse(list.FrontNode.HasValue);
            Assert.IsFalse(list.BackNode.HasValue);
        }

        [TestMethod]
        public void LinkedListFindAndContains()
        {
            var list = new LinkedList<int> { 66, 42, 13, 22, 11, 11, 22, 13, 54, 8473, -123, -34, 55, 13, -22 };
            Assert.IsTrue(list.Contains(-34));
            Assert.IsFalse(list.Contains(666));
            Assert.AreEqual(list.Find(v => v == 22).Value.ThisNode, list.NodeAt(3).ThisNode);
            var result = list.FindAll(v => v == 13);
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0].ThisNode, list.NodeAt(2).ThisNode);
            Assert.AreEqual(result[1].ThisNode, list.NodeAt(7).ThisNode);
            Assert.AreEqual(result[2].ThisNode, list.NodeAt(13).ThisNode);
        }

        [TestMethod]
        public void LinkedListRemoveValue()
        {
            var list = new LinkedList<int> { 1, 2, 3 };
            Assert.IsTrue(list.Remove(3));
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list.BackNode.Value.Next, null);
            Assert.IsFalse(list.Remove(100));
            Assert.IsTrue(list.Remove(1));
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list.BackNode.Value.ThisNode, list.FrontNode.Value.ThisNode);
            Assert.AreEqual(list.FrontValue, list.BackValue);
            Assert.AreEqual(list.FrontValue, 2);
        }

        [TestMethod]
        public void LinkedListRemoveNodeAt()
        {
            var list = new LinkedList<int> { 1, 2, 3 };
            list.RemoveAt(1);
            Assert.AreEqual(list.FrontNode.Value.Value, 1);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Value, 3);
            Assert.AreEqual(list.BackNode.Value.Value, 3);
            Assert.AreEqual(list.BackNode.Value.Previous.Value.Value, 1);
            Assert.IsFalse(list.FrontNode.Value.Previous.HasValue);
            Assert.IsFalse(list.BackNode.Value.Next.HasValue);
            list.RemoveNode(list.NodeAt(0));
            Assert.AreEqual(list.FrontNode.Value.Value, 3);
            Assert.IsFalse(list.FrontNode.Value.Next.HasValue);
            Assert.IsFalse(list.FrontNode.Value.Previous.HasValue);
            list.PushFront(100);
            list.RemoveNode(list.BackNode.Value);
            Assert.AreEqual(list.FrontNode.Value.Value, 100);
            Assert.IsFalse(list.FrontNode.Value.Next.HasValue);
            Assert.IsFalse(list.FrontNode.Value.Previous.HasValue);
        }

        [TestMethod]
        public void LinkedListRemoveAll()
        {
            var list = new LinkedList<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            list.RemoveAll(v => v > 2 && v < 7);
            Assert.IsTrue(list.Equals(new LinkedList<int> { 1, 2, 7, 8, 9 }));
        }

        [TestMethod]
        public void LinkedListSwapNodes()
        {
            var list = new LinkedList<int> { 1, 2, 3, 4 };
            list.Swap(0, 1);
            list.Swap(list.NodeAt(1), list.NodeAt(3));
            Assert.AreEqual(list[0], 2);
            Assert.AreEqual(list[1], 4);
            Assert.AreEqual(list[2], 3);
            Assert.AreEqual(list[3], 1);
        }

        [TestMethod]
        public void LinkedListEquals()
        {
            var values = new ListInt { 18, 25, 33, 42 };
            var list = new LinkedList<int>(values);
            Assert.IsTrue(new LinkedList<int>(values).Equals(list));
            Assert.AreEqual(new LinkedList<int>(values).GetHashCode(), list.GetHashCode());
            Assert.IsFalse(new LinkedList<int> { 58, 47, 12 }.Equals(list));
        }

        [TestMethod]
        public void LinkedListSort()
        {
            var list = new LinkedList<float> { -54.4f, 18.4f, 3.14f, 85.5f, -111.11f, 54.34f, 41.5f, 69.4f };
            list.Sort(NumericUtilities.FloatNumberComparer<float>());
            Assert.IsTrue(TestHelper.IsSortedAscending(list));
        }

        [TestMethod]
        public void LinkedListReverse()
        {
            var values = new ListInt { 45, 56, 12, 1, 23, -23, 1, 21, -294, -24, 13, 14, 22 };
            var list = new LinkedList<int>(values);
            list.Reverse();
            Assert.IsTrue(list.SequenceEqual(values.AsEnumerable().Reverse()));
        }

        [TestMethod]
        public void LinkedListAppendList()
        {
            var list = new LinkedList<int> { 1, 2 };
            list.AppendValues(new LinkedList<int> { 3, 4 });
            list.AppendValues(new ListInt { 5, 6 });
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(i + 1, list[i]);
            }
        }

        [TestMethod]
        public void LinkedListEnumerate()
        {
            var values = new ListFloat { 42.42f, 3.1415f, 18.4f, 89.25f, 56.35f };
            int index = 0;
            foreach (var value in new LinkedList<float>(values))
            {
                Assert.AreEqual(value, values[index++]);
            }
        }

        [TestMethod]
        public void LinkedListCopyTo()
        {
            var values = new ListInt { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new int[values.Count];
            new LinkedList<int>(values).CopyTo(array, 0);
            Assert.IsTrue(array.SequenceEqual(values));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LinkedListCreateNullValues()
        {
            new LinkedList<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LinkedListAppendNullValues()
        {
            new LinkedList<int>().AppendValues(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LinkedListAtInvalidIndex()
        {
            new LinkedList<int> { 1, 2, 3 }.NodeAt(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LinkedListRemoveAtInvalidIndex()
        {
            new LinkedList<int> { 1, 2, 3 }.RemoveAt(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListPopFrontEmptyList()
        {
            new LinkedList<int>().PopFront();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListPopBackEmptyList()
        {
            new LinkedList<int>().PopBack();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LinkedListInsertBeforeInvalid()
        {
            new LinkedList<int>().InsertBefore(-1, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LinkedListInsertAfterInvalid()
        {
            new LinkedList<int>().InsertBefore(1, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListPopEmptyList()
        {
            new LinkedList<int>().PopBack();
        }

        private void CheckEmptyList<T>(LinkedList<T> list)
        {
            Assert.AreEqual(list.Count, 0);
            Assert.IsTrue(list.Empty);
            Assert.AreEqual(list.FrontNode, null);
            Assert.AreEqual(list.BackNode, null);
        }

        private void CheckSingleElementList<T>(LinkedList<T> list, T value)
        {
            Assert.AreEqual(list.FrontValue, value);
            Assert.AreEqual(list.BackValue, value);
            Assert.AreEqual(list.FrontNode.HasValue, true);
            Assert.AreEqual(list.BackNode.HasValue, true);
            var frontNode = list.FrontNode.Value;
            var backNode = list.BackNode.Value;
            Assert.AreEqual(frontNode.ThisNode, backNode.ThisNode);
            Assert.AreEqual(frontNode.Value, backNode.Value);
            Assert.AreEqual(frontNode.Value, value);
            Assert.AreEqual(frontNode.Next, null);
            Assert.AreEqual(frontNode.Previous, null);
            Assert.AreEqual(backNode.Next, null);
            Assert.AreEqual(backNode.Previous, null);
            Assert.AreEqual(list.Count, 1);
            Assert.IsFalse(list.Empty);
        }

        private void CheckList_13_7_16(LinkedList<int> list)
        {
            Assert.AreEqual(list.Count, 3);
            Assert.AreEqual(list.FrontValue, 13);
            Assert.AreEqual(list.BackValue, 16);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.Value, 7);
            Assert.AreEqual(list.BackNode.Value.Previous.Value.Value, 7);
            Assert.AreEqual(list.FrontNode.Value.Previous, null);
            Assert.AreEqual(list.BackNode.Value.Next, null);
            Assert.AreEqual(list.FrontNode.Value.Next.Value.ThisNode, list.BackNode.Value.Previous.Value.ThisNode);
        }
    }
}
