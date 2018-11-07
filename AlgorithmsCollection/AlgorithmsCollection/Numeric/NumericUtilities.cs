using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public static class NumericUtilities
    {
        private class FloatNumberEqualityCmp<T> : IEqualityComparer<T>
        {
            private Comparer<T> comparer = FloatNumberComparer<T>();

            public FloatNumberEqualityCmp()
            {
                var type = typeof(T);
                if (type != typeof(double) && type != typeof(float))
                {
                    throw new Exception("Invalid template parameter, must be double or float.");
                }
            }

            public bool Equals(T x, T y)
            {
                return comparer.Compare(x, y) == 0;
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        private const double Epsillon = 0.00001;

        /// <summary>
        /// Compare two double values for their equality.
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        /// <returns>True if two values are (approximately) equal, false otherwise.</returns>
        public static bool DoubleCompare(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < Epsillon;
        }

        /// <summary>
        /// Check whether given number is a prime number.
        /// </summary>
        /// <param name="num">Number to check</param>
        /// <returns>True if given number is prime number, false otherwise</returns>
        public static bool IsPrime(uint num)
        {
            if (num == 0 || num == 1)
            {
                return false;
            }
            // Check 6k+1 primes
            if (num != 2 && num != 3 && (num % 2 == 0 || num % 3 == 0))
            {
                return false;
            }
            var limit = (int)Math.Ceiling(Math.Sqrt(num));
            for (uint i = 5u; i <= limit; i+=2)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Count GCD of two numbers. Working well with negative values aswell.
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>GCD of two values</returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            if (a == 0)
            {
                return 0; // Skip
            }
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        /// <summary>
        /// Count LCM of two numbers. Only for unsigned numbers.
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>LCM of two values</returns>
        public static uint LeastCommonMultiple(uint a, uint b) => a * b / (uint) GreatestCommonDivisor((int) a, (int) b);
        
        /// <summary>
        /// Count Eratosthene sieve.
        /// </summary>
        /// <param name="maxValue">Maximum value of prime number</param>
        /// <returns>List of prime numbers less than maxValue</returns>
        public static List<uint> EratosthenesSieve(uint maxValue)
        {
            var numbers = new List<uint>();
            for(uint i = 2u; i < maxValue; i++)
            {
                numbers.Add(i);
            }
            for (int position = 0; position < numbers.Count; position++)
            {
                var divisorFilter = numbers[position];
                numbers.RemoveAll(value => value != divisorFilter && value % divisorFilter == 0);
            }
            return numbers;
        }

        /// <summary>
        /// Solve modulo of value with exponent (value^exponent % modulo).
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="exponent">Exponent of value</param>
        /// <param name="modulo">Modulo</param>
        /// <returns>Result of value^exponent % modulo expression</returns>
        public static uint SolveModulo(uint value, uint exponent, uint modulo)
        {
            var result = value % modulo;
            for (int exp = 1; exp < exponent; exp++)
            {
                result = (result * value) % modulo;
            }
            return result;
        }

        /// <summary>
        /// Create comparer for float numbers (float, double).
        /// </summary>
        /// <typeparam name="T">Type of float number (float, double)</typeparam>
        /// <returns>Comparer for float numbers</returns>
        public static Comparer<T> FloatNumberComparer<T>()
        {
            return Comparer<T>.Create(new Comparison<T>((a, b) =>
            {
                if (Math.Abs((dynamic)a - b) < Epsillon)
                {
                    return 0;
                }
                if ((dynamic)a - b < 0.0)
                {
                    return -1;
                }
                return 1;
            }));
        }

        /// <summary>
        /// Create equality comparer for float numbers (float, double).
        /// </summary>
        /// <typeparam name="T">Type of float number (float, double)</typeparam>
        /// <returns>Equality comparer for float numbers</returns>
        public static IEqualityComparer<T> FloatNumberEqualityComparer<T>() => new FloatNumberEqualityCmp<T>();
    }
}
