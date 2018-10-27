using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Templated square matrix.
    /// </summary>
    /// <typeparam name="T">Type of value in matrix</typeparam>
    public class MatrixNxN<T> : Matrix<T>
        where T : struct
    {
        /// <summary>
        /// Check whether matrix is regular (it means that have nonzero determinant).
        /// </summary>
        public bool IsRegular => (dynamic)Determinant() != 0;

        /// <summary>
        /// Create square empty matrix with given size.
        /// </summary>
        /// <param name="size">Size of matrix</param>
        public MatrixNxN(int size) : base(size, size)
        {
        }

        /// <summary>
        /// Create square matrix and fill it's diagonal (from top-left to bottom-right corner) with given value.
        /// </summary>
        /// <param name="size">Size of matrix</param>
        /// <param name="diagonalValue">Diagonal value</param>
        public MatrixNxN(int size, T diagonalValue) : base(size, size)
        {
            for (int i = 0; i < Rows; i++)
            {
                this[i, i] = diagonalValue;
            }
        }

        /// <summary>
        /// Create matrix from given values. Size of matrix is inherited from array size.
        /// </summary>
        /// <param name="values">Array of values</param>
        public MatrixNxN(T[,] values) : base(CheckValuesDimensionsNxN(values))
        {
        }

        /// <summary>
        /// Create matrix from other matrix (deep copy).
        /// </summary>
        /// <param name="from">Original matrix</param>
        public MatrixNxN(Matrix<T> from) : this(from.GetMatrix())
        {
        }
        
        /// <summary>
        /// Count determinant (using La Place method).
        /// </summary>
        /// <returns>Determinant of this matrix</returns>
        public T Determinant() => DeterminantLaPlace(this);

        /// <summary>
        /// Create inverse of matrix.
        /// </summary>
        /// <returns>Matrix inverse</returns>
        public MatrixNxN<T> Inverse()
        {
            var cofactorMatrix = new MatrixNxN<T>(Rows);
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    var submatrix = CreateSubmatrix(this, row, column);
                    cofactorMatrix[row, column] = (dynamic)submatrix.Determinant() * (int)Math.Pow(-1, row + column);
                }
            }
            return new MatrixNxN<T>(cofactorMatrix.Transpose() * ((dynamic)1.0 / Determinant()));
        }

        /// <summary>
        /// Solve system of equation using Cramer's rule. 
        /// The equation system must have just one valid solution.
        /// </summary>
        /// <see cref="https://en.wikipedia.org/wiki/Cramer%27s_rule"/>
        /// <param name="equationRightSideResults">Right side equation ordered results</param>
        /// <returns>List of results</returns>
        public List<double> CramersRule(List<T> equationRightSideResults)
        {
            if (equationRightSideResults.Count != Rows)
            {
                throw new ArgumentException("Right side equation results do not match with matrix");
            }
            var determinantMain = Determinant();
            if ((int)(dynamic)determinantMain == 0)
            {
                throw new InvalidOperationException("Unable to perform Cramer's rule on system of equations with more than one possible solution");
            }
            var result = new List<double>();
            for (int column = 0; column < Columns; column++)
            {
                var matrix = CreateMatrixForCramersRule(this, column, equationRightSideResults);
                result.Add((dynamic)matrix.Determinant() / (double)(dynamic)determinantMain);
            }
            return result;
        }

        /// <summary>
        /// Divide two matrices.
        /// </summary>
        /// <param name="mat1">Matrix A</param>
        /// <param name="mat2">Matrix B</param>
        /// <returns></returns>
        public static MatrixNxN<T> operator /(MatrixNxN<T> mat1, MatrixNxN<T> mat2)
        {
            return new MatrixNxN<T>(mat1 * mat2.Inverse());
        }

        private static T[,] CheckValuesDimensionsNxN(T[,] values)
        {
            if (values.GetLength(0) != values.GetLength(1))
            {
                throw new ArgumentException("Invalid matrix proportions. Must be NxN.");
            }
            return values;
        }
        
        private static T DeterminantLaPlace(MatrixNxN<T> matrix)
        {
            if (matrix.Rows == 1) // matrix.Columns == 1
            {
                return matrix[0, 0];
            }
            if (matrix.Rows == 2) // matrix.Columns == 2
            {
                var ac = (dynamic)matrix[0, 0] * matrix[1, 1];
                var bd = (dynamic)matrix[0, 1] * matrix[1, 0];
                return ac - bd;
            }
            dynamic determinant = 0;
            for (int i = 0; i < matrix.Columns; i++)
            {
                var submatrix = CreateSubmatrix(matrix, 0, i);
                determinant += (int)Math.Pow(-1, i) * (dynamic)matrix[0, i] * submatrix.Determinant();
            }
            return determinant;
        }

        private static MatrixNxN<T> CreateSubmatrix(MatrixNxN<T> matrix, int skipRow, int skipColumn)
        {
            var submatrix = new MatrixNxN<T>(matrix.Rows - 1);
            var column = 0;
            for (int k = 0; k < matrix.Columns; k++)
            {
                if (k != skipColumn)
                {
                    var columnValues = matrix.GetColumn(k).ToList();
                    columnValues.RemoveAt(skipRow);
                    submatrix.SetColumn(column, columnValues);
                    column++;
                }
            }
            return submatrix;
        }

        private static MatrixNxN<T> CreateMatrixForCramersRule(MatrixNxN<T> matrixFrom, int skipColumn, List<T> equationRightSideResults)
        {
            var matrix = new MatrixNxN<T>(matrixFrom);
            matrix.SetColumn(skipColumn, equationRightSideResults);
            return matrix;
        }
    }
}
