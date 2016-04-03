using ECalc.Classes;
using ECalc.Maths;
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime.Types;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECalc.IronPythonEngine
{
    internal delegate object Pyf(params object[] arguments);

    internal partial class Engine
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private Dictionary<string, string> _functioncache;
        private IMemManager _mem;

        public Engine()
        {
            Dictionary<String, Object> options = new Dictionary<string, object>();
            options["DivisionOptions"] = PythonDivisionOptions.New;
            _engine = Python.CreateEngine(options);
            _scope = _engine.CreateScope();
            foreach (var t in _pluggable)
            {
                _scope.SetVariable(t.Name, DynamicHelpers.GetPythonTypeFromType(t));
            }
            _scope.SetVariable("CalculatorFunctions", DynamicHelpers.GetPythonTypeFromType(typeof(CalculatorFunctions)));
            _functioncache = new Dictionary<string, string>();
            _functioncache.Add("Var", "CalculatorFunctions.Var");
            foreach (var f in _functions)
            {
                if (_functioncache.ContainsKey(f.Name)) continue;
                _functioncache.Add(f.Name, f.FullName);
            }
        }

        public IMemManager MemoryManager
        {
            get { return _mem; }
            set
            {
                _mem = value;
                CalculatorFunctions.MemoryManager = value;
            }
        }

        private bool ParseNumber(out string parsed, string c)
        {
            if (c.StartsWith("H#"))
            {
                parsed = Convert.ToInt64(c.Replace("H#", ""), 16).ToString();
                return true;
            }
            else if (c.StartsWith("O#"))
            {
                parsed = Convert.ToInt64(c.Replace("O#", ""), 8).ToString();
                return true;
            }
            else if (c.StartsWith("B#"))
            {
                parsed = Convert.ToInt64(c.Replace("B#", ""), 2).ToString();
                return true;
            }
            else if (c.StartsWith("R#"))
            {
                parsed = NumberSystemConversions.RomanToInt(c.Replace("R#", "")).ToString();
                return true;
            }
            else
            {
                foreach (var item in _prefixes)
                {
                    if (c.EndsWith(item.Key))
                    {
                        string num = c.Replace(item.Key, "");
                        double n = Convert.ToDouble(num);
                        n *= item.Value;
                        parsed = n.ToString();
                        return true;
                    }
                }
                try
                {
                    var num = Convert.ToDouble(c).ToString();
                    parsed = num;
                    return true;
                }
                catch (Exception)
                {
                    parsed = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Pre process calculator raw input
        /// </summary>
        /// <param name="input">raw input</param>
        /// <returns>executable python code</returns>
        public string PreProcess(string input)
        {
            var parts = Regex.Split(input, @"(\+)|(\*)|(\()|(\))|(\×)|(\×)|(\÷)|(\%)");
            string temp;
            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(parts[i])) continue;

                parts[i] = parts[i].Trim();

                if (parts[i] == "×") parts[i] = "*";
                else if (parts[i] == "÷") parts[i] = "/";
                else if (parts[i].StartsWith("&")) parts[i] = ConstantDB.Lookup(parts[i]).ToString(CultureInfo.InvariantCulture);
                else if (_functioncache.ContainsKey(parts[i])) parts[i] = _functioncache[parts[i]];
                else
                {
                    var result = ParseNumber(out temp, parts[i]);
                    if (result) parts[i] = temp;
                }
            }
            StringBuilder processed = new StringBuilder();
            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(parts[i])) continue;
                processed.Append(parts[i]);
                if (i != parts.Length - 1) processed.Append(" ");
            }
            return processed.ToString();
        }

        private string FormatDouble(double input)
        {
            string gchar = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            string fchar = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (double.IsNaN(input) || double.IsInfinity(input)) return input.ToString(CultureInfo.CurrentCulture);
            StringBuilder sb = new StringBuilder();
            bool passed = false;
            int j = 1;
            int i;
            char[] ar;
            string text = input.ToString();
            if (text.Contains(fchar))
            {
                for (i = text.Length - 1; i >= 0; i--)
                {
                    if (!passed && text[i] != fchar[0]) sb.Append(text[i]);
                    else if (text[i] == fchar[0])
                    {
                        sb.Append(text[i]);
                        passed = true;
                    }
                    if (passed && text[i] != fchar[0])
                    {
                        sb.Append(text[i]);
                        if (j % 3 == 0) sb.Append(gchar);
                        j++;
                    }
                }
                ar = sb.ToString().ToCharArray();
                Array.Reverse(ar);
                return new string(ar).Trim();
            }
            else
            {
                for (i = text.Length - 1; i >= 0; i--)
                {
                    sb.Append(text[i]);
                    if (j % 3 == 0) sb.Append(gchar);
                    j++;
                }
                ar = sb.ToString().ToCharArray();
                Array.Reverse(ar);
                return new string(ar).Trim();
            }
        }

        private string FormatComplex(Complex c)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Algebric: {0} + {1}*i\tAbs: {2}\tPhase: {3} rad\r\n", c.Real, c.Imaginary, c.Magnitude, c.Phase);
            sb.AppendFormat("Trigonometric: {0} * ({1} + i*{2})", c.Magnitude, Math.Cos(c.Phase), Math.Sin(c.Phase));
            return sb.ToString();
        }

        private string DisplayString(object o)
        {
            Type t = o.GetType();
            switch (t.Name)
            {
                case "Double":
                    return FormatDouble(Convert.ToDouble(o));
                case "Complex":
                    return FormatComplex((Complex)o);
                default:
                    if (t.IsArray)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Array of " + t.Name + " {\n");
                        foreach (object x in (object[])o)
                        {
                            sb.Append(x.ToString());
                            sb.Append("\n");
                        }
                        sb.Append("}");
                        return sb.ToString();
                    }
                    else if (o is IEnumerable)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Collection {\n");
                        IEnumerable coll = (IEnumerable)o;
                        foreach (var i in coll)
                        {
                            sb.Append(i.ToString());
                            sb.Append("\n");
                        }
                        sb.Append("}");
                        return sb.ToString();
                    }
                    else if ((o is IronPython.Runtime.PythonFunction) || (o is IronPython.Runtime.Types.BuiltinFunction))
                    {
                        return "This is a function.";
                    }
                    else return o.ToString();
            }
        }

        public Task<string> EvaluateAsync(string input)
        {
            return Task.Run(() =>
            {
                try
                {
                    var processed = PreProcess(input);
                    ScriptSource source = _engine.CreateScriptSourceFromString(processed, SourceCodeKind.AutoDetect);
                    object result = source.Execute(_scope);
                    _scope.SetVariable("ans", result);
                    if (result != null)
                    {
                        Engine.Ans = result;
                        return DisplayString(result);
                    }
                    else return null;
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorDialog(ex.Message);
                    return null;
                }
            });
        }
    }
}
