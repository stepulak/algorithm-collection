using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class SimpleElGamal
    {
        public uint Key { get; private set; }
        public uint Modulo { get; }

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

        // Note that value should be less than ElGamal's Modulo,
        // otherwise inverse decryption will return different message...
        public uint Encrypt(uint value) => (Key * value) % Modulo;
        public List<uint> Encrypt(List<uint> message) => message.Select(Encrypt).ToList();

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

        public List<uint> Decrypt(List<uint> message) => message.Select(Decrypt).ToList();
    }
}
