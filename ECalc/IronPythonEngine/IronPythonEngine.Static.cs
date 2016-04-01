using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.IronPythonEngine
{
    internal partial class Engine
    {
        private static List<IFunction> _functions;
        private static PrefixDictionary _prefixes;
        private static readonly string[] _operators;

        static Engine()
        {
            _functions = new List<IFunction>();
            _prefixes = new PrefixDictionary();
            _operators = new string[] { "+", "-", "*", "/", "×", "÷" };
            try
            {
                var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where
                        t.IsClass &&
                        t.Namespace == "ECalc.Maths" &&
                        t.GetInterfaces().Contains(typeof(IFunction))
                        select t;

                foreach (var item in q)
                {
                    var fnc = (IFunction)Activator.CreateInstance(item);
                    _functions.Add(fnc);
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        public static TrigMode Mode
        {
            get;
            set;
        }

        public static object Ans
        {
            get;
            private set;
        }

        public static IFunction[] Functions
        {
            get { return _functions.ToArray(); }
        }

        public static bool IsOperator(string s)
        {
            return _operators.Contains(s);
        }
    }
}
