using ECalc.Classes;
using System;

namespace ECalc.Maths
{
    internal static class ProbHelper
    {
        public static void isValidValue(double val)
        {
            bool b = (val <= 1) && (val >= 0);
            if (!b) throw new ArgumentException("Probability values are expected within the range 0 and 1");
        }
    }


    internal class ProbNotA : IFunction
    {
        public string Name
        {
            get { return "ProbNotA"; }
        }

        public string Category
        {
            get { return "Probability"; }
        }

        public int ParamCount
        {
            get { return 1; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            ProbHelper.isValidValue(d);
            return 1 - d;
        }
    }

    internal class ProbAorB : IFunction
    {
        public string Name
        {
            get { return "ProbAorB"; }
        }

        public string Category
        {
            get { return "Probability"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            double a = Convert.ToDouble(arguments[0]);
            double b = Convert.ToDouble(arguments[1]);
            ProbHelper.isValidValue(a);
            ProbHelper.isValidValue(b);
            return a + b;
        }
    }

    internal class ProbAandB : IFunction
    {
        public string Name
        {
            get { return "ProbAandB"; }
        }

        public string Category
        {
            get { return "Probability"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            double a = Convert.ToDouble(arguments[0]);
            double b = Convert.ToDouble(arguments[1]);
            ProbHelper.isValidValue(a);
            ProbHelper.isValidValue(b);
            return a * b;
        }
    }

    internal class ProbAgivenB : IFunction
    {
        public string Name
        {
            get { return "ProbAgivenB"; }
        }

        public string Category
        {
            get { return "Probability"; }
        }

        public int ParamCount
        {
            get { return 2; }
        }

        public object Run(params object[] arguments)
        {
            double a = Convert.ToDouble(arguments[0]);
            double b = Convert.ToDouble(arguments[1]);
            ProbHelper.isValidValue(a);
            ProbHelper.isValidValue(b);
            return (((b * a) / a) * a) / b;
        }
    }
}
