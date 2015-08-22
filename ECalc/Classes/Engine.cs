using ECalc.Maths;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECalc.Classes
{
    public interface IMemManager
    {
        object GetItem(string name);
    }

    /// <summary>
    /// Token types for expression evaluator
    /// </summary>
    internal enum TokenType
    {
        Number, Operator, Function, LeftB, RightB, Seperator, UnaryMinus, Variable, Constant
    }

    /// <summary>
    /// Token Class for expression evaluator
    /// </summary>
    internal class Token
    {
        /// <summary>
        /// Token type
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// Token content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Creates a new instance of Token
        /// </summary>
        /// <param name="T">Token type</param>
        /// <param name="C">Token content</param>
        public Token(TokenType T, string C)
        {
            Type = T;
            Content = C;
        }

        public override string ToString()
        {
            return string.Format("{0}, Type: {1}", Content, Type);
        }
    }

    /// <summary>
    /// Evaluator Engine
    /// </summary>
    internal class Engine
    {

        const string operators = @"(\+)|(\-)|(\÷)|(X)|(\()|(\))|(\;)|(~)|(mod)";

        private List<IFunction> _functions;
        private PrefixDictionary _prefixes;

        /// <summary>
        /// Trigonometry Mode
        /// </summary>
        public static TrigMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// Last calculation result as object
        /// </summary>
        public static Object Ans
        {
            get;
            private set;
        }

        /// <summary>
        /// Memory & constant manager object
        /// </summary>
        public IMemManager MemoryManager
        {
            get;
            set;
        }

        /// <summary>
        /// Decimal sepperator char on the current culture
        /// </summary>
        public static string DecimalSeperator
        {
            get;
            private set;
        }

        /// <summary>
        /// Number group sepperator
        /// </summary>
        public static string NumberGroupSeparator
        {
            get;
            private set;
        }

        /// <summary>
        /// Static ctor
        /// </summary>
        static Engine()
        {
            DecimalSeperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
        }

        /// <summary>
        /// Creates a new instance of engine
        /// </summary>
        public Engine()
        {
            _functions = new List<IFunction>();

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
                Helpers.ErrorDialog(ex);
            }

            _prefixes = new PrefixDictionary();

            Ans = 0.0d; //double

        }

        public static bool IsOperator(string s)
        {
            return Regex.IsMatch(s, operators);
        }

        private bool IsFunction(string s)
        {
            var q = from f in _functions where string.Compare(f.Name, s, StringComparison.CurrentCultureIgnoreCase) == 0 select f;
            return q.Count() > 0;
        }

        /// <summary>
        /// Compiles expression to reverse polish notation
        /// </summary>
        /// <param name="expression">Expression to compile</param>
        /// <returns>a Que containing token items</returns>
        private Queue<Token> CompileToRpn(string expression)
        {
            var good = expression.Trim();

            good = Regex.Replace(good, @"(?<number>^[^$]([\da-fA-F])+(\,\d+)?[boh]?$)", " ${number} ");

            good = Regex.Replace(good, "-", "MINUS");
            // Step 2. Looking for pi or e or generic number \d+(\.\d+)?
            //^\$[a-z1-9]+
            good = Regex.Replace(good, @"(?<number1>(\$[a-z1-9_-]+|\)|(\@[a-z1-9_-]+\[\d\])|(\#[a-z1-9_-]+\[\d\;\d\])|(\d+(\.\d+)?)))\s+MINUS", "${number1} -");
            // Step 3. Use the tilde ~ as the unary minus operator
            good = Regex.Replace(good, "MINUS", "~");

            var items = Regex.Split(good, operators);

            Queue<Token> Output = new Queue<Token>();
            Stack<Token> Stack = new Stack<Token>();
            Token temp = null;
            Token opstoken = null;

            foreach (var token in items)
            {
                if (string.IsNullOrWhiteSpace(token)) continue;
                var c = token.Trim();
                switch (c)
                {
                    case "+":
                    case "-":
                    case "÷":
                    case "X":
                    case "mod":
                        temp = new Token(TokenType.Operator, c);
                        if (Stack.Count > 0)
                        {
                            opstoken = Stack.Peek();
                            while (opstoken.Type == TokenType.Operator)
                            {
                                Output.Enqueue(Stack.Pop());
                                if (Stack.Count > 0) opstoken = Stack.Peek();
                                else break;
                            }
                        }
                        Stack.Push(temp);
                        break;
                    case "~":
                        temp = new Token(TokenType.UnaryMinus, c);
                        Stack.Push(temp);
                        break;
                    case "(":
                        temp = new Token(TokenType.LeftB, c);
                        Stack.Push(temp);
                        break;
                    case ")":
                        temp = new Token(TokenType.RightB, c);
                        if (Stack.Count > 0)
                        {
                            opstoken = Stack.Peek();
                            while (opstoken.Type != TokenType.LeftB)
                            {
                                Output.Enqueue(Stack.Pop());
                                if (Stack.Count > 0) opstoken = Stack.Peek();
                                else throw new ArgumentException("Unbalanced () parenthesis!");
                            }
                            Stack.Pop();
                        }
                        if (Stack.Count > 0)
                        {
                            opstoken = Stack.Peek();
                            if (opstoken.Type == TokenType.Function) Output.Enqueue(Stack.Pop());
                        }
                        break;
                    case ";":
                        temp = new Token(TokenType.Seperator, c);
                        Stack.Push(temp);
                        break;
                    default:
                        if (IsFunction(c))
                        {
                            temp = new Token(TokenType.Function, c);
                            Stack.Push(temp);
                        }
                        else
                        {
                            if (c.StartsWith("$"))
                            {
                                temp = new Token(TokenType.Variable, c);
                                Output.Enqueue(temp);
                            }
                            else if (c.StartsWith("&"))
                            {
                                temp = new Token(TokenType.Constant, c);
                                Output.Enqueue(temp);
                            }
                            else
                            {
                                try
                                {
                                    double val = ParseNumber(c);
                                    temp = new Token(TokenType.Number, val.ToString());
                                    Output.Enqueue(temp);
                                }
                                catch (Exception) { throw new ArgumentException("Can't parse " + c); }
                            }
                        }
                        break;
                }
            }

            while (Stack.Count != 0)
            {
                opstoken = Stack.Pop();
                if (opstoken.Type == TokenType.LeftB || opstoken.Type == TokenType.RightB)
                {
                    throw new ArgumentException("Unballanced ()");
                }
                else Output.Enqueue(opstoken);
            }

            return Output;
        }

        /// <summary>
        /// Parses a double number
        /// </summary>
        /// <param name="c">string to process</param>
        /// <returns>a double number</returns>
        private double ParseNumber(string c)
        {
            if (c.StartsWith("H#")) return Convert.ToInt64(c.Replace("H#", ""), 16);
            else if (c.StartsWith("O#")) return Convert.ToInt64(c.Replace("O#", ""), 8);
            else if (c.StartsWith("B#")) return Convert.ToInt64(c.Replace("B#", ""), 2);
            else if (c.StartsWith("R#")) return NumberSystemConversions.RomanToInt(c.Replace("R#", ""));
            else
            {
                foreach (var item in _prefixes)
                {
                    if (c.EndsWith(item.Key))
                    {
                        string num = c.Replace(item.Key, "");
                        double n = Convert.ToDouble(num);
                        n *= item.Value;
                        return n;
                    }
                }
                return Convert.ToDouble(c);
            }
        }

        /// <summary>
        /// Gets the currently available function names
        /// </summary>
        public string[] Functions
        {
            get
            {
                var q = from i in _functions select i.Name;
                return q.ToArray();
            }
        }

        /*  return type matrix:
            Type       CPLX      Fraction        Double
            CPX        CPLX      CPLX            CPLX
            Fraction   CPLX      Fraction        Fraction
            Double     CPLX      Fraction        Double  */
        /// <summary>
        /// Type matching operator handler function
        /// </summary>
        /// <param name="op1">operand1</param>
        /// <param name="op2">operand2</param>
        /// <param name="op">operation</param>
        /// <returns>result type</returns>
        public object HandleOperators(object op1, object op2, string op)
        {
            var t1 = op1.GetType().FullName;
            var t2 = op2.GetType().FullName;

            if (t1 == "System.Numerics.Complex" || t2 == "System.Numerics.Complex")
            {
                Complex a = new Complex();
                Complex b = new Complex();
                switch (t1)
                {
                    case "System.Numerics.Complex":
                        a = (Complex)op1;
                        break;
                    case "System.Double":
                        a = new Complex((double)op1, 0);
                        break;
                    case "ECalc.Classes.Fraction":
                       double d = ((Fraction)op1).ToDouble();
                        a = new Complex(d, 0);
                        break;
                }
                switch (t2)
                {
                    case "System.Numerics.Complex":
                        b = (Complex)op2;
                        break;
                    case "System.Double":
                        b = new Complex((double)op2, 0);
                        break;
                    case "ECalc.Classes.Fraction":
                        double d = ((Fraction)op2).ToDouble();
                        b = new Complex(d, 0);
                        break;
                }

                switch (op)
                {
                    case "+":
                        return a + b;
                    case "-":
                        return a - b;
                    case "÷":
                        return a / b;
                    case "X":
                        return a * b;
                    case "mod":
                        return new Complex(a.Real % b.Real, a.Imaginary % b.Imaginary);
                }
            }
            if (t1 == "ECalc.Classes.Fraction" || t2 == "ECalc.Classes.Fraction")
            {
                Fraction f1 = new Fraction();
                Fraction f2 = new Fraction();
                switch (t1)
                {
                    case "System.Double":
                        f1 = new Fraction((double)op1);
                        break;
                    case "ECalc.Classes.Fraction":
                        f1 = (Fraction)op1;
                        break;
                }
                switch (t2)
                {
                    case "System.Double":
                        f2 = new Fraction((double)op2);
                        break;
                    case "ECalc.Classes.Fraction":
                        f2 = (Fraction)op2;
                        break;
                }
                switch (op)
                {
                    case "+":
                        return f1 + f2;
                    case "-":
                        return f1 - f2;
                    case "÷":
                        return f1 / f2;
                    case "X":
                        return f1 * f2;
                    case "mod":
                        return new Fraction(f1.ToDouble() % f2.ToDouble());
                }
            }
            else
            {
                double n1 = (double)op1;
                double n2 = (double)op2;
                switch (op)
                {
                    case "+":
                        return n1 + n2;
                    case "-":
                        return n1 - n2;
                    case "÷":
                        return n1 / n2;
                    case "X":
                        return n1 * n2;
                    case "mod":
                        return n1 % n2;
                }
            }
            //default return
            return null;
        }


        /// <summary>
        /// Evaluates an expression
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Evaluated Expression</returns>
        public string Evaluate(string expression)
        {
            var que = CompileToRpn(expression);
            Stack<object> result = new Stack<object>();

            foreach (var token in que)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        result.Push(Convert.ToDouble(token.Content));
                        break;
                    case TokenType.UnaryMinus:
                        if (result.Count >= 1)
                        {
                            object min = result.Pop();
                            if (min is Complex) result.Push(((Complex)min) * -1);
                            result.Push(-(double)min);
                        }
                        break;
                    case TokenType.Variable:
                    case TokenType.Constant:
                        var content = MemoryManager.GetItem(token.Content);
                        result.Push(content);
                        break;
                    case TokenType.Operator:
                        if (result.Count >= 2)
                        {
                            object op2 = result.Pop();
                            object op1 = result.Pop();
                            object r = HandleOperators(op1, op2, token.Content);
                            result.Push(r);
                        }
                        else throw new ArgumentException("Evaluation error at: " + token.Content);
                        break;
                    case TokenType.Function:
                        var fnc = (from i in _functions where i.Name == token.Content select i).FirstOrDefault();
                        List<object> args = new List<object>();
                        if (result.Count < fnc.ParamCount) throw new ArgumentException("Too few parameters for function");
                        else if (result.Count > (fnc.ParamCount + 1)) throw new ArgumentException("Too many parameters for function");
                        for (int j = 0; j < fnc.ParamCount; j++) args.Add(result.Pop());
                        args.Reverse();
                        object o = fnc.Run(args.ToArray());
                        result.Push(o);
                        break;
                }
            }

            if (result.Count == 1)
            {
                Ans = result.Pop();
                return Helpers.ResultToString(Ans);
            }
            else throw new ArgumentException("Evalutation Error!");
        }

        /// <summary>
        /// Evaluates an expression async version
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Evaluated Expression</returns>
        public Task<string> EvaluateAsync(string input)
        {
            return Task<string>.Run(() =>
            {
                return Evaluate(input);
            });
        }
    }
}
