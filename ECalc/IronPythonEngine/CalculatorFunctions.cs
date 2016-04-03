using ECalc.Classes;
using ECalc.Maths;
using System;
using System.Numerics;

namespace ECalc.IronPythonEngine
{
    [Loadable]
    public class CalculatorFunctions
    {
        public static IMemManager MemoryManager { get; set; }

        [Category("Calculator")]
        public static dynamic Var(string name)
        {
            var o = MemoryManager.GetItem(name);
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
    }
}
