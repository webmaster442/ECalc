using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Maths
{
    public class Rad2Deg: IFunction
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

    public class Rad2Grad: IFunction
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

    public class Sin: IFunction
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
}
