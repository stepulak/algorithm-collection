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
                5, 1, 7
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

        [TestMethod]
        public void RedBlackTreeAddValuesLeftLeftBalance()
        {
            TestRedBlackTreeThreeValues(new RedBlackTree<int>(Comparer<int>.Default) { 3, 2, 1 }, 1, 2, 3);
        }

        [TestMethod]
        public void RedBlackTreeAddValuesLeftRightBalance()
        {
            TestRedBlackTreeThreeValues(new RedBlackTree<int>(Comparer<int>.Default) { 4, 1, 2 }, 1, 2, 4);
        }

        [TestMethod]
        public void RedBlackTreeAddValuesRightRightBalance()
        {
            TestRedBlackTreeThreeValues(new RedBlackTree<int>(Comparer<int>.Default) { 1, 2, 3 }, 1, 2, 3);
        }

        [TestMethod]
        public void RedBlackTreeAddValuesRightLeftBalance()
        {
            TestRedBlackTreeThreeValues(new RedBlackTree<int>(Comparer<int>.Default) { 1, 4, 2 }, 1, 2, 4);
        }

        public void TestRedBlackTreeThreeValues(RedBlackTree<int> tree, int a, int b, int c)
        {
            Assert.AreEqual(tree.Count, 3);
            Assert.IsNotNull(tree.Tree);
            var root = tree.Tree.Value;
            Assert.AreEqual(root.Value, b);
            Assert.AreEqual(root.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNull(root.Parent);
            Assert.IsNotNull(root.Left);
            var left = root.Left.Value;
            Assert.AreEqual(left.Value, a);
            Assert.AreEqual(left.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(left.Parent);
            Assert.AreEqual(left.Parent.Value.Value, b);
            Assert.IsNull(left.Left);
            Assert.IsNull(left.Right);
            Assert.IsNotNull(root.Right);
            var right = root.Right.Value;
            Assert.AreEqual(right.Value, c);
            Assert.AreEqual(right.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(right.Parent);
            Assert.AreEqual(right.Parent.Value.Value, b);
            Assert.IsNull(right.Left);
            Assert.IsNull(right.Right);
        }

        [TestMethod]
        public void RedBlackTreeAddValuesRecolorNodes()
        {
            var tree = new RedBlackTree<int>(Comparer<int>.Default)
            {
                4, 8, 5, 1
            };
            Assert.AreEqual(tree.Count, 4);
            Assert.IsNotNull(tree.Tree);
            var root = tree.Tree.Value;
            Assert.AreEqual(root.Value, 5);
            Assert.AreEqual(root.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNull(root.Parent);
            Assert.IsNotNull(root.Left);
            var left = root.Left.Value;
            Assert.AreEqual(left.Value, 4);
            Assert.AreEqual(left.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNotNull(left.Parent);
            Assert.AreEqual(left.Parent.Value.Value, 5);
            Assert.IsNull(left.Right);
            Assert.IsNotNull(root.Right);
            var right = root.Right.Value;
            Assert.AreEqual(right.Value, 8);
            Assert.AreEqual(right.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNotNull(right.Parent);
            Assert.AreEqual(right.Parent.Value.Value, 5);
            Assert.IsNull(right.Left);
            Assert.IsNull(right.Right);
            Assert.IsNotNull(left.Left);
            var leftLeft = left.Left.Value;
            Assert.AreEqual(leftLeft.Value, 1);
            Assert.AreEqual(leftLeft.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(leftLeft.Parent);
            Assert.AreEqual(leftLeft.Parent.Value.Value, 4);
            Assert.IsNull(leftLeft.Left);
            Assert.IsNull(leftLeft.Right);
        }

        [TestMethod]
        public void RedBlackTreeAddValuesAdvanced()
        {
            var tree = new RedBlackTree<int>(Comparer<int>.Default)
            {
                1, 3, 2, -5, 8, 6, -3
            };
            Assert.AreEqual(tree.Count, 7);
            Assert.IsNotNull(tree.Tree);
            var root = tree.Tree.Value;
            Assert.AreEqual(root.Value, 2);
            Assert.AreEqual(root.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNull(root.Parent);
            Assert.IsNotNull(root.Left);
            Assert.IsNotNull(root.Right);
            var left = root.Left.Value;
            Assert.AreEqual(left.Value, -3);
            Assert.AreEqual(left.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNotNull(left.Parent);
            Assert.AreEqual(left.Parent.Value.Value, 2);
            Assert.IsNotNull(left.Left);
            Assert.IsNotNull(left.Right);
            var right = root.Right.Value;
            Assert.AreEqual(right.Value, 6);
            Assert.AreEqual(right.Color, RedBlackTree<int>.NodeColor.Black);
            Assert.IsNotNull(right.Parent);
            Assert.AreEqual(right.Parent.Value.Value, 2);
            Assert.IsNotNull(right.Left);
            Assert.IsNotNull(right.Right);
            var leftLeft = left.Left.Value;
            Assert.AreEqual(leftLeft.Value, -5);
            Assert.AreEqual(leftLeft.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(leftLeft.Parent);
            Assert.AreEqual(leftLeft.Parent.Value.Value, -3);
            Assert.IsNull(leftLeft.Left);
            Assert.IsNull(leftLeft.Right);
            var leftRight = left.Right.Value;
            Assert.AreEqual(leftRight.Value, 1);
            Assert.AreEqual(leftRight.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(leftRight.Parent);
            Assert.AreEqual(leftRight.Parent.Value.Value, -3);
            Assert.IsNull(leftRight.Left);
            Assert.IsNull(leftRight.Right);
            var rightLeft = right.Left.Value;
            Assert.AreEqual(rightLeft.Value, 3);
            Assert.AreEqual(rightLeft.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(rightLeft.Parent);
            Assert.AreEqual(rightLeft.Parent.Value.Value, 6);
            Assert.IsNull(rightLeft.Left);
            Assert.IsNull(rightLeft.Right);
            var rightRight = right.Right.Value;
            Assert.AreEqual(rightRight.Value, 8);
            Assert.AreEqual(rightRight.Color, RedBlackTree<int>.NodeColor.Red);
            Assert.IsNotNull(rightRight.Parent);
            Assert.AreEqual(rightRight.Parent.Value.Value, 6);
            Assert.IsNull(rightRight.Left);
            Assert.IsNull(rightRight.Right);
        }
    }
}
