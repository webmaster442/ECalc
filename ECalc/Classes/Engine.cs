using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Reflection;

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
    public class Engine
    {

        const string operators = @"(\+)|(\-)|(\÷)|(X)|(\()|(\))|(\;)|(~)|(mod)";

        private List<IFunction> _functions;
        private Dictionary<string, double> _prefixes;

        public static bool IsOperator(string s)
        {
            return Regex.IsMatch(s, operators);
        }

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

        public IMemManager MemoryManager
        {
            get;
            set;
        }

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

            _prefixes = new Dictionary<string, double>();
            _prefixes.Add("da", Math.Pow(10, 1));
            _prefixes.Add("y", Math.Pow(10, -24));
            _prefixes.Add("z", Math.Pow(10, -21));
            _prefixes.Add("f", Math.Pow(10, -18));
            _prefixes.Add("n", Math.Pow(10, -9));
            _prefixes.Add("u", Math.Pow(10, -6));
            _prefixes.Add("m", Math.Pow(10, -3));
            _prefixes.Add("c", Math.Pow(10, -2));
            _prefixes.Add("d", Math.Pow(10, -1));
            _prefixes.Add("a", Math.Pow(10, -18));
            _prefixes.Add("h", Math.Pow(10, 2));
            _prefixes.Add("k", Math.Pow(10, 3));
            _prefixes.Add("M", Math.Pow(10, 6));
            _prefixes.Add("G", Math.Pow(10, 9));
            _prefixes.Add("T", Math.Pow(10, 12));
            _prefixes.Add("P", Math.Pow(10, 15));
            _prefixes.Add("E", Math.Pow(10, 18));
            _prefixes.Add("Z", Math.Pow(10, 21));
            _prefixes.Add("Y", Math.Pow(10, 24));

            Ans = 0.0d; //double

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
                            double n1, n2;
                            Complex a, b;
                            object r = null;

                            if (Helpers.IsComplex(op1) || Helpers.IsComplex(op2))
                            {
                                a = Helpers.GetComplex(op1);
                                b = Helpers.GetComplex(op2);
                                switch (token.Content)
                                {
                                    case "+":
                                        r = a + b;
                                        break;
                                    case "-":
                                        r = a - b;
                                        break;
                                    case "÷":
                                        r = a / b;
                                        break;
                                    case "X":
                                        r = a * b;
                                        break;
                                    case "mod":
                                        r = new Complex(a.Real % b.Real, a.Imaginary % b.Imaginary);
                                        break;
                                }
                            }
                            else
                            {
                                n1 = (double)op1;
                                n2 = (double)op2;
                                switch (token.Content)
                                {
                                    case "+":
                                        r = n1 + n2;
                                        break;
                                    case "-":
                                        r = n1 - n2;
                                        break;
                                    case "÷":
                                        r = n1 / n2;
                                        break;
                                    case "X":
                                        r = n1 * n2;
                                        break;
                                    case "mod":
                                        r = n1 % n2;
                                        break;
                                }
                            }
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
