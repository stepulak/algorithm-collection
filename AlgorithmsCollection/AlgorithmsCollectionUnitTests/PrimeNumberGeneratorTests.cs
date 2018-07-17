using System;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class PrimeNumberGeneratorTests
    {
        [TestMethod]
        public void PrimeNumberGeneratorEmptyConstructor()
        {
            var generator = new PrimeNumberGenerator();
            var expected = new List<uint> { 3, 5, 7, 11, 13 };
            Assert.AreEqual(generator.CurrentPrime, 2u);
            foreach (var prime in expected)
            {
                Assert.AreEqual(generator.GetNext(), prime);
                Assert.AreEqual(generator.CurrentPrime, prime);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PrimeNumberGeneratorDefaultNonPrime()
        {
            new PrimeNumberGenerator(6).GetNext();
        }

        [TestMethod]
        public void PrimeNumberGeneratorDefaultPrime()
        {
            var generator = new PrimeNumberGenerator();
            var expected = new List<uint> { 3, 5, 7, 11, 13 };
            Assert.AreEqual(generator.CurrentPrime, 2u);
            foreach (var prime in expected)
            {
                Assert.AreEqual(generator.GetNext(), prime);
                Assert.AreEqual(generator.CurrentPrime, prime);
            }
        }

        [TestMethod]
        public void PrimeNumberGeneratorChosenPrime()
        {
            var generator = new PrimeNumberGenerator(7);
            var expected = new List<uint> { 11, 13, 17, 19, 23 };
            Assert.AreEqual(generator.CurrentPrime, 7u);
            foreach (var prime in expected)
            {
                Assert.AreEqual(generator.GetNext(), prime);
                Assert.AreEqual(generator.CurrentPrime, prime);
            }
        }
    }
}
