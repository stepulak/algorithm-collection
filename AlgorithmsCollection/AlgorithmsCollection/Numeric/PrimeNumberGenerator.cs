using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Class for generating consecutive prime numbers.
    /// </summary>
    public class PrimeNumberGenerator
    {
        /// <summary>
        /// Last generated prime number.
        /// </summary>
        public uint CurrentPrime { get; private set; } = 2;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PrimeNumberGenerator()
        {
        }

        /// <summary>
        /// Constructor with given starting prime number.
        /// </summary>
        /// <param name="startPrime">Starting prime number set to CurrentPrime property</param>
        public PrimeNumberGenerator(uint startPrime)
        {
            if (!NumericUtilities.IsPrime(startPrime))
            {
                throw new ArgumentException("Given start value is not a prime number!");
            }
            CurrentPrime = startPrime;
        }

        /// <summary>
        /// Generate next prime number and save it to CurrentPrime property.
        /// </summary>
        /// <returns>Next generated prime number</returns>
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
