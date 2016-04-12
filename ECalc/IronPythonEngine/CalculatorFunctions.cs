using ECalc.IronPythonEngine.Types;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ECalc.IronPythonEngine
{
    [Loadable]
    public class CalculatorFunctions
    {
        public static IMemManager MemoryManager { get; set; }

        [Category("Calculator")]
        public static dynamic Var(string name)
        {
            object o;

            if (name == "ans") o = Engine.Ans;
            else o = MemoryManager.GetItem(name);

            if (o == null) throw new ArgumentException("Variable not found");
            else if (o is Complex) return (Complex)o;
            else if (o is Vector) return (Vector)o;
            else if (o is Matrix) return (Matrix)o;
            else if (o is Fraction) return (Fraction)o;
            else
            {
                try
                {
                    return Convert.ToDouble(o);
                }
                catch (Exception) { throw new ArgumentException("Invalid type parameter", nameof(name)); }
            }
                
        }

        [Category("Calculator")]
        public static void Var(string name, object value)
        {
            MemoryManager.SetItem(name, value);
        }

        [Category("Calculator")]
        public static void Var(string destination, string source)
        {
            var s = Var(source);
            MemoryManager.SetItem(destination, s);
        }

        [Category("Calculator")]
        public static string FncList()
        {
            StringBuilder buffer = new StringBuilder();
            var names = (from i in Engine.Functions orderby i.Name select i.Name).Distinct();
            foreach (var name in names)
            {
                buffer.Append(name);
                buffer.Append("\r\n");
            }
            return buffer.ToString();
        }

        [Category("Calculator")]
        public static void RegFunction(string name)
        {
            var exists = from i in Engine.Functions where i.Category == "User" && i.Name == name select i;

            if (exists.Count() == 0)
            {
                Engine.Functions.Add(new FunctionInfo
                {
                    Name = name,
                    FullName = name,
                    Category = "User"
                });
            }
        }
    }
}
