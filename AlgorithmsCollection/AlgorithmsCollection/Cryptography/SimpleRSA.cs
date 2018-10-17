using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// RSA encryption/decryption algorithm.
    /// </summary>
    public class SimpleRSA
    {
        /// <summary>
        /// Shared modulo for encryption/decryption.
        /// </summary>
        public uint Modulo { get; }

        /// <summary>
        /// Public key.
        /// </summary>
        public uint PublicKey { get; }

        /// <summary>
        /// Private key (public for debug and information purposes).
        /// </summary>
        public uint PrivateKey { get; }

        /// <summary>
        /// Create instance of SimpleRSA class. 
        /// P and Q parameters must be prime numbers and P != Q.
        /// Their product should be large enough to correctly compute encrypted/decrypted messages.
        /// </summary>
        /// <param name="p">Prime number P</param>
        /// <param name="q">Primer number Q</param>
        public SimpleRSA(ushort p, ushort q)
        {
            if (!NumericUtilities.IsPrime(p))
            {
                throw new ArgumentException("P is not prime");
            }
            if (!NumericUtilities.IsPrime(q))
            {
                throw new ArgumentException("Q is not prime");
            }
            var euler = (uint)((p - 1) * (q - 1));
            Modulo = (uint)p * q;
            PublicKey = CountPublicKey(euler);
            PrivateKey = CountPrivateKey(euler, PublicKey);
        }
        
        /// <summary>
        /// Encrypt value.
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <returns>Encrypted value</returns>
        public uint Encrypt(uint value) => NumericUtilities.SolveModulo(value, PublicKey, Modulo);

        /// <summary>
        /// Encrypt list of values.
        /// </summary>
        /// <param name="message">List of values to encrypt</param>
        /// <returns>Encrypted list of values</returns>
        public List<uint> Encrypt(List<uint> message) => message.Select(Encrypt).ToList();

        /// <summary>
        /// Decrypt value.
        /// </summary>
        /// <param name="value">Value to decrypt</param>
        /// <returns>Decrypted value</returns>
        public uint Decrypt(uint value) => NumericUtilities.SolveModulo(value, PrivateKey, Modulo);

        /// <summary>
        /// Decrypt list of values.
        /// </summary>
        /// <param name="message">List of values to decrypt</param>
        /// <returns>Decrypted list of values</returns>
        public List<uint> Decrypt(List<uint> message) => message.Select(Decrypt).ToList();
        
        private static uint CountPublicKey(uint euler)
        {
            for (uint e = 2; e < euler; e++)
            {
                if (NumericUtilities.GreatestCommonDivisor((int)euler, (int)e) == 1)
                {
                    return e;
                }
            }
            throw new Exception("Unable to count public key");
        }

        private static uint CountPrivateKey(uint euler, uint publicKey)
        {
            for (uint d = 1; d < euler; d++)
            {
                if (d * publicKey % euler == 1)
                {
                    return d;
                }
            }
            throw new Exception("Unable to count private key");
        }
    }
}
