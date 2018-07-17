using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class MergeSortTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeSortArrayNull()
        {
            Sort.MergeSort(null, Comparer<int>.Default);
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
            Sort.MergeSort(new int[] { }, Comparer<int>.Default);
        }
        
        [TestMethod]
        public void MergeSortOneElement()
        {
            var values = new int[] { 42 };
            Sort.MergeSort(values, Comparer<int>.Default);
            Assert.AreEqual(values[0], 42);
        }

        [TestMethod]
        public void MergeSortEven()
        {
            {
                var values = new int[] { 22, 11 };
                Sort.MergeSort(values, Comparer<int>.Default);
                Assert.IsTrue(TestHelper.IsSortedAscending(values));
            }
            {
                var values = new int[] { 3, 2, 56, -12, 5, 6 };
                Sort.MergeSort(values, Comparer<int>.Default);
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
                Sort.MergeSort(values, Comparer<int>.Default);
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
    }
}
