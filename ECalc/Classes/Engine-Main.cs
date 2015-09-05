using ECalc.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECalc.Classes
{
    /// <summary>
    /// Evaluator Engine
    /// </summary>
    internal partial class Engine
    {

        const string operators = @"(\+)|(\-)|(\÷)|(X)|(\()|(\))|(\;)|(~)|(mod)|(and)|(or)|(not)|(xor)|(eq)";

        private List<IFunction> _functions;
        private PrefixDictionary _prefixes;

        /// <summary>
        /// Memory & constant manager object
        /// </summary>
        public IMemManager MemoryManager
        {
            get;
            set;
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
                    case "and":
                    case "or":
                    case "xor":
                    case "eq":
                        temp = new Token(TokenType.BitOperator, c);
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
        /// Converts the ANS value to displayable string
        /// </summary>
        /// <returns>Engine.ANS as formatted string</returns>
        private string ResultToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Engine.Ans is Complex)
            {
                Complex cplx = (Complex)Engine.Ans;
                sb.Append("R: ");
                sb.Append(cplx.Real);
                sb.Append(" i: ");
                sb.Append(cplx.Imaginary);
                sb.Append(" φ: ");
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
            }
            else if (Engine.Ans is double)
            {
                if (Engine.PreferPrefixes)
                {
                    sb.Append(_prefixes.DivideToPrefix((double)Engine.Ans));
                }
                else sb.Append(Engine.Ans.ToString());
            }
            else sb.Append(Engine.Ans.ToString());

            if (HadOwerFlow) return string.Format("{0} (Overflowed)", sb.ToString());
            else return sb.ToString();
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
            HadOwerFlow = false; //reset overflow
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
                    case TokenType.BitOperator:
                        if (result.Count >= 2)
                        {
                            object op2 = result.Pop();
                            object op1 = result.Pop();
                            object r = HandleBinOperators(op1, op2, token.Content);
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
                return ResultToString();
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
