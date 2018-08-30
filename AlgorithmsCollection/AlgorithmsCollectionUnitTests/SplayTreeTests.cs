using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class SplayTreeTests
    {
        [TestMethod]
        public void SplayTreeInsertThreeValues()
        {
            var tree = new SplayTree<int, int>();
            var node1 = tree.Insert(1, 1);
            var node2 = tree.Insert(2, 2);
            var node3 = tree.Insert(3, 3);
            Assert.AreEqual(node1.Key, 1);
            Assert.AreEqual(node1.Value, 1);
            Assert.AreEqual(node2.Key, 2);
            Assert.AreEqual(node2.Value, 2);
            Assert.AreEqual(node3.Key, 3);
            Assert.AreEqual(node3.Value, 3);
            Assert.AreEqual(tree.Count, 3);
            Assert.IsTrue(tree.Root.HasValue);
            Assert.AreEqual(tree.Root.Value.Value, 3);
            Assert.AreEqual(tree.Root.Value.Key, 3);
            Assert.IsFalse(tree.Root.Value.Parent.HasValue);
            Assert.IsTrue(tree.Root.Value.Left.HasValue);
            Assert.IsFalse(tree.Root.Value.Right.HasValue);
            var left = tree.Root.Value.Left.Value;
            Assert.AreEqual(left.Value, 2);
            Assert.AreEqual(left.Key, 2);
            Assert.IsTrue(left.Parent.HasValue);
            Assert.AreEqual(left.Parent.Value.Value, 3);
            Assert.IsFalse(left.Right.HasValue);
            Assert.IsTrue(left.Left.HasValue);
            left = left.Left.Value;
            Assert.AreEqual(left.Value, 1);
            Assert.AreEqual(left.Key, 1);
            Assert.IsTrue(left.Parent.HasValue);
            Assert.AreEqual(left.Parent.Value.Value, 2);
            Assert.IsFalse(left.Right.HasValue);
            Assert.IsFalse(left.Left.HasValue);
        }
    }
}
