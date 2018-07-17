using System;
using System.Linq;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class QuickSortTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickSortClassicNullArray()
        {
            Sort.QuickSortClassic(null, Comparer<int>.Default);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickSortClassicNullComparer()
        {
            Sort.QuickSortClassic(new int[0], null);
        }
        
        [TestMethod]
        public void QuickSortClassicEmpty()
        {
            Sort.QuickSortClassic(new int[0], Comparer<int>.Default);
        }
        
        [TestMethod]
        public void QuickSortClassicEven()
        {
            {
                var array = new int[] { 100, 200 };
                Sort.QuickSortClassic(array, Comparer<int>.Default);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
            {
                var array = new int[] { 10, 5, 66, -50, 1, 2, 23, -66 };
                Sort.QuickSortClassic(array, Comparer<int>.Default);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
            {
                var random = new Random();
                var array = new int[random.Next(50) * 2 + 1];
                for(int i = 0; i < array.Length; i++)
                {
                    array[i] = random.Next();
                }
                Sort.QuickSortClassic(array, Comparer<int>.Default);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
        }

        [TestMethod]
        public void QuickSortClassicOdd()
        {
            {
                var array = new int[] { -1 };
                Sort.QuickSortClassic(array, Comparer<int>.Default);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
            {
                var array = new int[] { 100, 555, 487, -4874, 1657, 1546, 0, 11, 22 };
                Sort.QuickSortClassic(array, Comparer<int>.Default);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
        }

        [TestMethod]
        public void QuickSortClassicDesceding()
        {
            var array = new int[] { 11, 22, 42, 314, 666, 66, 100, -84, -11, -50, 13, 0, 1, 587, -8467, 0, 154, 15, 13 };
            Sort.QuickSortClassic(array, TestHelper.DescedingComparer<int>());
            Assert.IsTrue(TestHelper.IsSortedAscending(array.Reverse()));
        }

        [TestMethod]
        public void QuickSortClassicFloat()
        {
            var array = new float[] { -12.4f, -666.0f, 873.0f, 31.678f, 763.5f };
            Sort.QuickSortClassic(array, Comparer<float>.Default);
            Assert.IsTrue(TestHelper.IsSortedAscending(array));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickSortLinqNullCollection()
        {
            Sort.QuickSortLinq(null, Comparer<int>.Default);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickSortLinqNullComparer()
        {
            Sort.QuickSortLinq(new int[0], null);
        }

        [TestMethod]
        public void QuickSortLinqEmpty()
        {
            Sort.QuickSortLinq(new int[0], Comparer<int>.Default);
        }

        [TestMethod]
        public void QuickSortLinqOne()
        {
            var collection = new List<int> { 1 };
            Assert.IsTrue(Enumerable.SequenceEqual(collection, Sort.QuickSortLinq(collection, Comparer<int>.Default)));
        }

        [TestMethod]
        public void QuickSortLinqTwo()
        {
            var collection = new List<int> { 1, -5 };
            Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection, Comparer<int>.Default)));
        }

        [TestMethod]
        public void QuickSortLinqMany()
        {
            {
                var collection = new List<int> { 11, 22, 42, 314, 666, 66, 100, -84, -11, -50, 13, 0, 1, 587, -8467, 0, 154, 15, 13 };
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection, Comparer<int>.Default)));
            }
            {
                var collection = new List<double> { -54.2, 66.6, 3.141592, 86.9, -54.2, 15, 13, 13, 0.12 };
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection, Comparer<double>.Default)));
            }
        }

        [TestMethod]
        public void QuickSortLinqManyDesceding()
        {
            {
                var collection = new List<int> { 11, 22, 42, 314, 666, 66, 100, -84, -11, -50, 13, 0, 1, 587, -8467, 0, 154, 15, 13 };
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection, TestHelper.DescedingComparer<int>()).Reverse()));
            }
            {
                var collection = new List<bool> { false, false, true, true };
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection, Comparer<bool>.Default)));
            }
        }
    }
}
