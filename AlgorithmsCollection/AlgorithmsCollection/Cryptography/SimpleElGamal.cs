using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// ElGamal decryption/encryption algorithm.
    /// </summary>
    public class SimpleElGamal
    {
        /// <summary>
        /// "Private" key computed for both sender and recipient.
        /// </summary>
        public uint Key { get; }

        /// <summary>
        /// Modulo used for key computation.
        /// </summary>
        public uint Modulo { get; }

        /// <summary>
        /// Create instance of SimpleElGamal class and count private key.
        /// Base and modulo should be large enough to correctly encrypt and decrypt given messages.
        /// </summary>
        /// <param name="exponentAlice">Exponent of Alice</param>
        /// <param name="exponentBob">Exponent of Bob</param>
        /// <param name="base">Shared base value. Must be larger than modulo.</param>
        /// <param name="modulo">Shared modulo. Must be less than base.</param>
        public SimpleElGamal(ushort exponentAlice, ushort exponentBob, ushort @base, ushort modulo)
        {
            if (@base >= modulo)
            {
                throw new ArgumentException("Base shouldn't be larger than modulo");
            }
            Modulo = modulo;
            var halfKeyAlice = NumericUtilities.SolveModulo(@base, exponentAlice, modulo);
            var halfKeyBob = NumericUtilities.SolveModulo(@base, exponentBob, modulo);
            // Discrete logarithm problem.
            // In real situation Alice wouldn't know about Bob's exponent and vice versa.
            // They change only "half parts of their own keys".
            var aliceKey = NumericUtilities.SolveModulo(halfKeyBob, exponentAlice, modulo);
            var bobKey = NumericUtilities.SolveModulo(halfKeyAlice, exponentBob, modulo);
            if (aliceKey != bobKey)
            {
                throw new Exception("Alice's and Bob's keys are not equal!");
            }
            Key = aliceKey;
        }
        
        /// <summary>
        /// Encrypt value.
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <returns>Encrypted value</returns>
        public uint Encrypt(uint value) => (Key * value) % Modulo;

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
        public uint Decrypt(uint value)
        {
            // Bruteforce congruency solution
            for (uint message = 0; message < Modulo; message++)
            {
                if ((message * Key) % Modulo == value % Modulo)
                {
                    return message;
                }
            }
            throw new Exception($"Unable to decrypt value {value}");
        }

        /// <summary>
        /// Decrypt list of values.
        /// </summary>
        /// <param name="message">List of values to decrypt</param>
        /// <returns>Decrypted list of values</returns>
        public List<uint> Decrypt(List<uint> message) => message.Select(Decrypt).ToList();
    }
}
