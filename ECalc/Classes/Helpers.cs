using ECalc.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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

        public static void ErrorDialog(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static string ResultToString(object o)
        {
            StringBuilder sb = new StringBuilder();
            if (IsComplex(o))
            {
                Complex cplx = GetComplex(o);
                sb.Append("R: ");
                sb.Append(cplx.Real);
                sb.Append(" i: ");
                sb.Append(cplx.Imaginary);
                sb.Append(" φ: ");
                switch (Engine.Mode)
                {
                    case TrigMode.DEG:
                        sb.Append(TrigFunctions.Rad2Deg(cplx.Phase));
                        sb.Append(" °");
                        break;
                    case TrigMode.GRAD:
                        sb.Append(TrigFunctions.Rad2Grad(cplx.Phase));
                        sb.Append(" grad");
                        break;
                    case TrigMode.RAD:
                        sb.Append(cplx.Phase);
                        sb.Append(" rad");
                        break;
                }

                return sb.ToString();
            }
            else return o.ToString();
        }
    }
}
