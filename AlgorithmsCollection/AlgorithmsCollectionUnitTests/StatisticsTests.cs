using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class StatisticsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AverageValueNullList()
        {
            List<double> list = null;
            Statistics.AverageValue(list);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AverageValueEmptyList()
        {
            Statistics.AverageValue(new List<double>());
        }

        [TestMethod]
        public void AverageValue()
        {
            var list = new List<double> { 1.1, 5.4, 84.41, 51.25, 5.79, 66.22 };
            Assert.IsTrue(NumericUtilities.DoubleCompare(35.695, Statistics.AverageValue(list)));
            Assert.IsTrue(NumericUtilities.DoubleCompare(35.695, Statistics.ExpectedValue(list)));
        }

        [TestMethod]
        public void AverageValueWeight()
        {
            var list = new List<WeightedValue>
            {
                new WeightedValue { Weight = 5, Value = 12 },
                new WeightedValue { Weight = 0.2, Value = 5 },
                new WeightedValue { Weight = 1, Value = 14 },
                new WeightedValue { Weight = 0.67, Value = 13 },
                new WeightedValue { Weight = 1.7, Value = 55 },
            };
            var result = 20.67794632;
            Assert.IsTrue(NumericUtilities.DoubleCompare(result, Statistics.AverageValue(list)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CovarianceEmptyValues()
        {
            Statistics.Covariance(new List<double>(), new List<double>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CovarianceListDifferentLengths()
        {
            Statistics.Covariance(new List<WeightedValue>
            {
                new WeightedValue { Weight = 1.4, Value = 0.5 },
                new WeightedValue { Weight = 0.67, Value = 0.4 }
            }, new List<WeightedValue>
            {
                new WeightedValue { Weight = 0.21, Value = 1 }
            });
        }

        [TestMethod]
        public void CovarianceSingleValues()
        {
            var x = new List<double> { 65.21, 64.75, 65.26, 65.76, 65.96 };
            var y = new List<double> { 67.25, 66.39, 66.12, 65.70, 66.64 };
            Assert.IsTrue(NumericUtilities.DoubleCompare(-0.04644, Statistics.Covariance(x, y)));
        }

        [TestMethod]
        public void CovarianceWeightedValues()
        {
            var x = new List<WeightedValue>
            {
                new WeightedValue { Value = 65.21, Weight = 0.5 },
                new WeightedValue { Value = 64.75, Weight = 0.5 },
                new WeightedValue { Value = 65.26, Weight = 0.5 },
                new WeightedValue { Value = 65.76, Weight = 0.5 },
                new WeightedValue { Value = 65.96, Weight = 0.5 }
            };
            var y = new List<WeightedValue>
            {
                new WeightedValue { Value = 67.25, Weight = 0.5 },
                new WeightedValue { Value = 66.39, Weight = 0.5 },
                new WeightedValue { Value = 66.12, Weight = 0.5 },
                new WeightedValue { Value = 65.70, Weight = 0.5 },
                new WeightedValue { Value = 66.64, Weight = 0.5 }
            };
            Assert.IsTrue(NumericUtilities.DoubleCompare(-0.04644, Statistics.Covariance(x, y)));
        }

        [TestMethod]
        public void VarianceSingleValues()
        {
            var vals = new List<double> { 7.45, 11.25, 29.87, 41.15, 47.12 };
            Assert.IsTrue(NumericUtilities.DoubleCompare(248.572336, Statistics.Variance(vals)));
        }

        [TestMethod]
        public void VarianceWeightedValues()
        {
            var vals = new List<WeightedValue>
            {
                new WeightedValue { Value = 2, Weight = 1 },
                new WeightedValue { Value = 3, Weight = 1 },
                new WeightedValue { Value = 5, Weight = 0 },
                new WeightedValue { Value = 7, Weight = 0 },
                new WeightedValue { Value = 11, Weight = 4 },
                new WeightedValue { Value = 13, Weight = 1 },
                new WeightedValue { Value = 17, Weight = 2 },
                new WeightedValue { Value = 19, Weight = 1 },
                new WeightedValue { Value = 23, Weight = 0 },
            };
            Assert.IsTrue(NumericUtilities.DoubleCompare(28.25, Statistics.Variance(vals)));
        }

        [TestMethod]
        public void Corellation()
        {
            var x = new List<double> { 43, 21, 25, 42, 57, 59 };
            var y = new List<double> { 99, 65, 79, 75, 87, 81 };
            Assert.IsTrue(NumericUtilities.DoubleCompare(0.5298, Statistics.Corellation(x, y)));
        }

        [TestMethod]
        public void StandardErrorSingleValues()
        {
            var vals = new List<double> { 7.45, 11.25, 29.87, 41.15, 47.12 };
            var result = Statistics.StandardError(vals);
            Assert.IsTrue(NumericUtilities.DoubleCompare(15.7661769, Statistics.StandardError(vals)));
        }

        [TestMethod]
        public void StandardErrorWeightedValues()
        {
            var vals = new List<WeightedValue>
            {
                new WeightedValue { Value = 2, Weight = 1 },
                new WeightedValue { Value = 3, Weight = 1 },
                new WeightedValue { Value = 5, Weight = 0 },
                new WeightedValue { Value = 7, Weight = 0 },
                new WeightedValue { Value = 11, Weight = 4 },
                new WeightedValue { Value = 13, Weight = 1 },
                new WeightedValue { Value = 17, Weight = 2 },
                new WeightedValue { Value = 19, Weight = 1 },
                new WeightedValue { Value = 23, Weight = 0 },
            };
            Assert.IsTrue(NumericUtilities.DoubleCompare(5.31507, Statistics.StandardError(vals)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MedianNullValues()
        {
            Statistics.Median(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MedianEmptyValues()
        {
            List<double> list = null;
            Statistics.Median(list);
        }

        [TestMethod]
        public void MedianValuesOddLength()
        {
            var vals = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.AreEqual(Statistics.Median(vals), 5);
        }

        [TestMethod]
        public void MedianValuesEvenLength()
        {
            var vals = new List<double> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.IsTrue(NumericUtilities.DoubleCompare(4.5, Statistics.Median(vals)));
        }

        [TestMethod]
        public void Modus()
        {
            var vals = new List<double> { 1.5, 2, 3.1, 1.5, 6, 8.4, 1.5, 5 };
            Assert.IsTrue(NumericUtilities.DoubleCompare(1.5, Statistics.Modus(vals)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MovingAverageOddNumber()
        {
            Statistics.MovingAverage(new List<double> { 1, 2, 3 }, 2);
        }

        [TestMethod]
        public void MovingAverage()
        {
            var vals = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expected = new List<double> { 6.0 / 3.0, 9.0 / 3.0, 12.0 / 3.0, 15.0 / 3.0, 18.0 / 3.0, 21.0 / 3.0, 24.0/3.0, 27.0 / 3.0 };
            var result = Statistics.MovingAverage(vals, 3);
            result
                .Zip(expected, (res, exp) => NumericUtilities.DoubleCompare(res, exp))
                .ToList()
                .ForEach(r => Assert.IsTrue(r));
        }

        [TestMethod]
        public void LinearRegression()
        {
            var vals = new List<KeyValuePair<int, double>>
            {
                new KeyValuePair<int, double>(1, 5),
                new KeyValuePair<int, double>(2, 6),
                new KeyValuePair<int, double>(3, 8),
                new KeyValuePair<int, double>(4, 6),
                new KeyValuePair<int, double>(5, 7),
            };
            double slope, offset;
            Statistics.LinearRegression(vals, out slope, out offset);
            Assert.IsTrue(NumericUtilities.DoubleCompare(0.4, slope));
            Assert.IsTrue(NumericUtilities.DoubleCompare(5.2, offset));
        }
    }
}
