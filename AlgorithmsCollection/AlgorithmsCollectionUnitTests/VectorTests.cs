using System;
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
            vector.AddRange(new LinkedList<int> { 1, 2, 3, 4, 5 });
            Assert.AreEqual(vector.Count, 5);
            for (int i = 0; i < vector.Count; i++)
            {
                Assert.AreEqual(vector[i], i + 1);
            }
        }

        [TestMethod]
        public void VectorConstructorEnumerable()
        {
            var vector = new Vector<int>(new LinkedList<int> { 1, 2, 3, 4, 5 });
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
