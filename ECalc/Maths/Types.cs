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

        public string Category
        {
            get { return "Type functions"; }
        }

        public object Run(params object[] arguments)
        {
            var n1 = (double)arguments[0];
            var n2 = (double)arguments[1];
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

        public string Category
        {
            get { return "Type functions"; }
        }

        public object Run(params object[] arguments)
        {
            var n1 = (double)arguments[0];
            var n2 = (double)arguments[1];

            switch (Engine.Mode)
            {
            
                case TrigMode.RAD:
                    return Complex.FromPolarCoordinates(n1, n2);
                default:
                    var real = TrigFunctions.Cos(n2) * n1;
                    var imaginary = TrigFunctions.Sin(n2) * n1;
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

        public string Category
        {
            get { return "Type functions"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            var n1 = (double)arguments[0];
            var n2 = (double)arguments[1];
            return new Fraction((long)n1, (long)n2);
        }
    }

    internal class CplxConjugate: IFunction
    {
        public string Name
        {
            get { return "CplxConjugate"; }
        }

        public string Category
        {
            get { return "Type functions"; }
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

        public string Category
        {
            get { return "Type functions"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            var n1 = (double)arguments[0];
            var n2 = (double)arguments[1];
            return new Vector(n1, n2);
        }
    }

    internal class Vector3D : IFunction
    {
        public string Name
        {
            get { return "Vect3D"; }
        }

        public string Category
        {
            get { return "Type functions"; }
        }

        public int ParamCount
        {
            get { return 3; }
        }

        public object Run(params object[] arguments)
        {
            var n1 = (double)arguments[0];
            var n2 = (double)arguments[1];
            var n3 = (double)arguments[2];
            return new Vector(n1, n2, n3);
        }
    }

    internal class Determinant : IFunction
    {
        public string Name
        {
            get { return "Det"; }
        }

        public string Category
        {
            get { return "Type functions"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            var matrix = (Matrix)arguments[0];
            return matrix.Determinant();
        }
    }

    internal class Transpose : IFunction
    {
        public string Name
        {
            get { return "Transpose"; }
        }

        public string Category
        {
            get { return "Type functions"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            var matrix = (Matrix)arguments[0];
            return (Matrix)matrix.Transpose();
        }
    }

    internal class Negate : IFunction
    {
        public string Name
        {
            get { return "Negate"; }
        }

        public string Category
        {
            get { return "Type functions"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            var matrix = (Matrix)((Matrix)arguments[0]).Clone();
            matrix.Negate();
            return matrix;
        }
    }
}
