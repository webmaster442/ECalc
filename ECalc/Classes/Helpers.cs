using ECalc.IronPythonEngine.Types;
using System.Numerics;

namespace ECalc.Classes
{
    class Helpers
    {
        public static bool IsComplex(object o)
        {
            return o is Complex;
        }

        public static Complex GetComplex(object o)
        {
            if (IsComplex(o)) return (Complex)o;
            else return new Complex((double)o, 0);
        }

        public static double GetDouble(object o)
        {
            if (o is double) return (double)o;
            else return ((Fraction)o).ToDouble();
        }

        public static bool IsSpecialType(object o)
        {
            return (o is Complex) || (o is Fraction) || (o is Matrix) || (o is Vector);
        }

        public static string DivideToFileSize(double val)
        {
            double value = val;
            string prefix = "Byte";

            if (value > 1125899906842624)
            {
                value /= 1125899906842624;
                prefix = "PiB";
            }
            else if (value > 1099511627776)
            {
                value /= 1099511627776;
                prefix = "TiB";
            }
            else if (value > 1073741824)
            {
                value /= 1073741824;
                prefix = "GiB";
            }
            else if (value > 1048576)
            {
                value /= 1048576;
                prefix = "MiB";
            }
            else if (value > 1024)
            {
                value /= 1024;
                prefix = "kiB";
            }
            return string.Format("{0} {1}", value, prefix);
        }
    }
}
