using ECalc.Classes;
using System.Numerics;

namespace ECalc.Maths
{
    internal class CplxRi : IFunction
    {
        public string Name
        {
            get { return "CplxRi"; }
        }

        public object Run(params object[] arguments)
        {
            double n1 = (double)arguments[0];
            double n2 = (double)arguments[1];
            return new Complex(n1, n2);
        }

        public TrigMode Mode
        {
            get;
            set;
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    internal class CplxPolar: IFunction
    {
        public string Name
        {
            get { return "CplxPolar"; }
        }

        public object Run(params object[] arguments)
        {
            double n1 = (double)arguments[0];
            double n2 = (double)arguments[1];

            switch (Engine.Mode)
            {
            
                case TrigMode.RAD:
                    return Complex.FromPolarCoordinates(n1, n2);
                default:
                    double real = TrigFunctions.Cos(n2) * n1;
                    double imaginary = TrigFunctions.Sin(n2) * n1;
                    return new Complex(real, imaginary);
            }
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    internal class Fract : IFunction
    {
        public string Name
        {
            get { return "Fraction"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            double n1 = (double)arguments[0];
            double n2 = (double)arguments[1];
            return new Fraction((long)n1, (long)n2);
        }
    }

    internal class CplxConjugate: IFunction
    {
        public string Name
        {
            get { return "CplxConjugate"; }
        }

        public object Run(params object[] arguments)
        {
            Complex param = Helpers.GetComplex(arguments[0]);
            return Complex.Conjugate(param);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    internal class Vector2D : IFunction
    {
        public string Name
        {
            get { return "Vect2D"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            double n1 = (double)arguments[0];
            double n2 = (double)arguments[1];
            return new Vector(n1, n2);
        }
    }

    internal class Vector3D : IFunction
    {
        public string Name
        {
            get { return "Vect3D"; }
        }

        public int ParamCount
        {
            get { return 3; }
        }

        public object Run(params object[] arguments)
        {
            double n1 = (double)arguments[0];
            double n2 = (double)arguments[1];
            double n3 = (double)arguments[2];
            return new Vector(n1, n2, n3);
        }
    }

    internal class Determinant : IFunction
    {
        public string Name
        {
            get { return "Det"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            Matrix matrix = (Matrix)arguments[0];
            return matrix.Determinant();
        }
    }

    internal class Transpose : IFunction
    {
        public string Name
        {
            get { return "Transpose"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            Matrix matrix = (Matrix)arguments[0];
            return (Matrix)matrix.Transpose();
        }
    }

    internal class Negate : IFunction
    {
        public string Name
        {
            get { return "Negate"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            Matrix matrix = (Matrix)((Matrix)arguments[0]).Clone();
            matrix.Negate();
            return matrix;
        }
    }
}
