using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Maths
{
    public class Replus : IFunction
    {
        public string Name
        {
            get { return "Replus"; }
        }

        public object Run(params object[] arguments)
        {
            if (Helpers.IsComplex(arguments[0]) || Helpers.IsComplex(arguments[1]))
            {
                Complex a = Helpers.GetComplex(arguments[0]);
                Complex b = Helpers.GetComplex(arguments[1]);
                return (a * b) / (a + b);
            }
            else
            {
                double n1 = (double)arguments[0];
                double n2 = (double)arguments[1];
                return (n1 * n2) / (n1 + n2);
            }
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    public class Pow: IFunction
    {
        public string Name
        {
            get { return "Pow"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            double e = Convert.ToDouble(arguments[1]);
            return Math.Pow(b, e);
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    public class Square : IFunction
    {
        public string Name
        {
            get { return "Square"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            return Math.Pow(b, 2.0d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Root : IFunction
    {
        public string Name
        {
            get { return "Root"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            double e = Convert.ToDouble(arguments[1]);
            return Math.Pow(b, 1/e);
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    public class Sqrt : IFunction
    {
        public string Name
        {
            get { return "Sqrt"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            return Math.Pow(b, 1 / 2.0d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Log : IFunction
    {
        public string Name
        {
            get { return "Log"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            double e = Convert.ToDouble(arguments[1]);
            return Math.Log(b, e);
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    public class Factorial : IFunction
    {
        public string Name
        {
            get { return "Factorial"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            double result = 1;
            for (int i = 1; i < b; i++)
            {
                result *= i;
            }
            return result;
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Abs : IFunction
    {
        public string Name
        {
            get { return "Abs"; }
        }

        public object Run(params object[] arguments)
        {
            double b = Convert.ToDouble(arguments[0]);
            return Math.Abs(b);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Gcd: IFunction
    {
        public string Name
        {
            get { return "GCD"; }
        }

        public static double FGcd(double x, double y) //LNKO
        {
            if ((x == 0) || (y == 0)) throw new ArgumentException("Can't divide with zero!");
            while (x != y)
            {
                if (x > y) x = x - y;
                else y = y - x;
            }
            return x;
        }


        public object Run(params object[] arguments)
        {
            double x = Convert.ToDouble(arguments[0]);
            double y = Convert.ToDouble(arguments[1]);
            return FGcd(x, y);
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }

    public class LCM: IFunction
    {
        public string Name
        {
            get { return "LCM"; }
        }

        public object Run(params object[] arguments)
        {
            double x = Convert.ToDouble(arguments[0]);
            double y = Convert.ToDouble(arguments[1]);
            return Math.Round((x * y) / Gcd.FGcd(x, y), 0);
        }

        public int ParamCount
        {
            get { return 2; }
        }
    }
}
