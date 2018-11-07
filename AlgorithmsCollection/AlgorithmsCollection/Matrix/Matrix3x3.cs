using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Templated 3x3 (square) matrix.
    /// </summary>
    /// <typeparam name="T">Type of value in matrix</typeparam>
    public class Matrix3x3<T> : MatrixNxN<T>
        where T : struct
    {
        /// <summary>
        /// Create empty matrix.
        /// </summary>
        public Matrix3x3() : base(3)
        {
        }

        /// <summary>
        /// Create matrix from given values. 
        /// </summary>
        /// <param name="values">Array of values</param>
        public Matrix3x3(T[,] values) : base(CheckValuesDimensions3x3(values))
        {
        }

        /// <summary>
        /// Create matrix from other matrix (deep copy).
        /// </summary>
        /// <param name="from">Original matrix</param>
        public Matrix3x3(Matrix<T> from) : this(from.GetMatrix())
        {
        }

        /// <summary>
        /// Count determinant using fast Sarrus rule.
        /// </summary>
        /// <returns>Determinant of matrix</returns>
        public T DeterminantSarrusRule()
        {
            var diagonal1 = (dynamic)this[0, 0] * this[1, 1] * this[2, 2];
            var diagonal2 = (dynamic)this[0, 1] * this[1, 2] * this[2, 0];
            var diagonal3 = (dynamic)this[0, 2] * this[1, 0] * this[2, 1];
            var invDiagonal1 = (dynamic)this[0, 2] * this[1, 1] * this[2, 0];
            var invDiagonal2 = (dynamic)this[0, 0] * this[1, 2] * this[2, 1];
            var invDiagonal3 = (dynamic)this[0, 1] * this[1, 0] * this[2, 2];
            return diagonal1 + diagonal2 + diagonal3 - invDiagonal1 - invDiagonal2 - invDiagonal3;
        }

        private static T[,] CheckValuesDimensions3x3(T[,] values)
        {
            if (values.GetLength(0) != values.GetLength(1) && values.GetLength(0) != 3)
            {
                throw new ArgumentException("Invalid matrix proportions. Must be 3x3.");
            }
            return values;
        }
    }
}
