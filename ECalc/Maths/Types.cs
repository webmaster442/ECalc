using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
}
