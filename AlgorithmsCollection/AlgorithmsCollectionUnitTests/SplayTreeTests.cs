using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class SplayTreeTests
    {
        [TestMethod]
        public void SplayTreeAddOneValue()
        {
            var tree = new SplayTree<string, int>();
            var node = tree.Add("1", 1);
            Assert.AreEqual(tree.Count, 1);
            Assert.AreEqual(node.Key, "1");
            Assert.AreEqual(node.Value, 1);
            Assert.IsFalse(node.Parent.HasValue);
            Assert.IsFalse(node.Left.HasValue);
            Assert.IsFalse(node.Right.HasValue);
            Assert.ReferenceEquals(tree.Root.Value.ThisNode, node.ThisNode);
        }

        [TestMethod]
        public void SplayTreeAddThreeValues()
        {
            var tree = new SplayTree<int, int>();
            var node1 = tree.Add(1, 1);
            var node2 = tree.Add(2, 2);
            var node3 = tree.Add(3, 3);
            Assert.AreEqual(node1.Key, 1);
            Assert.AreEqual(node1.Value, 1);
            Assert.AreEqual(node2.Key, 2);
            Assert.AreEqual(node2.Value, 2);
            Assert.AreEqual(node3.Key, 3);
            Assert.AreEqual(node3.Value, 3);
            Assert.AreEqual(tree.Count, 3);
            Assert.IsTrue(tree.Root.HasValue);
            var root = tree.Root.Value;
            Assert.AreEqual(root.Value, 3);
            Assert.AreEqual(root.Key, 3);
            Assert.IsFalse(root.Parent.HasValue);
            Assert.IsTrue(root.Left.HasValue);
            Assert.IsFalse(root.Right.HasValue);
            var left = root.Left.Value;
            Assert.AreEqual(left.Value, 2);
            Assert.AreEqual(left.Key, 2);
            Assert.IsTrue(left.Parent.HasValue);
            Assert.AreEqual(left.Parent.Value.Value, 3);
            Assert.IsFalse(left.Right.HasValue);
            Assert.IsTrue(left.Left.HasValue);
            var leftLeft = left.Left.Value;
            Assert.AreEqual(leftLeft.Value, 1);
            Assert.AreEqual(leftLeft.Key, 1);
            Assert.IsTrue(leftLeft.Parent.HasValue);
            Assert.AreEqual(leftLeft.Parent.Value.Value, 2);
            Assert.IsFalse(leftLeft.Right.HasValue);
            Assert.IsFalse(leftLeft.Left.HasValue);
        }

        [TestMethod]
        public void SplayTreeAddFiveValues()
        {
            var tree = new SplayTree<int, string>();
            Assert.AreEqual(tree.Add(1, "1").Value, "1");
            Assert.AreEqual(tree.Add(2, "2").Value, "2");
            Assert.AreEqual(tree.Add(5, "5").Value, "5");
            Assert.AreEqual(tree.Add(-1, "-1").Value, "-1");
            Assert.AreEqual(tree.Add(4, "4").Value, "4");
            Assert.IsTrue(tree.Root.HasValue);
            var root = tree.Root.Value;
            Assert.AreEqual(root.Value, "4");
            Assert.AreEqual(root.Key, 4);
            Assert.IsFalse(root.Parent.HasValue);
            Assert.IsTrue(root.Left.HasValue);
            Assert.IsTrue(root.Right.HasValue);
            var right = root.Right.Value;
            Assert.IsTrue(right.Parent.HasValue);
            Assert.AreEqual(right.Parent.Value.Value, "4");
            Assert.AreEqual(right.Value, "5");
            Assert.IsFalse(right.Left.HasValue);
            Assert.IsFalse(right.Right.HasValue);
            var left = root.Left.Value;
            Assert.IsTrue(left.Parent.HasValue);
            Assert.AreEqual(left.Parent.Value.Value, "4");
            Assert.AreEqual(left.Value, "-1");
            Assert.IsFalse(left.Left.HasValue);
            Assert.IsTrue(left.Right.HasValue);
            var leftRight = left.Right.Value;
            Assert.IsTrue(leftRight.Parent.HasValue);
            Assert.AreEqual(leftRight.Parent.Value.Value, "-1");
            Assert.AreEqual(leftRight.Value, "2");
            Assert.IsFalse(leftRight.Right.HasValue);
            Assert.IsTrue(leftRight.Left.HasValue);
            var leftRightLeft = leftRight.Left.Value;
            Assert.IsTrue(leftRightLeft.Parent.HasValue);
            Assert.AreEqual(leftRightLeft.Parent.Value.Value, "2");
            Assert.AreEqual(leftRightLeft.Value, "1");
            Assert.IsFalse(leftRightLeft.Right.HasValue);
            Assert.IsFalse(leftRightLeft.Left.HasValue);
        }

        [TestMethod]
        public void SplayTreeFind()
        {
            var tree = new SplayTree<int, int>
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 3 }
            };
            var node = tree.Find(2);
            Assert.AreEqual(tree.Count, 3);
            Assert.IsTrue(tree.Root.HasValue);
            Assert.AreEqual(tree.Root.Value.Value, 2);
            Assert.IsTrue(node.HasValue);
            Assert.AreEqual(node.Value.Key, 2);
            Assert.AreEqual(node.Value.Value, 2);
            Assert.IsFalse(node.Value.Parent.HasValue);
            Assert.IsTrue(node.Value.Left.HasValue);
            Assert.IsTrue(node.Value.Right.HasValue);
            Assert.AreEqual(node.Value.Left.Value.Value, 1);
            Assert.AreEqual(node.Value.Right.Value.Value, 3);
        }

        [TestMethod]
        public void SplayTreeRemoveSimpleTree()
        {
            var tree = new SplayTree<int, int>
            {
                { 1, 1 }
            };
            Assert.IsTrue(tree.Remove(1));
            Assert.AreEqual(tree.Count, 0);
            Assert.IsFalse(tree.Root.HasValue);
            Assert.IsFalse(tree.Remove(0));
        }

        [TestMethod]
        public void SplayTreeRemoveMediumTree()
        {
            var tree = new SplayTree<int, string>
            {
                { 1, "1" },
                { 2, "2" },
                { 3, "3" }
            };
            Assert.IsTrue(tree.Remove(2));
            Assert.AreEqual(tree.Count, 2);
            Assert.IsTrue(tree.Root.HasValue);
            var root = tree.Root.Value;
            Assert.AreEqual(root.Value, "1");
            Assert.IsFalse(root.Parent.HasValue);
            Assert.IsTrue(root.Right.HasValue);
            Assert.IsFalse(root.Left.HasValue);
            var right = root.Right.Value;
            Assert.AreEqual(right.Value, "3");
            Assert.IsTrue(right.Parent.HasValue);
            Assert.AreEqual(right.Parent.Value.Value, "1");
            Assert.IsFalse(right.Left.HasValue);
            Assert.IsFalse(right.Right.HasValue);
        }

        [TestMethod]
        public void SplayTreeRemoveLargeTree()
        {
            var tree = new SplayTree<int, int>()
            {
                { 1, 1 },
                { 2, 2 },
                { 6, 6 },
                { 8, 8 },
                { 5, 5 },
                { 4, 4 }
            };
            Assert.IsTrue(tree.Remove(new KeyValuePair<int, int>(8, 8)));
            Assert.IsTrue(tree.Remove(new KeyValuePair<int, int>(1, 1)));
            Assert.AreEqual(tree.Count, 4);
            Assert.IsTrue(tree.Root.HasValue);
            var root = tree.Root.Value;
            Assert.AreEqual(root.Value, 5);
            Assert.IsFalse(root.Parent.HasValue);
            Assert.IsTrue(root.Left.HasValue);
            Assert.IsTrue(root.Right.HasValue);
            var left = root.Left.Value;
            Assert.AreEqual(left.Value, 2);
            Assert.IsTrue(left.Parent.HasValue);
            Assert.AreEqual(left.Parent.Value.Value, 5);
            Assert.IsFalse(left.Left.HasValue);
            Assert.IsTrue(left.Right.HasValue);
            var leftRight = left.Right.Value;
            Assert.AreEqual(leftRight.Value, 4);
            Assert.IsTrue(leftRight.Parent.HasValue);
            Assert.AreEqual(leftRight.Parent.Value.Value, 2);
            Assert.IsFalse(leftRight.Left.HasValue);
            Assert.IsFalse(leftRight.Right.HasValue);
            var right = root.Right.Value;
            Assert.AreEqual(right.Value, 6);
            Assert.IsTrue(right.Parent.HasValue);
            Assert.AreEqual(right.Parent.Value.Value, 5);
            Assert.IsFalse(right.Left.HasValue);
            Assert.IsFalse(right.Right.HasValue);
        }
    }
}
