using ECalc.Maths;
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
    }
}
