using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Possible results of Gauss elimination.
    /// </summary>
    public enum GaussEliminationResultType
    {
        ValidResult,
        NoValidResult,
        InfinityResults,
    };

    /// <summary>
    /// Templated rectangular matrix data type.
    /// </summary>
    /// <typeparam name="T">Type of value in matrix</typeparam>
    public class Matrix<T> : IEnumerable<T>
        where T : struct
    {
        private T[,] matrix;

        /// <summary>
        /// Number of rows in matrix (Y).
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Number of columns in matrix (X).
        /// </summary>
        public int Columns { get; }

        /// <summary>
        /// Create matrix with given number of rows and columns.
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        public Matrix(int rows, int columns)
        {
            if (rows <= 0)
            {
                throw new ArgumentException("Rows must be positive");
            }
            if (columns <= 0)
            {
                throw new ArgumentException("Columns must be positive");
            }
            Rows = rows;
            Columns = columns;
            matrix = new T[rows, columns];
        }

        /// <summary>
        /// Create matrix from given values. Number of rows and columns is inherited from values dimensions.
        /// </summary>
        /// <param name="values">Array of values</param>
        public Matrix(T[,] values) : this(values.GetLength(0), values.GetLength(1))
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    matrix[i, j] = values[i, j];
                }
            }
        }

        /// <summary>
        /// Create matrix from other matrix (deep copy).
        /// </summary>
        /// <param name="from">Original matrix</param>
        public Matrix(Matrix<T> from) : this(from.matrix)
        {
        }

        /// <summary>
        /// Return matrix as an array.
        /// </summary>
        /// <returns>Matrix as an array</returns>
        public T[,] GetMatrix() => matrix;

        /// <summary>
        /// Return transposed matrix.
        /// </summary>
        /// <returns>Transposed matrix</returns>
        public Matrix<T> Transpose()
        {
            var result = new Matrix<T>(Columns, Rows);
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    result[column, row] = this[row, column];
                }
            }
            return result;
        }
        
        /// <summary>
        /// Copy list of values into specific row in matrix.
        /// </summary>
        /// <param name="rowIndex">Row index in matrix</param>
        /// <param name="values">List of values</param>
        public void SetRow(int rowIndex, List<T> values)
        {
            if (values.Count != Columns)
            {
                throw new ArgumentException("Number of given values does not match matrix size");
            }
            for (int column = 0; column < Columns; column++)
            {
                this[rowIndex, column] = values[column];
            }
        }

        /// <summary>
        /// Copy list of values into specific column in matrix.
        /// </summary>
        /// <param name="columnIndex">Column index in matrix</param>
        /// <param name="values">List of values</param>
        public void SetColumn(int columnIndex, List<T> values)
        {
            if (values.Count != Rows)
            {
                throw new ArgumentException("Number of given values does not match matrix size");
            }
            for (int row = 0; row < Rows; row++)
            {
                this[row, columnIndex] = values[row];
            }
        }

        /// <summary>
        /// Get row at given index.
        /// </summary>
        /// <param name="rowIndex">Row index in matrix</param>
        /// <returns>Row at given index</returns>
        public IEnumerable<T> GetRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= Rows)
            {
                throw new ArgumentOutOfRangeException("Invalid row index");
            }
            for (int column = 0; column < Columns; column++)
            {
                yield return this[rowIndex, column];
            }
        }

        /// <summary>
        /// Get column at given index.
        /// </summary>
        /// <param name="columnIndex">Column index in matrix</param>
        /// <returns>Column at given index</returns>
        public IEnumerable<T> GetColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= Columns)
            {
                throw new ArgumentOutOfRangeException("Invalid column index");
            }
            for (int row = 0; row < Rows; row++)
            {
                yield return this[row, columnIndex];
            }
        }

        /// <summary>
        /// Get value at given position.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <returns>Value at give position</returns>
        public T this[int row, int column]
        {
            get
            {
                // disable bound-checking for better performance
                TestIndexPositionThrow(row, column);
                return matrix[row, column];
            }
            set
            {
                // disable bound-checking for better performance
                TestIndexPositionThrow(row, column);
                matrix[row, column] = value;
            }
        }

        /// <summary>
        /// Perform gauss elimination. Each row in matrix is an equation.
        /// Each column is a list of coefficients for one variable.
        /// </summary>
        /// <see cref="https://en.wikipedia.org/wiki/Gaussian_elimination"/>
        /// <param name="result">List of results</param>
        /// <returns>Gauss elimination result type</returns>
        public GaussEliminationResultType GaussElimination(out List<double> result)
        {
            if (Rows + 1 != Columns || Rows <= 1)
            {
                throw new InvalidOperationException($"Unable to perform gauss elimination on matrix with size {Rows}x{Columns}");
            }
            GaussEliminationForward();
            var resultType = CheckGaussEliminationResultType();
            if (resultType == GaussEliminationResultType.ValidResult)
            {
                GaussEliminationBackward(out result);
            }
            else
            {
                result = null;
            }
            return resultType;
        }

        /// <summary>
        /// Count sum of two matrices with same proportions.
        /// </summary>
        /// <param name="mat1">Matrix A</param>
        /// <param name="mat2">Matrix B</param>
        /// <returns>A+B</returns>
        public static Matrix<T> operator +(Matrix<T> mat1, Matrix<T> mat2) => MatricesTransform(mat1, mat2, (value1, value2) => (dynamic)value1 + value2);

        /// <summary>
        /// Count subtract of two matrices with same proportions.
        /// </summary>
        /// <param name="mat1">Matrix A</param>
        /// <param name="mat2">Matrix B</param>
        /// <returns>A-B</returns>
        public static Matrix<T> operator -(Matrix<T> mat1, Matrix<T> mat2) => MatricesTransform(mat1, mat2, (value1, value2) => (dynamic)value1 - value2);

        /// <summary>
        /// Multiply matrix with value.
        /// </summary>
        /// <param name="matrix">Matrix</param>
        /// <param name="value">Value</param>
        /// <returns>A*k</returns>
        public static Matrix<T> operator *(Matrix<T> matrix, T value) => MatrixTransform(matrix, (matrixValue) => (dynamic)value * matrixValue);

        /// <summary>
        /// Multiply matrix with value.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="matrix">Matrix</param>
        /// <returns>k*A</returns>
        public static Matrix<T> operator *(T value, Matrix<T> matrix) => matrix * value;

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        /// <param name="mat1">Matrix A</param>
        /// <param name="mat2">Matrix B</param>
        /// <returns>A*B</returns>
        public static Matrix<T> operator *(Matrix<T> mat1, Matrix<T> mat2)
        {
            if (mat1.Columns != mat2.Rows)
            {
                throw new ArgumentException("Unable to apply cross product, given matrix have unsuitable size.");
            }
            var result = new Matrix<T>(mat1.Rows, mat2.Columns);
            for (int row = 0; row < mat1.Rows; row++)
            {
                for (int column = 0; column < mat2.Columns; column++)
                {
                    dynamic sum = 0;
                    for (int i = 0; i < mat1.Columns; i++)
                    {
                        sum += (dynamic)mat1[row, i] * mat2[i, column];
                    }
                    result[row, column] = sum;
                }
            }
            return result;
        }

        public IEnumerator<T> GetEnumerator() => Enumerable.Cast<T>(matrix).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj is Matrix<T> matrix)
            {
                var cmp = Comparer<T>.Default;
                return this
                    .Zip(matrix, (a, b) => cmp.Compare(a, b) == 0)
                    .Count(a => a) == Rows * Columns;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 0;
            foreach (var value in this)
            {
                result ^= value.GetHashCode();
            }
            return result;
        }

        public override string ToString()
        {
            var ss = new StringBuilder();
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    ss.Append(this[row, column].ToString());
                    if (column < Columns - 1)
                    {
                        ss.Append(' ');
                    }
                }
                ss.Append('\n');
            }
            return ss.ToString();
        }
        
        protected void TestIndexPositionThrow(int row, int column)
        {
            if (row < 0 || row >= Rows)
            {
                throw new ArgumentOutOfRangeException("Invalid row index");
            }
            if (column < 0 || column >= Columns)
            {
                throw new ArgumentOutOfRangeException("Invalid column index");
            }
        }

        protected static Matrix<T> MatrixTransform(Matrix<T> matrix, Func<T, T> transformFunc)
        {
            var result = new Matrix<T>(matrix.Rows, matrix.Columns);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int column = 0; column < matrix.Columns; column++)
                {
                    result[row, column] = transformFunc(matrix[row, column]);
                }
            }
            return result;
        }

        protected static void TestMatricesSameSizeThrow(Matrix<T> mat1, Matrix<T> mat2)
        {
            if (mat1.Rows != mat2.Rows || mat1.Columns != mat2.Columns)
            {
                throw new ArgumentException("Matrices sizes are not equal!");
            }
        }

        protected static Matrix<T> MatricesTransform(Matrix<T> mat1, Matrix<T> mat2, Func<T, T, T> transformFunc)
        {
            TestMatricesSameSizeThrow(mat1, mat2);
            var result = new Matrix<T>(mat1.Rows, mat1.Columns);
            for (int row = 0; row < mat1.Rows; row++)
            {
                for (int column = 0; column < mat1.Columns; column++)
                {
                    result[row, column] = transformFunc(mat1[row, column], mat2[row, column]);
                }
            }
            return result;
        }
        
        private void GaussEliminationForward()
        {
            for (int row = 0; row < Rows; row++) 
            {
                for (int elimRow = row + 1; elimRow < Rows; elimRow++)
                {
                    var ratio = this[elimRow, row] / (dynamic)this[row, row];
                    for (int column = 0; column < Columns; column++)
                    {
                        this[elimRow, column] -= ratio * this[row, column];
                    }
                }
            }
        }

        private void GaussEliminationBackward(out List<double> results)
        {
            results = new List<double>(new double[Rows])
            {
                [Rows - 1] = this[Rows - 1, Rows] / (dynamic)this[Rows - 1, Rows - 1]
            };
            for (int row = Rows - 2; row >= 0; row--)
            {
                var sum = 0.0;
                for (int j = row; j < Rows; j++)
                {
                    sum += (dynamic)this[row, j] * results[j];
                }
                results[row] = ((dynamic)this[row, Rows] - sum) / this[row, row];
            }
        }

        private GaussEliminationResultType CheckGaussEliminationResultType()
        {
            for (int row = 0; row < Rows; row++)
            {
                // Check single row for all zeros
                bool allZeros = true;
                for (int column = 0; column < Columns - 1; column++)
                {
                    var matrixValue = (dynamic)this[row, column];
                    if (!NumericUtilities.DoubleCompare((double)matrixValue, 0.0))
                    {
                        allZeros = false;
                        break;
                    }
                }
                if (allZeros)
                {
                    // Two possible results: Infinity results, No valid result
                    var equationResult = (dynamic)this[row, Columns - 1];
                    return NumericUtilities.DoubleCompare((double)equationResult, 0.0)
                        ? GaussEliminationResultType.InfinityResults : GaussEliminationResultType.NoValidResult;
                }
            }
            return GaussEliminationResultType.ValidResult;
        }
    }
}
