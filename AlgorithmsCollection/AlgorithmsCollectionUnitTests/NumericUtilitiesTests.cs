using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class NumericUtilitiesTests
    {
        [TestMethod]
        public void DoubleCompare()
        {
            Assert.IsTrue(NumericUtilities.DoubleCompare(1.24, 1.2400000001));
            Assert.IsTrue(NumericUtilities.DoubleCompare(4.7 / 3.14, 4.7 * (1.0 / 3.14)));
            Assert.IsFalse(NumericUtilities.DoubleCompare(2.52, 2.53));
        }

        [TestMethod]
        public void FloatNumberComparer()
        {
            var comparer = NumericUtilities.FloatNumberComparer<double>();
            Assert.IsTrue(comparer.Compare(1.0, 2.0) < 0);
            Assert.AreEqual(comparer.Compare(0.02, 0.02), 0);
            Assert.IsTrue(comparer.Compare(13.07, 11.22) > 0);
        }

        [TestMethod]
        public void FloatNumberEqualityComparer()
        {
            var comparer = NumericUtilities.FloatNumberEqualityComparer<double>();
            Assert.IsTrue(comparer.Equals(1.001, 1.001));
            Assert.IsFalse(comparer.Equals(654.3210, 654.3211));
        }

        [TestMethod]
        public void IsPrime()
        {
            Assert.IsTrue(NumericUtilities.IsPrime(2));
            Assert.IsTrue(NumericUtilities.IsPrime(31));
            Assert.IsTrue(NumericUtilities.IsPrime(17));
            Assert.IsFalse(NumericUtilities.IsPrime(4));
            Assert.IsFalse(NumericUtilities.IsPrime(1));
            Assert.IsFalse(NumericUtilities.IsPrime(55));
        }

        [TestMethod]
        public void GreatestCommonDivisor()
        {
            Assert.AreEqual(NumericUtilities.GreatestCommonDivisor(2, 3), 1);
            Assert.AreEqual(NumericUtilities.GreatestCommonDivisor(12, 8), 4);
            Assert.AreEqual(NumericUtilities.GreatestCommonDivisor(165, 15), 15);
            Assert.AreEqual(NumericUtilities.GreatestCommonDivisor(-8, 44), 4);
            Assert.AreEqual(NumericUtilities.GreatestCommonDivisor(41, 43), 1);
        }

        [TestMethod]
        public void LeastCommonMultiple()
        {
            Assert.AreEqual(NumericUtilities.LeastCommonMultiple(6, 4), 12u);
            Assert.AreEqual(NumericUtilities.LeastCommonMultiple(1, 5), 5u);
            Assert.AreEqual(NumericUtilities.LeastCommonMultiple(11, 25), 275u);
        }

        [TestMethod]
        public void EratosthenesSieveEmpty()
        {
            Assert.AreEqual(NumericUtilities.EratosthenesSieve(0).Count, 0);
        }

        [TestMethod]
        public void EratosthenesSieve()
        {
            var result = NumericUtilities.EratosthenesSieve(30);
            var expected = new List<uint> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void SolveModulo()
        {
            Assert.AreEqual(NumericUtilities.SolveModulo(2, 4, 5), 1u);
            Assert.AreEqual(NumericUtilities.SolveModulo(142, 41, 96), 64u);
            Assert.AreEqual(NumericUtilities.SolveModulo(2, 128, 41), 10u);
        }
    }
}
