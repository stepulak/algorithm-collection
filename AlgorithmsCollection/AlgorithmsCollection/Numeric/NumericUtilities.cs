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
                if (typeof(T) != typeof(double) && typeof(T) != typeof(float))
                {
                    throw new Exception("Invalid template parameter, must be double or float");
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

        public static bool DoubleCompare(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < Epsillon;
        }
        
        public static bool IsPrime(uint a)
        {
            if (a == 0 || a == 1)
            {
                return false;
            }
            // Check 6k+1 primes
            if (a != 2 && a != 3 && (a % 2 == 0 || a % 3 == 0))
            {
                return false;
            }
            var limit = (int)Math.Ceiling(Math.Sqrt(a));
            for (uint i = 5u; i <= limit; i+=2)
            {
                if (a % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        // GCD is also defined on negative numbers aswell
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

        public static uint LeastCommonMultiple(uint a, uint b) => a * b / (uint) GreatestCommonDivisor((int) a, (int) b);

        // Iterative non-lazy implementation
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

        public static uint SolveModulo(uint value, uint exponent, uint modulo)
        {
            var result = value % modulo;
            for (int exp = 1; exp < exponent; exp++)
            {
                result = (result * value) % modulo;
            }
            return result;
        }

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

        public static IEqualityComparer<T> FloatNumberEqualityComparer<T>()
        {
            return new FloatNumberEqualityCmp<T>();
        }
    }
}
