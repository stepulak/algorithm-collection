﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Struct that represents weighted value.
    /// </summary>
    public struct WeightedValue
    {
        public double Weight { get; set; }
        public double Value { get; set; }
    }

    /// <summary>
    /// Static class with various statistics methods.
    /// </summary>
    public static class Statistics
    {
        /// <summary>
        /// Count expected (average) value of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>Expected (average) value of given list</returns>
        public static double ExpectedValue(List<double> values) => AverageValue(values);

        /// <summary>
        /// Count expected (average) value of given list.
        /// </summary>
        /// <param name="values">List of weighted values</param>
        /// <returns>Expected (average) value of given list</returns>
        public static double ExpectedValue(List<WeightedValue> values) => AverageValue(values);
        
        /// <summary>
        /// Count average (expected) value of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>Average (expected) value of given list</returns>
        public static double AverageValue(List<double> values)
        {
            TestEmptyOrNullValuesThrow(values);
            return values.Average();
        }

        /// <summary>
        /// Count average (expected) value of given list.
        /// </summary>
        /// <param name="values">List of weighted values</param>
        /// <returns>Average (expected) value of given list</returns>
        public static double AverageValue(List<WeightedValue> values)
        {
            TestEmptyOrNullValuesThrow(values);
            var sumWeights = GetWeightsSumThrowIfZero(values);
            return values.Aggregate(0.0, (prev, current) => prev + current.Weight * current.Value) / sumWeights;
        }
        
        /// <summary>
        /// Count variance of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>Variance of given list</returns>
        public static double Variance(List<double> values) => Covariance(values, values);

        /// <summary>
        /// Count variance of given list.
        /// </summary>
        /// <param name="values">List of weighted values</param>
        /// <returns>Variance of given list</returns>
        public static double Variance(List<WeightedValue> values)
        {
            var sameWeightedValues = values
                .Select(val =>
                {
                    val.Weight = 1;
                    return val;
                }).ToList();
            return Covariance(values, sameWeightedValues);
        }

        /// <summary>
        /// Count standard error of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>Standard error of given list</returns>
        public static double StandardError(List<double> values) => Math.Sqrt(Variance(values));

        /// <summary>
        /// Count standard error of given list.
        /// </summary>
        /// <param name="values">List of weighted values</param>
        /// <returns>Standard error of given list</returns>
        public static double StandardError(List<WeightedValue> values) => Math.Sqrt(Variance(values));

        /// <summary>
        /// Count covariance between two given lists with same length.
        /// </summary>
        /// <param name="valuesX">List of X values</param>
        /// <param name="valuesY">List of Y values</param>
        /// <returns>Covariance between values X and Y</returns>
        public static double Covariance(List<double> valuesX, List<double> valuesY)
        {
            TestValuesForCovarianceThrow(valuesX, valuesY);
            var transformedX = valuesX.Select(val => new WeightedValue { Value = val, Weight = 1 }).ToList();
            var transformedY = valuesY.Select(val => new WeightedValue { Value = val, Weight = 1 }).ToList();
            return CovarianceImpl(transformedX, transformedY);
        }

        /// <summary>
        /// Count covariance between two given lists with same length.
        /// </summary>
        /// <param name="valuesX">List of weighted X values</param>
        /// <param name="valuesY">List of weighted Y values</param>
        /// <returns>Covariance between weighted values X and Y</returns>
        public static double Covariance(List<WeightedValue> valuesX, List<WeightedValue> valuesY)
        {
            TestValuesForCovarianceThrow(valuesX, valuesY);
            return CovarianceImpl(valuesX, valuesY);
        }

        /// <summary>
        /// Count corellation between two given lists with same length.
        /// </summary>
        /// <param name="valuesX">List of X values</param>
        /// <param name="valuesY">List of Y values</param>
        /// <returns>Corellation between values X and Y</returns>
        public static double Corellation(List<double> valuesX, List<double> valuesY) => Covariance(valuesX, valuesY) / (StandardError(valuesX) * StandardError(valuesY));

        /// <summary>
        /// Get median of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>Median of given list</returns>
        public static double Median(List<double> values)
        {
            TestEmptyOrNullValuesThrow(values);
            var middle = values.Count / 2;
            if (values.Count % 2 == 1)
            {
                return values[middle];
            }
            return (values[middle - 1] + values[middle]) / 2.0;
        }

        /// <summary>
        /// Get modus of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>Modus of given list</returns>
        public static double Modus(List<double> values)
        {
            TestEmptyOrNullValuesThrow(values);
            var comparer = NumericUtilities.FloatNumberEqualityComparer<double>();
            var occurenceDictionary = new ConcurrentDictionary<double, int>(comparer);
            foreach (var value in values)
            {
                occurenceDictionary.AddOrUpdate(value, 1, (key, val) => val + 1);
            }
            return occurenceDictionary
                .OrderByDescending(pair => pair.Value)
                .First().Key;
        }

        /// <summary>
        /// Count windowed moving average of given list.
        /// </summary>
        /// <param name="values">List of values</param>
        /// <param name="window">Size of floating window</param>
        /// <returns>Moving average from given list</returns>
        public static List<double> MovingAverage(List<double> values, uint window)
        {
            TestEmptyOrNullValuesThrow(values);
            if (window == 0 || window % 2 == 0)
            {
                throw new ArgumentException("Window cannot be zero or even number");
            }
            if (window == 1)
            {
                return values; // Optimalization
            }
            var windowOffset = (int)window / 2;
            var result = new List<double>(values.Count);
            for (int index = windowOffset; index < values.Count - windowOffset; index++)
            {
                double avg = 0.0;
                for (int subindex = index - windowOffset; subindex <= index + windowOffset; subindex++)
                {
                    avg += values[subindex];
                }
                avg /= window;
                result.Add(avg);
            }
            return result;
        }

        /// <summary>
        /// Count linear regression (y = slope * x + intercept) from given list.
        /// </summary>
        /// <param name="values">List of pairs of index+value</param>
        /// <param name="slope">Slope result</param>
        /// <param name="intercept">Intercept result</param>
        public static void LinearRegression(List<KeyValuePair<int, double>> values, out double slope, out double intercept)
        {
            TestEmptyOrNullValuesThrow(values);
            var valuesX = values.Select(pair => (double)pair.Key).ToList();
            var valuesY = values.Select(pair => pair.Value).ToList();
            slope = Covariance(valuesX, valuesY) / Variance(valuesX);
            intercept = AverageValue(valuesY) - slope * AverageValue(valuesX);
        }
        
        private static void TestEmptyOrNullValuesThrow<T>(List<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("Values are null");
            }
            if (values.Count == 0)
            {
                throw new ArgumentException("Values are empty");
            }
        }

        private static double GetWeightsSumThrowIfZero(List<WeightedValue> values)
        {
            var sumWeights = values.Aggregate(0.0, (prev, current) => prev + current.Weight);
            if (NumericUtilities.DoubleCompare(sumWeights, 0.0))
            {
                throw new ArgumentException("Sum of weights cannot be zero");
            }
            return sumWeights;
        }

        private static void TestValuesForCovarianceThrow<T>(List<T> valuesX, List<T> valuesY)
        {
            TestEmptyOrNullValuesThrow(valuesX);
            TestEmptyOrNullValuesThrow(valuesY);
            if (valuesX.Count != valuesY.Count)
            {
                throw new ArgumentException("Given lists do not have same size");
            }
        }

        private static double CovarianceImpl(List<WeightedValue> valuesX, List<WeightedValue> valuesY)
        {
            var expectedValueX = ExpectedValue(valuesX);
            var expectedValueY = ExpectedValue(valuesY);
            var vals = valuesX
                .Zip(valuesY, (x, y) => new { x, y })
                .Select(pair =>
                {
                    var x = pair.x.Value - expectedValueX;
                    var y = pair.y.Value - expectedValueY;
                    var weight = pair.x.Weight * pair.y.Weight;
                    var value = x * y * weight;
                    return new WeightedValue { Value = value, Weight = weight };
                });
            var agg = vals.Aggregate((prev, curr) => new WeightedValue { Value = prev.Value + curr.Value, Weight = prev.Weight + curr.Weight });
            return agg.Value / agg.Weight;
        }
    }
}
