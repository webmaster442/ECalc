using ECalc.IronPythonEngine.Types;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ECalc.IronPythonEngine
{
    [Loadable]
    public static class CalculatorFunctions
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
            else if (o is Set) return (Set)o;
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
            var buffer = new StringBuilder();
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

        [Category("Calculator")]
        public static double BitOp(double d1, double d2, string op)
        {
            BitFunction f = BitFunction.AND;
            switch (op)
            {
                case "and":
                    f = BitFunction.AND;
                    break;
                case "or":
                    f = BitFunction.OR;
                    break;
                case "not":
                    f = BitFunction.NOT;
                    break;
                case "xor":
                    f = BitFunction.XOR;
                    break;
                case "shl":
                    f = BitFunction.SHL;
                    break;
                case "shr":
                    f = BitFunction.SHR;
                    break;
            }
            return BitOps.DoFunction(d1, d2, f);
        }
    }
}
