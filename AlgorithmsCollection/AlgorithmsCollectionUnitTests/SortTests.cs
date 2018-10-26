using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class SortTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HeapSortArrayNull()
        {
            Sort.HeapSort<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HeapSortComparerNull()
        {
            Sort.HeapSort<int>(new int[] { }, null);
        }
        
        [TestMethod]
        public void HeapSortEmpty()
        {
            var array = new int[0] { };
            Sort.HeapSort(array);
        }

        [TestMethod]
        public void HeapSortSimple()
        {
            var array = new int[] { 66, 42, 314, 22, 11, 13, 13, 0, -56, -69 };
            Sort.HeapSort(array);
            Assert.IsTrue(TestHelper.IsSortedAscending(array));
        }

        [TestMethod]
        public void HeapSortAdvanced()
        {
            {
                var array = new int[] { -42, -42, 314, 22, 0, 41, 14, 11, 13, 13, 0, 5, -56, -800 };
                Sort.HeapSort(array, TestHelper.DescedingComparer<int>());
                Assert.IsTrue(TestHelper.IsSortedAscending(array.Reverse()));
            }
            {
                var array = new float[] { 0.225f, 2.4f, 888.8f, 42.424242f, 0.98f, 0.435f };
                Sort.HeapSort(array, NumericUtilities.FloatNumberComparer<float>());
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
        }

        [TestMethod]
        public void HeapSortLargeArray()
        {
            var rand = new Random();
            var array = new int[500];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next();
            }
            Sort.HeapSort(array, Comparer<int>.Default);
            Assert.IsTrue(TestHelper.IsSortedAscending(array));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeSortArrayNull()
        {
            Sort.MergeSort<double>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeSortComparerNull()
        {
            Sort.MergeSort(new int[] { }, null);
        }

        [TestMethod]
        public void MergeSortArrayEmpty()
        {
            Sort.MergeSort(new int[] { });
        }

        [TestMethod]
        public void MergeSortOneElement()
        {
            var values = new int[] { 42 };
            Sort.MergeSort(values);
            Assert.AreEqual(values[0], 42);
        }

        [TestMethod]
        public void MergeSortEven()
        {
            {
                var values = new int[] { 22, 11 };
                Sort.MergeSort(values);
                Assert.IsTrue(TestHelper.IsSortedAscending(values));
            }
            {
                var values = new int[] { 3, 2, 56, -12, 5, 6 };
                Sort.MergeSort(values);
                Assert.IsTrue(TestHelper.IsSortedAscending(values));
            }
            {
                var values = new int[] { 6, 7, 66, 42, 314, -714, 13, 16 };
                Sort.MergeSort(values, TestHelper.DescedingComparer<int>());
                Assert.IsTrue(TestHelper.IsSortedAscending(values.Reverse()));
            }
        }

        [TestMethod]
        public void MergeSortOdd()
        {
            {
                var values = new int[] { 22, 11, 33 };
                Sort.MergeSort(values);
                Assert.IsTrue(TestHelper.IsSortedAscending(values));
            }
            {
                var values = new int[] { 16, 32, 64, -128, 256, -512, -1024, 2048, 4096 };
                Sort.MergeSort(values, TestHelper.DescedingComparer<int>());
                Assert.IsTrue(TestHelper.IsSortedAscending(values.Reverse()));
            }
        }

        [TestMethod]
        public void MergeSortFloat()
        {
            var values = new float[] { 3.14f, 66.6f, 42.5f, 0.567f, -867.3f, 55.7f, 14.3f };
            Sort.MergeSort(values, NumericUtilities.FloatNumberComparer<float>());
            Assert.IsTrue(TestHelper.IsSortedAscending(values));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickSortClassicNullArray()
        {
            Sort.QuickSortClassic<char>(null);
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
            Sort.QuickSortClassic(new int[0]);
        }

        [TestMethod]
        public void QuickSortClassicEven()
        {
            {
                var array = new int[] { 100, 200 };
                Sort.QuickSortClassic(array);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
            {
                var array = new int[] { 10, 5, 66, -50, 1, 2, 23, -66 };
                Sort.QuickSortClassic(array);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
            {
                var random = new Random();
                var array = new int[random.Next(50) * 2 + 1];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = random.Next();
                }
                Sort.QuickSortClassic(array);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
        }

        [TestMethod]
        public void QuickSortClassicOdd()
        {
            {
                var array = new int[] { -1 };
                Sort.QuickSortClassic(array);
                Assert.IsTrue(TestHelper.IsSortedAscending(array));
            }
            {
                var array = new int[] { 100, 555, 487, -4874, 1657, 1546, 0, 11, 22 };
                Sort.QuickSortClassic(array);
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
            Sort.QuickSortClassic(array);
            Assert.IsTrue(TestHelper.IsSortedAscending(array));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickSortLinqNullCollection()
        {
            Sort.QuickSortLinq<int>(null);
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
            Sort.QuickSortLinq(new int[0]);
        }

        [TestMethod]
        public void QuickSortLinqOne()
        {
            var collection = new List<int> { 1 };
            Assert.IsTrue(Enumerable.SequenceEqual(collection, Sort.QuickSortLinq(collection)));
        }

        [TestMethod]
        public void QuickSortLinqTwo()
        {
            var collection = new List<int> { 1, -5 };
            Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection)));
        }

        [TestMethod]
        public void QuickSortLinqMany()
        {
            {
                var collection = new List<int> { 11, 22, 42, 314, 666, 66, 100, -84, -11, -50, 13, 0, 1, 587, -8467, 0, 154, 15, 13 };
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection)));
            }
            {
                var collection = new List<double> { -54.2, 66.6, 3.141592, 86.9, -54.2, 15, 13, 13, 0.12 };
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection)));
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
                Assert.IsTrue(TestHelper.IsSortedAscending(Sort.QuickSortLinq(collection)));
            }
        }
    }
}
