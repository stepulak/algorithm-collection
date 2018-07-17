using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class PrimeNumberGenerator
    {
        public uint CurrentPrime { get; private set; } = 2;

        public PrimeNumberGenerator()
        {
        }

        public PrimeNumberGenerator(uint startPrime)
        {
            if (!NumericUtilities.IsPrime(startPrime))
            {
                throw new ArgumentException("Given start value is not a prime number!");
            }
            CurrentPrime = startPrime;
        }

        public uint GetNext()
        {
            if (CurrentPrime == 2)
            {
                // Optimalization
                return (CurrentPrime = 3);
            }
            do
            {
                CurrentPrime += 2; // Skip even numbers
            } while (!NumericUtilities.IsPrime(CurrentPrime));
            return CurrentPrime;
        }
    }
}
