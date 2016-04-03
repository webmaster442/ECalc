using ECalc.IronPythonEngine;
using System;

namespace ECalc.Maths
{
    /// <summary>
    /// General mathematical functions
    /// </summary>
    [Loadable]
    public static class Functions
    {
        /// <summary>
        /// Replus calculation
        /// </summary>
        /// <param name="x1">Parameter 1</param>
        /// <param name="x2">Parameter 2</param>
        [Category("General")]
        public static double Replus(double x1, double x2)
        {
            return (x1 * x2) / (x1 + x2);
        }

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base.
        /// </summary>
        /// <param name="value1">The number whose logarithm is to be found.</param>
        /// <param name="basen">The base of the logarithm.</param>
        [Category("General")]
        public static double Log(double value1, double basen)
        {
            return Math.Log(value1, basen);
        }

        /// <summary>
        /// Returns the Square root of a specified number
        /// </summary>
        /// <param name="num">the number whose square root is to be found</param>
        [Category("General")]
        public static double Sqrt(double num)
        {
            return Math.Sqrt(num);
        }

        /// <summary>
        /// Returns the root of a specified number in a specified root base
        /// </summary>
        /// <param name="num">the number whose root is to be found</param>
        /// <param name="basen">the root base</param>
        [Category("General")]
        public static double Root(double num, double basen)
        {
            return Math.Pow(num, 1 / basen);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="num">number to be raised to a power.</param>
        /// <param name="exp">number that specifies a power.</param>
        [Category("General")]
        public static double Pow(double num, double exp)
        {
            return Math.Pow(num, exp);
        }
       
        /// <summary>
        /// Returns the least common multiple of two numbers
        /// </summary>
        /// <param name="x">A number</param>
        /// <param name="y">Another number</param>
        /// <returns></returns>
        [Category("General")]
        public static double Lcm(double x, double y)
        {
            return Math.Round((x * y) / Gcd(x, y), 0);
        }

        /// <summary>
        /// Returns the Greatest common divisor of two numbers
        /// </summary>
        /// <param name="x">A number</param>
        /// <param name="y">Another number</param>
        [Category("General")]
        public static double Gcd(double x, double y) //LNKO
        {
            if ((x == 0) || (y == 0)) throw new ArgumentException("Can't divide with zero!");
            while (x != y)
            {
                if (x > y) x = x - y;
                else y = y - x;
            }
            return x;
        }
    }
}
