using System;
using System.Linq;
using System.Collections.Generic;
using AlgorithmsCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class RsaTests
    {
        [TestMethod]
        public void RSAConstructorNonPrimes()
        {
            Assert.ThrowsException<ArgumentException>(() => { new SimpleRSA(8, 13); });
            Assert.ThrowsException<ArgumentException>(() => { new SimpleRSA(7, 14); });
        }

        [TestMethod]
        public void RSASimpleEncryptDecrypt()
        {
            var rsa = new SimpleRSA(11, 13);
            Assert.AreEqual(rsa.Modulo, 143u);
            Assert.AreEqual(rsa.PublicKey, 7u);
            Assert.AreEqual(rsa.PrivateKey, 103u);
            Assert.AreEqual(rsa.Encrypt(5), 47u);
            Assert.AreEqual(rsa.Decrypt(47), 5u);
        }

        [TestMethod]
        public void RSALongMessageEncryption()
        {
            var rsa = new SimpleRSA(3, 11);
            Assert.AreEqual(rsa.Modulo, 33u);
            Assert.AreEqual(rsa.PublicKey, 3u);
            Assert.AreEqual(rsa.PrivateKey, 7u);
            var message = new List<uint> { 1, 5, 31, 14, 8 };
            var expected = new List<uint>();
            foreach (var value in message)
            {
                expected.Add(NumericUtilities.SolveModulo(value, rsa.PublicKey, rsa.Modulo));
            }
            var encrypted = rsa.Encrypt(message);
            Assert.IsTrue(expected.SequenceEqual(encrypted));
            Assert.IsTrue(message.SequenceEqual(rsa.Decrypt(encrypted)));
        }
    }
}
