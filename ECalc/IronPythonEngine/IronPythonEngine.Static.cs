using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECalc.IronPythonEngine
{
    internal partial class Engine: IDisposable
    {
        private static PrefixDictionary _prefixes;
        private static readonly string[] _operators;
        private static List<FunctionInfo> _functions;
        private static List<Type> _pluggable;

        static Engine()
        {
            _prefixes = new PrefixDictionary();
            _operators = new string[] { "+", "*", "/", "×", "÷", "(", ")", "%", "," };
            _functions = new List<FunctionInfo>();
            _pluggable = new List<Type>();
            try
            {
                ReflectLoad("ECalc.Maths");
            }
            catch (Exception ex) { MainWindow.ErrorDialog(ex.Message); }
        }

        public static void ReflectLoad(string ns)
        {
            var classes = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where
                          t.IsClass &&
                          t.Namespace == ns &&
                          Attribute.IsDefined(t, typeof(LoadableAttribute))
                          select t;

            foreach (var t in classes) _pluggable.Add(t);

            var methods = from type in classes select type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var goodmethods = from m in methods from j in m where Attribute.IsDefined(j, typeof(CategoryAttribute)) select j;

            foreach (var method in goodmethods)
            {
                var category = method.GetCustomAttribute<CategoryAttribute>();
                var f = new FunctionInfo
                {
                    FullName = string.Format("{0}.{1}", method.ReflectedType.Name, method.Name),
                    Category = category.Category,
                    Name = method.Name
                };
                _functions.Add(f);
            }

        }

        public static TrigMode Mode
        {
            get;
            set;
        }

        public static bool PreferPrefixes
        {
            get; set;
        }

        public static bool GroupByThousands
        {
            get; set;
        }

        public static object Ans
        {
            get;
            private set;
        }

        public static FunctionInfo[] Functions
        {
            get { return _functions.ToArray(); }
        }

        public static bool IsOperator(string s)
        {
            return _operators.Contains(s);
        }
    }
}
