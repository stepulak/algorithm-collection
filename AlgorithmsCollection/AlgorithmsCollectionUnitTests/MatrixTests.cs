using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixConstructorNonpositiveRows()
        {
            new Matrix<double>(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixConstructorNonpositiveColumns()
        {
            new Matrix<double>(1, -1);
        }

        [TestMethod]
        public void MatrixConstructor()
        {
            var matrix = new Matrix<double>(10, 5);
            Assert.AreEqual(matrix.Rows, 10);
            Assert.AreEqual(matrix.Columns, 5);
        }

        [TestMethod]
        public void MatrixConstructorFromValues()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            var matrix = new Matrix<int>(values);
            Assert.AreEqual(matrix.Rows, 3);
            Assert.AreEqual(matrix.Columns, 3);
            var matrix2 = new Matrix<int>(matrix);
            Assert.AreEqual(matrix2.Rows, 3);
            Assert.AreEqual(matrix2.Columns, 3);
            Assert.IsTrue(matrix.Equals(matrix2));
        }

        [TestMethod]
        public void MatrixTranspose()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            var transposed = matrix.Transpose();
            Assert.AreEqual(transposed.Rows, 3);
            Assert.AreEqual(transposed.Columns, 2);
            int[,] transposedValues = new int[,]
            {
                { 1, 4 },
                { 2, 5 },
                { 3, 6 }
            };
            Assert.IsTrue(transposed.Equals(new Matrix<int>(transposedValues)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MatrixGetRowIndexOutOfRange()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            matrix.GetRow(-1).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MatrixGetRowIndexOutOfRange2()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            matrix.GetRow(2).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MatrixGetColumnIndexOutOfRange()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            matrix.GetColumn(-1).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MatrixGetColumnIndexOutOfRange2()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            matrix.GetColumn(3).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MatrixIndexerInvalidRowIndex()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            var result = matrix[5, 0];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MatrixIndexerInvalidColumnIndex()
        {
            var values = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
            };
            var matrix = new Matrix<int>(values);
            var result = matrix[0, 5];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixSetRowDifferentLength()
        {
            var matrix = new Matrix<int>(2, 1);
            matrix.SetRow(1, new List<int> { 1, 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixSetColumnDifferentLength()
        {
            var matrix = new Matrix<int>(1, 2);
            matrix.SetColumn(1, new List<int> { 1, 1 });
        }

        [TestMethod]
        public void MatrixSetRow()
        {
            var matrix = new Matrix<int>(2, 3);
            var values = new List<int> { 1, 2, 3 };
            matrix.SetRow(0, values);
            Assert.IsTrue(matrix.GetRow(0).SequenceEqual(values));
        }

        [TestMethod]
        public void MatrixSetColumn()
        {
            var matrix = new Matrix<int>(3, 2);
            var values = new List<int> { 1, 2, 3 };
            matrix.SetColumn(0, values);
            Assert.IsTrue(matrix.GetColumn(0).SequenceEqual(values));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MatrixGaussEliminationInvalidMatrix()
        {
            var matrix = new Matrix<int>(3, 3).GaussElimination(out List<double> result);
        }

        [TestMethod]
        public void MatrixGaussEliminationValidResult()
        {
            var values = new double[,]
            {
                { 8, -1, -2, 0 },
                { -1, 7, -1, 10 },
                { -2, -1, 9, 23 }
            };
            var matrix = new Matrix<double>(values);
            var resultType = matrix.GaussElimination(out List<double> result);
            Assert.AreEqual(resultType, GaussEliminationResultType.ValidResult);
            var expectedResult = new List<double> { 1, 2, 3 };
            Assert.AreEqual(result
                .Zip(expectedResult, (r, e) => NumericUtilities.DoubleCompare(r, e))
                .Count(a => a), 3);
        }
        
        [TestMethod]
        public void MatrixGaussEliminationInfinityResults()
        {
            var values = new double[,]
            {
                { 1, -1, 2, -3 },
                { 4, 4, -2, 1 },
                { -2, 2, -4, 6 }
            };
            var matrix = new Matrix<double>(values);
            var resultType = matrix.GaussElimination(out List<double> result);
            Assert.AreEqual(resultType, GaussEliminationResultType.InfinityResults);
        }

        [TestMethod]
        public void MatrixGaussEliminationNoValidResult()
        {
            var values = new double[,]
            {
                { 2, -1, 1, 1 },
                { 3, 2, -4, 4 },
                { -6, 3, -3, 2 }
            };
            var matrix = new Matrix<double>(values);
            var resultType = matrix.GaussElimination(out List<double> result);
            Assert.AreEqual(resultType, GaussEliminationResultType.NoValidResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixAdditionDifferentSize()
        {
            var matrix1 = new Matrix<int>(3, 4);
            var matrix2 = new Matrix<int>(4, 3);
            var result = matrix1 + matrix2;
        }
        
        [TestMethod]
        public void MatrixAddition()
        {
            var values1 = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var values2 = new int[,]
            {
                { 5, 6 },
                { 7, 8 }
            };
            var expected = new int[,]
            {
                { 6, 8 },
                { 10, 12 }
            };
            var result = new Matrix<int>(values1) + new Matrix<int>(values2);
            Assert.IsTrue(result.Equals(new Matrix<int>(expected)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixSubtractionDifferentSize()
        {
            var matrix1 = new Matrix<int>(3, 4);
            var matrix2 = new Matrix<int>(4, 3);
            var result = matrix1 - matrix2;
        }

        [TestMethod]
        public void MatrixSubtraction()
        {
            var values1 = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var values2 = new int[,]
            {
                { 5, 6 },
                { 7, 8 }
            };
            var expected = new int[,]
            {
                { -4, -4 },
                { -4, -4 }
            };
            var result = new Matrix<int>(values1) - new Matrix<int>(values2);
            Assert.IsTrue(result.Equals(new Matrix<int>(expected)));
        }

        [TestMethod]
        public void MatrixMultiplicationByConstant()
        {
            var values = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var expected = new int[,]
            {
                { 2, 4 },
                { 6, 8 }
            };
            var constant = 2;
            var matrix = new Matrix<int>(values);
            var expectedMatrix = new Matrix<int>(expected);
            var result1 = constant * matrix;
            var result2 = matrix * constant;
            Assert.IsTrue(expectedMatrix.Equals(result1));
            Assert.IsTrue(expectedMatrix.Equals(result2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixMatrixMultiplicationDifferentSizes()
        {
            var result = new Matrix<int>(3, 2) * new Matrix<int>(3, 3);
        }

        [TestMethod]
        public void MatrixMatrixMultiplication()
        {
            var values1 = new int[,]
            {
                { 3, 2, -1 },
                { 0, 4, 5}
            };
            var values2 = new int[,]
            {
                { 1, -3, -1, -2 },
                { 3, 2, 0, -4 },
                { 4, 6, 5, 7 }
            };
            var expected = new int[,]
            {
                { 5, -11, -8, -21 },
                { 32, 38, 25, 19 }
            };
            var result = new Matrix<int>(values1) * new Matrix<int>(values2);
            Assert.IsTrue(result.Equals(new Matrix<int>(expected)));
        }

        [TestMethod]
        public void MatrixEnumerator()
        {
            var values1 = new int[,]
            {
                { 3, 2, -1 },
                { 0, 4, 5}
            };
            var matrix = new Matrix<int>(values1);
            var expected = new int[,]
            {
                { 3, 2, -1, 0, 4, 5 }
            };
            matrix.SequenceEqual(Enumerable.Cast<int>(expected));
        }

        [TestMethod]
        public void MatrixHashCode()
        {
            var values1 = new int[,]
            {
                { 3, 2, -1 },
                { 0, 4, 5}
            };
            var values2 = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var matrix1a = new Matrix<int>(values1);
            var matrix1b = new Matrix<int>(values1);
            var matrix2 = new Matrix<int>(values2);
            Assert.AreEqual(matrix1a.GetHashCode(), matrix1b.GetHashCode());
            Assert.AreNotEqual(matrix1a.GetHashCode(), matrix2.GetHashCode());
        }

        [TestMethod]
        public void MatrixToString()
        {
            var values = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var matrix = new Matrix<int>(values);
            Assert.AreEqual(matrix.ToString(), "1 2\n3 4\n");
        }

        [TestMethod]
        public void MatrixNxNConstructor()
        {
            var matrix = new MatrixNxN<int>(10);
            Assert.AreEqual(matrix.Rows, 10);
            Assert.AreEqual(matrix.Columns, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixNxNConstructorValuesNonSquaredSize()
        {
            var values = new int[,]
            {
                { 0, 1, 2 },
                { 3, 4, 5 }
            };
            new MatrixNxN<int>(values);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixNxNConstructorFronNonSquaredMatrixSize()
        {
            new MatrixNxN<int>(new Matrix<int>(10, 5));
        }

        [TestMethod]
        public void MatrixNxNConstructorFromMatrix()
        {
            var values = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var matrix1 = new MatrixNxN<int>(values);
            Assert.AreEqual(matrix1.Zip(Enumerable.Cast<int>(matrix1), (a, b) => a == b).Count(a => !a), 0);
            var matrix2 = new MatrixNxN<int>(matrix1);
            Assert.IsTrue(matrix2.Equals(matrix1));
        }

        [TestMethod]
        public void MatrixDeterminant2x2()
        {
            var values = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var result = new MatrixNxN<int>(values).Determinant();
            Assert.AreEqual(result, -2);
        }

        [TestMethod]
        public void MatrixDeterminant3x3()
        {
            var values = new int[,]
            {
                { 1, 1, 2 },
                { 3, 5, 1 },
                { 2, 8, 6 }
            };
            var result = new MatrixNxN<int>(values).Determinant();
            Assert.AreEqual(result, 34);
        }

        [TestMethod]
        public void MatrixDeterminant4x4()
        {
            var values = new int[,]
            {
                { 1, 0, 1, 2 },
                { 3, 5, 7, 1 },
                { 2, 8, 4, 6 },
                { 1, -4, -3, 1 }
            };
            var result = new MatrixNxN<int>(values).Determinant();
            Assert.AreEqual(result, 150);
        }

        [TestMethod]
        public void MatrixInverse()
        {
            var values = new double[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var expected = new double[,]
            {
                { -2, 1 },
                { 1.5, -0.5 }
            };
            var matrix = new MatrixNxN<double>(values).Inverse();
            var expectedMatrix = new MatrixNxN<double>(expected);
            Assert.IsTrue(matrix.Equals(expectedMatrix));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatrixCramersRuleInvalidEquationResults()
        {
            var values = new int[,]
            {
                { 1, 2 },
                { 3, 4 }
            };
            var matrix = new MatrixNxN<int>(values).CramersRule(new List<int> { 1, 2, 3 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MatrixCramersRuleNoValidResult()
        {
            var values = new double[,]
            {
                { 1, -1, 2 },
                { 4, 4, -2 },
                { -2, 2, -4 }
            };
            new MatrixNxN<double>(values).CramersRule(new List<double> { -3, 1, 6 });
        }

        [TestMethod]
        public void MatrixCramersRuleValidResult()
        {
            var values = new double[,]
            {
                { 8, -1, -2 },
                { -1, 7, -1 },
                { -2, -1, 9 }
            };
            var result = new MatrixNxN<double>(values).CramersRule(new List<double> { 0, 10, 23 });
            var expectedResult = new List<double> { 1, 2, 3 };
            Assert.IsTrue(result.SequenceEqual(expectedResult, NumericUtilities.FloatNumberEqualityComparer<double>()));
        }

        [TestMethod]
        public void MatrixDivision()
        {
            var values1 = new double[,]
            {
                { 1, 2, 3 },
                { 5, 6, 7 },
                { 0, 0, 0 }
            };
            var values2 = new double[,]
            {
                { -7, -9, 11 },
                { 10, -14, -5 },
                { 4, -10, 10 }
            };
            var expectedValues = new double[,]
            {
                { -281/963.0, -283/963.0, 913/1926.0 },
                { -989/963.0, -763/963.0, 2761/1926.0 },
                { 0, 0, 0 }
            };
            var result = new MatrixNxN<double>(values1) / new MatrixNxN<double>(values2);
            Assert.IsTrue(result.SequenceEqual(Enumerable.Cast<double>(expectedValues), NumericUtilities.FloatNumberEqualityComparer<double>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Matrix3x3ConstructorInvalidValuesSize()
        {
            new Matrix3x3<int>(new int[3, 2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Matrix3x3ConstructorInvalidMatrixSize()
        {
            new Matrix3x3<int>(new Matrix<int>(1, 2));
        }

        [TestMethod]
        public void MatrixDeterminantSarrusRule()
        {
            var values = new int[,]
            {
                { 1, 1, 2 },
                { 3, 5, 1 },
                { 2, 8, 6 }
            };
            var result = new Matrix3x3<int>(values).DeterminantSarrusRule();
            Assert.AreEqual(result, 34);
        }
    }
}
