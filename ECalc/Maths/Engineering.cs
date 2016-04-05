using ECalc.IronPythonEngine;
using System;
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
    }
}
