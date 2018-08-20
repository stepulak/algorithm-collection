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
        }
    }
}
