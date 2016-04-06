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
    internal partial class Engine: IDisposable
    {
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private Dictionary<string, string> _functioncache;
        private IMemManager _mem;
        private EventRaisingStreamWriter _output;
        private NullStream _history;

        public Engine()
        {
            Dictionary<String, Object> options = new Dictionary<string, object>();
            options["DivisionOptions"] = PythonDivisionOptions.New;
            _history = new NullStream();
            _output = new EventRaisingStreamWriter(_history);
            _output.StringWritten += _output_StringWritten;
            _engine = Python.CreateEngine(options);
            _engine.Runtime.IO.SetOutput(_history, _output);
            _engine.Runtime.IO.SetErrorOutput(_history, _output);
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

        public event EventHandler<MyEvtArgs<string>> StdOutWriten;

        private void _output_StringWritten(object sender, MyEvtArgs<string> e)
        {
            if (StdOutWriten != null) StdOutWriten(this, e);
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
            var parts = Regex.Split(input, @"(\+)|(\*)|(\()|(\))|(\×)|(\×)|(\÷)|(\%)|(\,)");
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
            if (PreferPrefixes)
            {
                PrefixDictionary pfx = new PrefixDictionary();
                return pfx.DivideToPrefix(input);
            }
            if (GroupByThousands)
            {
                string gchar = " ";
                string fchar = ".";
                if (double.IsNaN(input) || double.IsInfinity(input)) return input.ToString(CultureInfo.InvariantCulture);
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
            else return input.ToString(CultureInfo.InvariantCulture);
        }

        private string FormatComplex(Complex cplx)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("R: ");
            sb.Append(cplx.Real);
            sb.Append(" i: ");
            sb.Append(cplx.Imaginary);
            sb.Append("\r\n φ: ");
            switch (Engine.Mode)
            {
                case TrigMode.DEG:
                    sb.Append(TrigFunctions.Rad2Deg(cplx.Phase));
                    sb.Append(" °");
                    break;
                case TrigMode.GRAD:
                    sb.Append(TrigFunctions.Rad2Grad(cplx.Phase));
                    sb.Append(" grad");
                    break;
                case TrigMode.RAD:
                    sb.Append(cplx.Phase);
                    sb.Append(" rad");
                    break;
            }
            sb.Append(" ABS: ");
            sb.Append(cplx.Magnitude);
            return sb.ToString();
        }

        private string FormatEnumerable(object o)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable coll = (IEnumerable)o;

            int idx = 0;
            if (o is Array || o is IList)
            {
                foreach (var i in coll)
                {
                    sb.AppendFormat("{0} =>\n", idx);
                    if (i is Complex) sb.Append(FormatComplex((Complex)i));
                    else sb.Append(i.ToString());
                    sb.Append("\n");
                    ++idx;
                }
            }
            else
            {
                foreach (var i in coll)
                {
                    if (i is Complex) sb.Append(FormatComplex((Complex)i));
                    else sb.Append(i.ToString());
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }

        private string DisplayString(object o)
        {
            Type t = o.GetType();
            switch (t.Name)
            {
                case "Double":
                case "Single":
                case "Int32":
                case "Int16":
                case "Byte":
                case "SByte":
                case "UInt32":
                case "UInt64":
                    return FormatDouble(Convert.ToDouble(o));
                case "Complex":
                    return FormatComplex((Complex)o);
                default:
                    if (o is IEnumerable) return FormatEnumerable(o);
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
                    if (result != null)
                    {
                        _scope.SetVariable("ans", result);
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

        public CompiledCode Compile(string input)
        {
            try
            {
                var processed = PreProcess(input);
                ScriptSource source = _engine.CreateScriptSourceFromString(processed, SourceCodeKind.AutoDetect);
                return source.Compile();
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
                return null;
            }
        }

        public Task<double> EvaluateCompiled(CompiledCode c)
        {
            return Task.Run(() =>
            {
                try
                {
                    object result = c.Execute(_scope);
                    return (double)result;
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorDialog(ex.Message);
                    return double.NaN;
                }
            });
        }

        protected virtual void Dispose(bool native)
        {
            if (_output != null)
            {
                _output.Dispose();
                _output = null;
            }
            if (_history != null)
            {
                _history.Dispose();
                _history = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
