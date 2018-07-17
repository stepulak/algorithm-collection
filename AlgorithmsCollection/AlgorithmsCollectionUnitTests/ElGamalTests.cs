using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class ElGamalTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ElGamalBaseLargerThanModulo()
        {
            new SimpleElGamal(2, 3, 100, 50);
        }

        [TestMethod]
        public void ElGamalEncryptSimpleEncryptDecrypt()
        {
            var elgamal = new SimpleElGamal(8, 13, 5, 89);
            Assert.AreEqual(elgamal.Key, 16u);
            Assert.AreEqual(elgamal.Encrypt(35), 26u);
            Assert.AreEqual(elgamal.Decrypt(26), 35u);
        }

        [TestMethod]
        public void ElGamalLongMessageEncryption()
        {
            var elgamal = new SimpleElGamal(12, 17, 3, 74);
            var message = new List<uint> { 13, 7, 22, 43, 56, 64, 8, 1, 12 };
            var expected = new List<uint>();
            foreach (var value in message)
            {
                expected.Add(NumericUtilities.SolveModulo(value * elgamal.Key, 1, elgamal.Modulo));
            }
            var encrypted = elgamal.Encrypt(message);
            Assert.IsTrue(expected.SequenceEqual(encrypted));
            Assert.IsTrue(message.SequenceEqual(elgamal.Decrypt(encrypted)));
        }
    }
}
