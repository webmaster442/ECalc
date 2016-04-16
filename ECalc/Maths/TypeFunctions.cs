using ECalc.IronPythonEngine;
using ECalc.IronPythonEngine.Types;
using System;
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
        public static Set Set(params double[] d)
        {
            return new Set(d);
        }
    }
}
