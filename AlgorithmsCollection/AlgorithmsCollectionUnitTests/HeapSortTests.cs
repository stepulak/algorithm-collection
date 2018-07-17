using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class HeapSortTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HeapSortArrayNull()
        {
            Sort.HeapSort(null, Comparer<int>.Default);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HeapSortComparerNull()
        {
            Sort.HeapSort(new int[] { }, null);
        }

        [TestMethod]
        public void HeapSortEmpty()
        {
            var array = new int[0] { };
            Sort.HeapSort(array, Comparer<int>.Default);
        }

        [TestMethod]
        public void HeapSortSimple()
        {
            var array = new int[] { 66, 42, 314, 22, 11, 13, 13, 0, -56, -69 };
            Sort.HeapSort(array, Comparer<int>.Default);
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

    }
}
