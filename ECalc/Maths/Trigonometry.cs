using ECalc.Classes;
using System;

namespace ECalc.Maths
{

    #region Conversion functions
    public class Rad2Deg : IFunction
    {
        public string Name
        {
            get { return "Rad2Deg"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Rad2Deg(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Deg2Rad : IFunction
    {
        public string Name
        {
            get { return "Deg2Rad"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Deg2Rad(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Grad2Deg : IFunction
    {
        public string Name
        {
            get { return "Grad2Deg"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Grad2Deg(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Deg2Grad : IFunction
    {
        public string Name
        {
            get { return "Deg2Grad"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Deg2Grad(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Rad2Grad : IFunction
    {
        public string Name
        {
            get { return "Rad2Grad"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Rad2Grad(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Grad2Rad : IFunction
    {
        public string Name
        {
            get { return "Grad2Rad"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Grad2Rad(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    #endregion

    #region Trigonometrical functions
    public class Sin : IFunction
    {
        public string Name
        {
            get { return "Sin"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Sin(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Cos : IFunction
    {
        public string Name
        {
            get { return "Cos"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Cos(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Tan : IFunction
    {
        public string Name
        {
            get { return "Tan"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Tan(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Ctg : IFunction
    {
        public string Name
        {
            get { return "Ctg"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Ctg(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Sec : IFunction
    {
        public string Name
        {
            get { return "Sec"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Sec(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class Cosec : IFunction
    {
        public string Name
        {
            get { return "Cosec"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.Cosec(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }
    #endregion

    #region Inverse Trigonometrical Functions
    public class ArcSin : IFunction
    {
        public string Name
        {
            get { return "ArcSin"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.ArcSin(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class ArcCos : IFunction
    {
        public string Name
        {
            get { return "ArcCos"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.ArcCos(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class ArcTan : IFunction
    {
        public string Name
        {
            get { return "ArcTan"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.ArcTan(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class ArcCtg : IFunction
    {
        public string Name
        {
            get { return "ArcCtg"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.ArcCtg(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class ArcSec : IFunction
    {
        public string Name
        {
            get { return "ArcSec"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.ArcSec(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    public class ArcCosec : IFunction
    {
        public string Name
        {
            get { return "ArcCosec"; }
        }

        public object Run(params object[] arguments)
        {
            double d = Convert.ToDouble(arguments[0]);
            return TrigFunctions.ArcCosec(d);
        }

        public int ParamCount
        {
            get { return 1; }
        }
    }

    #endregion
}
