using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Maths
{
    internal static class EnumerableStat
    {
        public static double Variance(IEnumerable<double> source)
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

        public static double StandardDeviation(IEnumerable<double> source)
        {
            return Math.Sqrt(Variance(source));
        }

        public static double Median(IEnumerable<double> source)
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

        public static double Range(IEnumerable<double> source)
        {
            return source.Max() - source.Min();
        }

        public static double Count(IEnumerable<double> source)
        {
            return source.Count();
        }

        public static double Minimum(IEnumerable<double> source)
        {
            return source.Min();
        }

        public static double Maximum(IEnumerable<double> source)
        {
            return source.Max();
        }

        public static double Average(IEnumerable<double> source)
        {
            return source.Average();
        }
    }
}
