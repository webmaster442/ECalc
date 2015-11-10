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

        public static double GetDouble(object o)
        {
            if (o is double) return (double)o;
            else return ((Fraction)o).ToDouble();
        }

        public static void ErrorDialog(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static bool IsInteger(object o)
        {
            if (o is double)
            {
                double d = Convert.ToDouble(o);
                double calc = d - Math.Truncate(d);
                return calc == 0.0d;
            }
            else return false;
        }
    }
}
