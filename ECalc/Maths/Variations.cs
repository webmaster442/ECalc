using ECalc.IronPythonEngine;
using System;
using System.Linq;

namespace ECalc.Maths
{
    [Loadable]
    public static class Variations
    {
        [Category("Variations")]
        public static double VariationNoRepeat(double n, double k)
        {
            return Functions.Fact(n) / Functions.Fact(n - k);
        }

        [Category("Variations")]
        public static double VariationRepeat(double n, double k)
        {
            return Math.Pow(n, k);
        }

        [Category("Variations")]
        public static double CombinationNoRepeat(double n, double k)
        {
            return Functions.Fact(n) / Functions.Fact(n - k) * Functions.Fact(k);
        }

        [Category("Variations")]
        public static double CombinationRepeat(double n, double k)
        {
            double n2 = (n + k) - 1;
            return Functions.Fact(n2) / Functions.Fact(n2 - k) * Functions.Fact(k);
        }

        [Category("Variations")]
        public static double Permutation(double n, params double[] repeatk)
        {
            double ktest = repeatk.Sum();
            if (ktest > n)
                throw new ArgumentException("Sum of repeating elements is bigger than the first parameter!", nameof(repeatk));

            double factn = Functions.Fact(n);
            double sk = 1;
            foreach (var k in repeatk)
            {
                sk *= Functions.Fact(k);
            }
            return factn / sk;
        }
    }
}
