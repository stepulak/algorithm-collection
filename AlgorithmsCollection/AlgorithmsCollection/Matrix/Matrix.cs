using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public enum GaussEliminationResultType
    {
        ValidResult,
        NoValidResult,
        InfinityResults,
    };

    public class Matrix<T> : IEnumerable<T>
        where T : struct
    {
        private T[,] matrix;

        public int Rows { get; }
        public int Columns { get; }

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

        public Matrix(Matrix<T> from) : this(from.matrix)
        {
        }

        public T[,] GetMatrix() => matrix;

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

        public static Matrix<T> operator +(Matrix<T> mat1, Matrix<T> mat2) => MatricesTransform(mat1, mat2, (value1, value2) => (dynamic)value1 + value2);
        public static Matrix<T> operator -(Matrix<T> mat1, Matrix<T> mat2) => MatricesTransform(mat1, mat2, (value1, value2) => (dynamic)value1 - value2);
        public static Matrix<T> operator *(Matrix<T> matrix, T value) => MatrixTransform(matrix, (matrixValue) => (dynamic)value * matrixValue);
        public static Matrix<T> operator *(T value, Matrix<T> matrix) => matrix * value;

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
            if (obj is Matrix<T>)
            {
                var cmp = Comparer<T>.Default;
                return this
                    .Zip(obj as Matrix<T>, (a, b) => cmp.Compare(a, b) == 0)
                    .Count(a => a) == Rows * Columns;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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
