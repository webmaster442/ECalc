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
        /// Returns the absolute value of the parameter
        /// </summary>
        /// <param name="param">the number that's absolute value will be reaturned</param>
        /// <returns>the absolute value of the parameter</returns>
        [Category("General")]
        public static double Abs(double param)
        {
            return Math.Abs(param);
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
        /// Calculates the factorial of a parameter number
        /// </summary>
        /// <param name="target">target number</param>
        /// <returns>factorial of target</returns>
        [Category("General")]
        public static double Fact(double target)
        {
            double result = 1;
            for (int i = 1; i < target; i++)
            {
                result *= i;
            }
            return result;
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

        /// <summary>
        ///  Rounds a double-precision floating-point value to a specified number of fractional digits.
        /// </summary>
        /// <param name="number">A double-precision floating-point number to be rounded</param>
        /// <param name="digits">The number of fractional digits in the return value</param>
        /// <returns> The number nearest to value that contains a number of fractional digits equal to digits</returns>
        [Category("General")]
        public static double Round(double number, int digits)
        {
            return Math.Round(number, digits);
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified double-precision floating-point number.
        /// </summary>
        /// <param name="number">A double-precision floating-point number</param>
        /// <returns>The largest integer less than or equal to number</returns>
        [Category("General")]
        public static double Floor(double number)
        {
            return Math.Floor(number);
        }
    }
}
