using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void VectorEmptyConstructor()
        {
            var vector = new Vector<int>();
            Assert.AreEqual(vector.Count, 0);
            Assert.IsTrue(vector.Empty);
            Assert.IsTrue(vector.Capacity > 0);
        }

        [TestMethod]
        public void VectorConstructorCapacity()
        {
            var vector = new Vector<int>(13);
            Assert.AreEqual(vector.Capacity, 13);
            Assert.AreEqual(vector.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VectorConstructorCapacityZero()
        {
            new Vector<int>(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VectorConstructorCapacityNegative()
        {
            new Vector<int>(-1);
        }

        [TestMethod]
        public void VectorPushBackSimple()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            Assert.AreEqual(vector.Count, 1);
            Assert.AreEqual(vector.Front, 1);
            Assert.AreEqual(vector.Back, 1);
            Assert.IsFalse(vector.Empty);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VectorFrontEmpty()
        {
            var result = new Vector<string>().Front;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VectorBackEmpty()
        {
            var result = new Vector<string>().Back;
        }

        [TestMethod]
        public void VectorPushBackAdvanced()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.PushBack(2);
            vector.PushBack(3);
            Assert.AreEqual(vector.Count, 3);
            Assert.AreEqual(vector.Front, 1);
            Assert.AreEqual(vector.Back, 3);
        }

        [TestMethod]
        public void VectorIndexer()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.PushBack(2);
            vector.PushBack(3);
            Assert.AreEqual(vector[0], 1);
            Assert.AreEqual(vector[1], 2);
            Assert.AreEqual(vector[2], 3);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void VectorIndexerInvalidIndexUnderflow()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            var result = vector[-1];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void VectorIndexerInvalidIndexOverflow()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            var result = vector[1];
        }

        [TestMethod]
        public void VectorAdd()
        {
            var vector = new Vector<string>();
            vector.Add("Hello");
            Assert.AreEqual(vector.Front, "Hello");
            Assert.AreEqual(vector.Count, 1);
        }

        [TestMethod]
        public void VectorAddRange()
        {
            var vector = new Vector<int>(10);
            vector.AddRange(new List<int> { 1, 2, 3, 4, 5 });
            Assert.AreEqual(vector.Count, 5);
            for (int i = 0; i < vector.Count; i++)
            {
                Assert.AreEqual(vector[i], i + 1);
            }
        }

        [TestMethod]
        public void VectorConstructorEnumerable()
        {
            var vector = new Vector<int>(new List<int> { 1, 2, 3, 4, 5 });
            Assert.AreEqual(vector.Count, 5);
            for (int i = 0; i < vector.Count; i++)
            {
                Assert.AreEqual(vector[i], i + 1);
            }
        }
        [TestMethod]
        public void VectorPopBack()
        {
            var vector = new Vector<string>();
            vector.PushBack("Hello");
            vector.PushBack("World");
            Assert.AreEqual(vector.PopBack(), "World");
            Assert.AreEqual(vector.PopBack(), "Hello");
            Assert.AreEqual(vector.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VectorPopBackOnEmpty()
        {
            var result = new Vector<int>().PopBack();
        }

        [TestMethod]
        public void VectorInsertBefore()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.InsertBefore(2, 0);
            Assert.AreEqual(vector.Count, 2);
            Assert.AreEqual(vector[0], 2);
            Assert.AreEqual(vector[1], 1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void VectorInsertBeforeEmpty()
        {
            new Vector<int>().InsertBefore(1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void VectorInsertBeforeInvalidIndex()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.InsertBefore(2, -1);
        }

        [TestMethod]
        public void VectorInsertAfter()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.PushBack(2);
            vector.InsertAfter(3, 0);
            Assert.AreEqual(vector.Count, 3);
            Assert.AreEqual(vector[0], 1);
            Assert.AreEqual(vector[1], 3);
            Assert.AreEqual(vector[2], 2);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void VectorInsertAfterInvalidIndex()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.InsertAfter(2, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void VectorInsertAfterEmpty()
        {
            new Vector<int>().InsertAfter(1, 0);
        }

        [TestMethod]
        public void VectorInsertBeforeAfterAdvanced()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.InsertBefore(2, 0);
            vector.InsertBefore(3, 0);
            vector.InsertAfter(4, 1);
            vector.InsertAfter(5, 0);
            vector.InsertBefore(6, 4);
            Assert.AreEqual(vector.Count, 6);
            Assert.AreEqual(vector[0], 3);
            Assert.AreEqual(vector[1], 5);
            Assert.AreEqual(vector[2], 2);
            Assert.AreEqual(vector[3], 4);
            Assert.AreEqual(vector[4], 6);
            Assert.AreEqual(vector[5], 1);
        }

        [TestMethod]
        public void VectorClear()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.PushBack(2);
            vector.Clear();
            Assert.AreEqual(vector.Count, 0);
            Assert.IsTrue(vector.Capacity > 0);
        }

        [TestMethod]
        public void VectorContains()
        {
            var vector = new Vector<string> { "A", "B", "C", "D" };
            Assert.IsTrue(vector.Contains("A"));
            Assert.IsTrue(vector.Contains("D"));
            Assert.IsFalse(vector.Contains("E"));
        }

        [TestMethod]
        public void VectorCopyTo()
        {
            var vector = new Vector<int> { 1, 2, 3, 4 };
            var array = new int[5];
            vector.CopyTo(array, 1);
            for (int i = 1; i < 5; i++)
            {
                Assert.AreEqual(array[i], i);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VectorCopyToArrayNull()
        {
            new Vector<int>().CopyTo(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VectorCopyToArrayIndexNegative()
        {
            var array = new int[0];
            new Vector<int>().CopyTo(array, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VectorCopyToArrayTooSmall()
        {
            var vector = new Vector<int> { 1, 2, 3, 4 };
            var array = new int[3];
            vector.CopyTo(array, 0);
        }
        
        [TestMethod]
        public void VectorFind()
        {
            var vector = new Vector<int> { 1, 2, 3, 4 };
            Assert.AreEqual(vector.Find(v => v > 2 && v < 4), 3);
            Assert.AreEqual(vector.Find(v => v > 5), default(int));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VectorFindNullPredicate()
        {
            var result = new Vector<int>().Find(null);
        }

        [TestMethod]
        public void VectorFindAll()
        {
            var vector = new Vector<int> { 1, 2, 3, 4, 5 };
            Assert.IsTrue(vector.FindAll(v => v > 2 && v < 6).SequenceEqual(new List<int> { 3, 4, 5 }));
            Assert.AreEqual(vector.FindAll(v => v > 7).Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VectorFindAllNullPredicate()
        {
            var result = new Vector<int>().Find(null);
        }

        [TestMethod]
        public void VectorFindIndex()
        {
            var vector = new Vector<string> { "Abc", "aBc", "abC" };
            Assert.AreEqual(vector.FindIndex("aBc"), 1);
            Assert.AreEqual(vector.FindIndex("AAB"), -1);
        }

        [TestMethod]
        public void VectorFindAllIndices()
        {
            var vector = new Vector<char> { 'a', 'b', 'c', 'd' };
            Assert.IsTrue(vector.FindAllIndices(v => v == 'a' || v == 'b').SequenceEqual(new List<int> { 0, 1 }));
            Assert.AreEqual(vector.FindAllIndices(v => v > 'e').Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VectorFindAllIndicesNullPredicate()
        {
            new Vector<int>().FindAllIndices(null);
        }

        [TestMethod]
        public void VectorReserve()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.PushBack(2);
            vector.Reserve(100);
            Assert.AreEqual(vector.Count, 2);
            Assert.AreEqual(vector.Capacity, 100);
            Assert.AreEqual(vector.Front, 1);
            Assert.AreEqual(vector.Back, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VectorReserveZeroCapacity()
        {
            var vector = new Vector<string>();
            vector.Reserve(0);
        }

        [TestMethod]
        public void VectorResizeEnlarge()
        {
            var vector = new Vector<int>();
            vector.PushBack(1);
            vector.PushBack(2);
            vector.PushBack(3);
            vector.Resize(10);
            Assert.AreEqual(vector.Count, 10);
            Assert.AreEqual(vector.Capacity, 10);
            Assert.AreEqual(vector[0], 1);
            Assert.AreEqual(vector[1], 2);
            Assert.AreEqual(vector[2], 3);
            var result = vector[3]; // shouldn't throw an exception
        }

        [TestMethod]
        public void VectorResizeShrink()
        {
            var vector = new Vector<int>();
            for (int i = 0; i < 4; i++)
            {
                vector.PushBack(i + 1);
            }
            vector.Resize(2);
            Assert.AreEqual(vector.Count, 2);
            Assert.AreEqual(vector.Capacity, 2);
            Assert.AreEqual(vector[0], 1);
            Assert.AreEqual(vector[1], 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VectorResizeZeroSize()
        {
            var vector = new Vector<int>();
            vector.Resize(0);
        }

        [TestMethod]
        public void VectorShrinkToFit()
        {
            var vector = new Vector<int>();
            for (int i = 0; i < 4; i++)
            {
                vector.PushBack(i + 1);
            }
            vector.ShrinkToFit();
            Assert.AreEqual(vector.Count, 4);
            Assert.AreEqual(vector.Capacity, 4);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(vector[i], i + 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VectorShrinkToFitWhenEmpty()
        {
            new Vector<int>().ShrinkToFit();
        }


    }
}
