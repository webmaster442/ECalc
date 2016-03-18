using ECalc.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        const string operators = @"(\+)|(\-)|(\÷)|(\×)|(\*)|(\/)|(\()|(\))|(\;)|(~)|(mod)|(and)|(or)|(not)|(xor)|(eq)|(shr)|(shl)|(ror)|(rol)";
        const string exponentfix = @"[eE]\s+([+-~])\s+";

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
            _prefixes = new PrefixDictionary();

            _userfunctions.Add(new UserFuntion()
            {
                Name = "Test",
                ArgCount = 2,
                Commands = "$arg1 - $arg2"
            });

            Ans = 0.0d; //double
        }

        public static bool IsOperator(string s)
        {
            return Regex.IsMatch(s, operators);
        }

        private bool IsFunction(string s)
        {
            var q = (from f in _functions where
                     string.Compare(f.Name, s, StringComparison.CurrentCultureIgnoreCase) == 0
                     select f).FirstOrDefault();
            return q != null;
        }

        private bool IsUserFunction(string s)
        {
            var q = (from f in _userfunctions where
                     string.Compare(f.Name, s, StringComparison.CurrentCultureIgnoreCase) == 0
                     select f).FirstOrDefault();
            return q != null;
        }

        /// <summary>
        /// Compiles expression to reverse polish notation
        /// </summary>
        /// <param name="expression">Expression to compile</param>
        /// <returns>a Que containing token items</returns>
        public Queue<Token> CompileToRpn(string expression)
        {
            var good = expression.Trim();

            good = Regex.Replace(good, operators, " $0 ");

            good = Regex.Replace(good, exponentfix, "e$1").Trim();

            good = Regex.Replace(good, "- ", "MINUS ");
            good = Regex.Replace(good, @"(?<number1>(\d+([\.\,]\d+)?)|(\)))\s+MINUS", "${number1} -");
            // Step 3. Use the tilde ~ as the unary minus operator
            good = Regex.Replace(good, "MINUS", "~");

            var items = from i in good.Split(' ') where string.IsNullOrEmpty(i) == false select i;

            Queue<Token> Output = new Queue<Token>();
            Stack<Token> Stack = new Stack<Token>();
            Token temp = null;
            Token opstoken = null;

            var negative = false;

            foreach (var token in items)
            {
                var c = token.Trim();
                switch (c)
                {
                    case "+":
                    case "-":
                    case "÷":
                    case "/":
                    case "×":
                    case "*":
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
                    case "ror":
                    case "rol":
                    case "shr":
                    case "shl":
                    case "not":
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
                        negative = true;
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
                        else if (IsUserFunction(c))
                        {
                            temp = new Token(TokenType.UserFunction, c);
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
                                    if (negative)
                                    {
                                        val *= -1;
                                        negative = false;
                                    }
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

            if (negative) throw new ArgumentException("Negative error");

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
                sb.Append(" abs: ");
                sb.Append(cplx.Magnitude);
            }
            else if (Engine.Ans is double)
            {
                double d = (double)Engine.Ans;
                if (Engine.PreferPrefixes)
                {
                    sb.Append(_prefixes.DivideToPrefix(d));
                }
                else
                {
                    if ((d - Math.Truncate(d)) == 0)
                    {
                        BigInteger a = (BigInteger)d;
                        sb.Append(a);
                    }
                    else sb.Append(d);
                }
            }
            else sb.Append(Engine.Ans.ToString());

            if (HadOwerFlow) return string.Format("{0} (Overflowed)", sb.ToString());
            else return sb.ToString();
        }

        /// <summary>
        /// Gets the currently available function names
        /// </summary>
        public IFunction[] Functions
        {
            get { return _functions.ToArray(); }
        }

        private object RunUserFunction(string name, Stack<object> result)
        {
            var fnc = (from i in _userfunctions where i.Name == name select i).FirstOrDefault();
            List<object> args = new List<object>();
            if (result.Count < fnc.ArgCount) throw new ArgumentException("Too few parameters for function");
            else if (result.Count > (fnc.ArgCount + 1)) throw new ArgumentException("Too many parameters for function");
            var array = result.ToArray();
            for (int j = fnc.ArgCount -1; j >=0; j--)
            {
                result.Pop();
                MemoryManager.PushTemp(array[j]);
            }

            var lines = fnc.Commands.Split('\r', '\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                Evaluate(line);
            }

            return Ans;
        }

        /// <summary>
        /// Evaluates an RPN expression
        /// </summary>
        /// <param name="que">An RPN expression. To get this call CompileToRpn</param>
        public void Evaluate(Queue<Token> que)
        {
            HadOwerFlow = false; //reset overflow
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
                        if (token.Content == "not")
                        {
                            if (result.Count >= 1)
                            {
                                object op1 = result.Pop();
                                object r = HandleBinOperators(op1, 0.0d, token.Content);
                                result.Push(Convert.ToDouble(r));
                            }
                            else throw new ArgumentException("Evaluation error at: " + token.Content);
                        }
                        else if (result.Count >= 2)
                        {
                            object op2 = result.Pop();
                            object op1 = result.Pop();
                            object r = HandleBinOperators(op1, op2, token.Content);
                            result.Push(Convert.ToDouble(r));
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
                    case TokenType.UserFunction:
                        object user = RunUserFunction(token.Content, result);
                        result.Push(user);
                        break;
                }
            }

            if (result.Count == 1)
            {
                Ans = result.Pop();
            }
            else throw new ArgumentException("Evalutation Error!");
        }

        /// <summary>
        /// Evaluates an expression
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Evaluated Expression</returns>
        public string Evaluate(string expression)
        {
            var que = CompileToRpn(expression);
            Evaluate(que);
            return ResultToString();
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
