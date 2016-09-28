using ECalc.IronPythonEngine.Types;
using System;
using System.Numerics;
using System.Windows;

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

        public static bool IsSpecialType(object o)
        {
            return (o is Complex) || (o is Fraction) || (o is Matrix) || (o is IronPythonEngine.Types.Vector);
        }

        public static string ReadResourceStream(string path)
        {
            var resx = Application.GetResourceStream(new Uri("pack://application:,,,/ECalc;component/" + path));
            using (System.IO.StreamReader sr = new System.IO.StreamReader(resx.Stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static string DivideToFileSize(double val)
        {
            double value = val;
            string prefix = "Byte";

            if (value > 1180591620717411303424.0)
            {
                value /= 1180591620717411303424.0;
                prefix = "ZiB";
            }
            if (value > 1152921504606846976.0)
            {
                value /= 1152921504606846976.0;
                prefix = "EiB";
            }
            if (value > 1125899906842624.0)
            {
                value /= 1125899906842624.0;
                prefix = "PiB";
            }
            else if (value > 1099511627776.0)
            {
                value /= 1099511627776.0;
                prefix = "TiB";
            }
            else if (value > 1073741824.0)
            {
                value /= 1073741824.0;
                prefix = "GiB";
            }
            else if (value > 1048576.0)
            {
                value /= 1048576.0;
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
