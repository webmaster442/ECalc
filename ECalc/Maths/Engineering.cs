using ECalc.IronPythonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
