using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class RedBlackTreeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RedBlackTreeConstructorNullComparer()
        {
            new RedBlackTree<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RedBlackTreeConstructorNullInitialValues()
        {
            new RedBlackTree<int>(Comparer<int>.Default, null);
        }

        [TestMethod]
        public void RedBlackTreeConstructor()
        {
            var comparer = Comparer<int>.Default;
            var tree = new RedBlackTree<int>(comparer);
            Assert.IsTrue(tree.Comparer == comparer);
            Assert.AreEqual(tree.Count, 0);
            Assert.IsNull(tree.Tree);
        }

        [TestMethod]
        public void RedBlackTreeAddSingleValue()
        {
            var tree = new RedBlackTree<int>(Comparer<int>.Default);
            tree.Add(5);
            Assert.AreEqual(tree.Count, 1);
            Assert.IsNotNull(tree.Tree);
            var root = tree.Tree.Value;
            Assert.AreEqual(root.Value, 5);
            Assert.AreEqual(root.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNull(root.Parent);
            Assert.IsNull(root.Left);
            Assert.IsNull(root.Right);
        }

        [TestMethod]
        public void RedBlackTreeAddValuesNoBalance()
        {
            var tree = new RedBlackTree<int>(Comparer<int>.Default)
            {
                5,
                1,
                7
            };
            Assert.AreEqual(tree.Count, 3);
            Assert.IsNotNull(tree.Tree);
            var root = tree.Tree.Value;
            Assert.AreEqual(root.Value, 5);
            Assert.AreEqual(root.Color, RedBlackTree<int>.NodeColor.Black);
            var left = root.Left.Value;
            Assert.AreEqual(left.Value, 1);
            Assert.AreEqual(left.Color, RedBlackTree<int>.NodeColor.Red);
            var right = root.Right.Value;
            Assert.AreEqual(right.Value, 7);
            Assert.AreEqual(right.Color, RedBlackTree<int>.NodeColor.Red);
        }
    }
}
