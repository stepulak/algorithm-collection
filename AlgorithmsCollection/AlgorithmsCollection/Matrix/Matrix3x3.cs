using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class Matrix3x3<T> : MatrixNxN<T>
        where T : struct
    {
        public Matrix3x3(T[,] values) : base(CheckValuesDimensions3x3(values))
        {
        }

        public Matrix3x3(Matrix<T> from) : this(from.GetMatrix())
        {
        }

        public T DeterminantSarrusRule()
        {
            var diagonal1 = (dynamic)this[0, 0] * this[1, 1] * this[2, 2];
            var diagonal2 = (dynamic)this[0, 1] * this[1, 2] * this[2, 0];
            var diagonal3 = (dynamic)this[0, 2] * this[1, 0] * this[2, 1];
            var invDiagonal1 = (dynamic)this[0, 2] * this[1, 1] * this[2, 0];
            var invDiagonal2 = (dynamic)this[0, 0] * this[1, 2] * this[2, 1];
            var invDiagonal3 = (dynamic)this[0, 1] * this[1, 0] * this[2, 2];
            return diagonal1 * diagonal2 * diagonal3 - invDiagonal1 * invDiagonal2 * invDiagonal3;
        }

        private static T[,] CheckValuesDimensions3x3(T[,] values)
        {
            if (values.Length != values.GetLength(1) && values.Length != 3)
            {
                throw new ArgumentException("Invalid matrix proportions. Must be 3x3.");
            }
            return values;
        }
    }
}
