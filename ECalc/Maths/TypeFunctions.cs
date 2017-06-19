using ECalc.IronPythonEngine;
using ECalc.IronPythonEngine.Types;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ECalc.Maths
{
    [Loadable]
    public static class TypeFunctions
    {
        [Category("Types")]
        public static Complex CplxConjugate(Complex param)
        {
            return Complex.Conjugate(param);
        }

        [Category("Types")]
        public static Complex CplxPolar(double abs, double angle)
        {
            double rad = 0;
            switch (Engine.Mode)
            {
                case TrigMode.DEG:
                    rad = TrigFunctions.Deg2Rad(angle);
                    break;
                case TrigMode.GRAD:
                    rad = TrigFunctions.Grad2Rad(angle);
                    break;
                case TrigMode.RAD:
                    rad = angle;
                    break;
            }
            return Complex.FromPolarCoordinates(abs, rad);
        }

        [Category("Types")]
        public static Complex CplxRi(double r, double i)
        {
            return new Complex(r, i);
        }

        [Category("Types")]
        public static Fraction Fraction(double numerator, double denominator)
        {
            return new Fraction(Convert.ToInt64(numerator), Convert.ToInt64(denominator));
        }

        [Category("Types")]
        public static Vector Vect2D(double x, double y)
        {
            return new Vector(x, y);
        }

        [Category("Types")]
        public static Vector Vect3D(double x, double y, double z)
        {
            return new Vector(x, y, z);
        }

        [Category("Types")]
        public static Dictionary<string, double> VectorToPolar(Vector v)
        {
            var ret = new Dictionary<string, double>();
            ret.Add("r", v.Magnitude);
            ret.Add("φ", TrigFunctions.ArcTan(v.Y / v.X));
            if (v.Dimensions == 3)
                ret.Add("θ", TrigFunctions.ArcCos((double)v.Z / v.Magnitude));
            return ret;
        }

        [Category("Types")]
        public static Vector PolarToVector(double r, double a1)
        {
            var x = r * TrigFunctions.Cos(a1);
            var y = r * TrigFunctions.Sin(a1);
            return new Vector(x, y);
        }

        [Category("Types")]
        public static Vector PolarToVector(double r, double a1, double a2)
        {
            var x = r * TrigFunctions.Sin(a2) * TrigFunctions.Cos(a1);
            var y = r * TrigFunctions.Sin(a2) * TrigFunctions.Sin(a1);
            var z = r * TrigFunctions.Cos(a2);
            return new Vector(x, y, z);
        }

        [Category("Types")]
        public static double Det(Matrix m)
        {
            return m.Determinant();
        }

        [Category("Types")]
        public static Matrix Transpose(Matrix m)
        {
            return m.Transpose();
        }

        [Category("Types")]
        public static Time Seconds(double s)
        {
            return new Time(s);
        }

        [Category("Types")]
        public static Time Minutes(double m)
        {
            return new Time(m, 0);
        }

        [Category("Types")]
        public static Time Hours(double h)
        {
            return new Time(h, 0, 0);
        }

        [Category("Types")]
        public static Time Days(double d)
        {
            return new Time(d, 0, 0, 0);
        }
    }
}
