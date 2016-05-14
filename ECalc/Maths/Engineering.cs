using ECalc.IronPythonEngine;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ECalc.Maths
{
    [Loadable]
    public static class Engineering
    {
        /// <summary>
        /// Replus calculation
        /// </summary>
        /// <param name="x1">Parameter 1</param>
        /// <param name="x2">Parameter 2</param>
        [Category("Engineering")]
        public static double Replus(double x1, double x2)
        {
            return (x1 * x2) / (x1 + x2);
        }

        [Category("Engineering")]
        public static Complex Replus(Complex c1, Complex c2)
        {
            return (c1 * c2) / (c1 + c2);
        }

        /// <summary>
        /// Calculates the angular frequency
        /// </summary>
        /// <param name="freq">regular frequency</param>
        /// <returns>angular frequency</returns>
        [Category("Engineering")]
        public static double AngularFreq(double freq)
        {
            return Math.PI * 2 * freq;
        }

        /// <summary>
        /// Calculates the wavelength of a frequency
        /// </summary>
        /// <param name="freq">frequency</param>
        /// <returns>wavelength of frequency</returns>
        [Category("Engineering")]
        public static double Wavelength(double freq)
        {
            return 299792.458 / freq;
        }

        [Category("Engineering")]
        public static Complex Xc(double frequency, double capacity)
        {
            double imaginary = 1 / (2 * Math.PI * frequency * capacity);
            return new Complex(0, -imaginary);
        }

        [Category("Engineering")]
        public static Complex Xl(double frequency, double inductivity)
        {
            double imaginary = 2 * Math.PI * frequency * inductivity;
            return new Complex(0, imaginary);
        }

        [Category("Engineering")]
        public static double Map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        [Category("Engineering")]
        public static double Percent(double input, double percent)
        {
            if (percent < 0) throw new ArgumentException("Percent must be positive", nameof(percent));
            var multiplier = percent / 100;
            return input * multiplier;
        }

        [Category("Engineering")]
        public static object QuadraticEq(double a, double b, double c)
        {
            if (a == 0)
            {
                if (b == 0)
                {
                    if (c == 0) return 0;
                    else return double.NaN;
                }
                return -c / b;
            }
            else
            {
                double d = (b * b) - 4 * a * c;
                if (d < 0)
                {
                    var dc = new Complex(d, 0);
                    var ret = new Dictionary<string, Complex>();
                    ret.Add("x1", (-b - Complex.Sqrt(dc)) / 2 * a);
                    ret.Add("x2", (-b + Complex.Sqrt(dc)) / 2 * a);
                    return ret;
                }
                else if (d == 0) return -b / 2 * a;
                else
                {
                    var ret = new Dictionary<string, double>();
                    ret.Add("x1", (-b - Math.Sqrt(d)) / 2 * a);
                    ret.Add("x2", (-b + Math.Sqrt(d)) / 2 * a);
                    return ret;
                }
            }
        }

        [Category("Engineering")]
        public static double Erf(double x)
        {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }
    }
}
