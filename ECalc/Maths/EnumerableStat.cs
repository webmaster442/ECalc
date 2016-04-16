using ECalc.IronPythonEngine;
using ECalc.IronPythonEngine.Types;
using System;
using System.Linq;

namespace ECalc.Maths
{
    /// <summary>
    /// Stat functions for enumerable sets. Portions taken from:
    /// https://github.com/mariusschulz/NAverage/blob/master/NAverage and
    /// http://www.codeproject.com/Articles/42492/Using-LINQ-to-Calculate-Basic-Statistics
    /// </summary>
    [Loadable]
    public static class EnumerableStat
    {
        /// <summary>
        /// Variance is the measure of the amount of variation of all the scores for a variable (not just the extremes which give the range).
        /// </summary>
        /// <param name="source">The numbers whose variation is to be calculated.</param>
        /// <returns>The variation of the given numbers.</returns>
        [Category("Statistics")]
        public static double Variance(Set source)
        {
            int n = 0;
            double mean = 0;
            double M2 = 0;

            foreach (double x in source)
            {
                n = n + 1;
                double delta = x - mean;
                mean = mean + delta / n;
                M2 += delta * (x - mean);
            }
            return M2 / (n - 1);
        }

        /// <summary>
        /// The Standard Deviation of a statistical population, a data set, or a probability distribution is the square root of its variance.
        /// </summary>
        /// <param name="source">The numbers whose Standard Deviation is to be calculated.</param>
        /// <returns>The  Standard Deviation of the given numbers.</returns>
        [Category("Statistics")]
        public static double StandardDeviation(Set source)
        {
            return Math.Sqrt(Variance(source));
        }


        /// <summary>
        /// Calculates the median of the given numbers.
        /// </summary>
        /// <param name="source">The numbers whose median is to be calculated.</param>
        /// <returns>The median of the given numbers.</returns>
        [Category("Statistics")]
        public static double Median(Set source)
        {
            var sortedList = from number in source
                             orderby number
                             select number;

            int count = sortedList.Count();
            int itemIndex = count / 2;
            if (count % 2 == 0) // Even number of items. 
                return (sortedList.ElementAt(itemIndex) +
                        sortedList.ElementAt(itemIndex - 1)) / 2;

            // Odd number of items. 
            return sortedList.ElementAt(itemIndex);
        }

        /// <summary>
        /// Mode is the value that occurs the most frequently in a data set or a probability distribution.
        /// </summary>
        /// <param name="source">The numbers whose mode is to be calculated.</param>
        /// <returns>The mode of the given numbers.</returns>
        [Category("Statistics")]
        public static double Mode(this Set source)
        {
            var sortedList = from number in source
                             orderby number
                             select number;

            int count = 0;
            int max = 0;
            double current = 0.0;
            double mode = 0.0;

            foreach (double next in sortedList)
            {
                if (current.Equals(next) == false)
                {
                    current = next;
                    count = 1;
                }
                else count++;

                if (count > max)
                {
                    max = count;
                    mode = current;
                }
            }

            if (max > 1) return mode;

            return double.NaN;
        }

        /// <summary>
        /// Range is the length of the smallest interval which contains all the data.
        /// </summary>
        /// <param name="source">The numbers whose range is to be calculated.</param>
        /// <returns>The range of the given numbers.</returns>
        [Category("Statistics")]
        public static double Range(Set source)
        {
            return source.Max() - source.Min();
        }

        /// <summary>
        /// Counts the items in the set
        /// </summary>
        /// <param name="source">The numbers whose count is to be calculated.</param>
        ///  <returns>The count of the given numbers.</returns>
        [Category("Statistics")]
        public static double Count(Set source)
        {
            return source.Count();
        }

        /// <summary>
        /// Returns the minumum value of the set
        /// </summary>
        /// <param name="source">The numbers whose minimum is to be calculated.</param>
        /// <returns>The minimum of the given numbers.</returns>
        [Category("Statistics")]
        public static double Minimum(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");
            return source.Min();
        }

        /// <summary>
        /// Returns the maximum value of the set
        /// </summary>
        /// <param name="source">The numbers whose maximum is to be calculated.</param>
        /// <returns>The maximum of the given numbers.</returns>
        [Category("Statistics")]
        public static double Maximum(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");
            return source.Max();
        }

        /// <summary>
        /// Calculates the arithmetic mean of the given numbers.
        /// </summary>
        /// <param name="source">The numbers whose arithmetic mean is to be calculated.</param>
        /// <returns>The arithmetic mean of the given numbers.</returns>
        [Category("Statistics")]
        public static double ArithmeticMean(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");
            return source.Average();
        }

        /// <summary>
        /// Calculates the geometric mean of the given numbers.
        /// </summary>
        /// <param name="source">The numbers whose geometric mean is to be calculated.</param>
        /// <returns>The geometric mean of the given numbers.</returns>
        [Category("Statistics")]
        public static double GeometricMean(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");

            if (source.Any(number => number < 0)) throw new InvalidOperationException("The collection must not contain negative numbers!");
            double productOfNumbers = 1;

            foreach (double number in source)
            {
                productOfNumbers *= number;
            }

            double numbersCount = source.Count();
            double exponent = 1.0 / numbersCount;
            double geometricMean = Math.Pow(productOfNumbers, exponent);

            return geometricMean;
        }

        /// <summary>
        /// Calculates the harmonic mean of the given numbers.
        /// The harmonic mean is defined as zero if at least one of the numbers is zero.
        /// </summary>
        /// <param name="source">The numbers whose harmonic mean is to be calculated.</param>
        /// <returns>The harmonic mean of the given numbers.</returns>
        [Category("Statistics")]
        public static double HarmonicMean(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");

            if (source.Any(number => number < 0)) throw new InvalidOperationException("The collection must not contain negative numbers!");

            if (source.Contains(0)) return 0;

            double sumOfReciprocalValues = source.Sum(number => 1.0 / number);
            double harmonicMean = source.Count() / sumOfReciprocalValues;

            return harmonicMean;
        }

        /// <summary>
        /// Calculates the midrange of the specified collection of numbers.
        /// </summary>
        /// <param name="source">The numbers whose midrange is to be calculated.</param>
        /// <returns>The midrange of the specified collection of numbers.</returns>
        [Category("Statistics")]
        public static double Midrange(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");

            double minValue = source.Min();
            double maxValue = source.Max();

            double midrange = (minValue + maxValue) / 2.0d;

            return midrange;
        }

        /// <summary>
        /// Calculates the quadratic mean (or RMS, respectively) of the specified numbers.
        /// </summary>
        /// <param name="source">The numbers whose quadratic mean is to be calculated.</param>
        [Category("Statistics")]
        public static double QuadraticMean(Set source)
        {
            if (!source.Any()) throw new InvalidOperationException("The collection must not be empty!");

            Set squaredNumbers = (Set)source.Select(n => n * n).ToList();

            double arithmeticMeanOfSquaredNumbers = ArithmeticMean(squaredNumbers);
            double quadraticMean = Math.Sqrt(arithmeticMeanOfSquaredNumbers);

            return quadraticMean;
        }
    }
}
