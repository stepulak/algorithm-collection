using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class SimpleRSA
    {
        public uint Modulo { get; private set; }
        public uint PublicKey { get; private set; }
        public uint PrivateKey { get; private set; } // Make private key public for debug purposes

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
            CountPublicKey(euler);
            CountPrivateKey(euler);
        }

        // Note that value should be less than RSA's Modulo,
        // otherwise inverse decryption will return different message...
        public uint Encrypt(uint value) => NumericUtilities.SolveModulo(value, PublicKey, Modulo);
        public List<uint> Encrypt(List<uint> message) => message.Select(Encrypt).ToList();

        public uint Decrypt(uint value) => NumericUtilities.SolveModulo(value, PrivateKey, Modulo);
        public List<uint> Decrypt(List<uint> message) => message.Select(Decrypt).ToList();
        
        private void CountPublicKey(uint euler)
        {
            bool found = false;
            for (uint e = 2; e < euler; e++)
            {
                if (NumericUtilities.GreatestCommonDivisor((int)euler, (int)e) == 1)
                {
                    PublicKey = e;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new Exception("Unable to count public key");
            }
        }

        private void CountPrivateKey(uint euler)
        {
            bool found = false;
            for (uint d = 1; d < euler; d++)
            {
                if ((d * PublicKey) % euler == 1)
                {
                    PrivateKey = d;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new Exception("Unable to count private key");
            }
        }
    }
}
