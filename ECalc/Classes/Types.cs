using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECalc.Classes
{
    /// <summary>
    /// Event Handler for string events
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">parameters</param>
    internal delegate void StringEventHandler(object sender, StringEventArgs e);

    /// <summary>
    /// String Event Args
    /// </summary>
    internal class StringEventArgs: RoutedEventArgs
    {
        public StringEventArgs() : base() { }
        
        public StringEventArgs(string param): base()
        {
            Text = param;
        }

        /// <summary>
        /// Parameter String
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Function Interface
    /// </summary>
    internal interface IFunction
    {
        /// <summary>
        /// Function name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Function code
        /// </summary>
        /// <param name="arguments">function arguments</param>
        /// <returns>a result object</returns>
        object Run(params object[] arguments);

        /// <summary>
        /// Argument count
        /// </summary>
        int ParamCount { get; }
    }

    /// <summary>
    /// Used in memory management
    /// </summary>
    internal class MemoryItem
    {
        /// <summary>
        /// Register counter
        /// </summary>
        private static int Counter;

        public static void ResetCounter()
        {
            Counter = 0;
        }

        static MemoryItem()
        {
            Counter = 0;
        }

        /// <summary>
        /// Variable name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Variable value
        /// </summary>
        public object Value { get; set; }

        public MemoryItem()
        {
            Name = String.Format("$Reg_{0}", Counter);
            Value = null;
            Counter++;
        }

        public MemoryItem(object val)
        {
            Name = String.Format("$Reg_{0}", Counter);
            Value = val;
            Counter++;
        }

        public MemoryItem(string name, object val)
        {
            Value = val;
            Name = name;
        }
    }

    /// <summary>
    /// Trigonometry modes
    /// </summary>
    internal enum TrigMode
    {
        DEG, RAD, GRAD
    }

    /// <summary>
    /// Unit Converter actions
    /// </summary>
    internal enum Actions
    {
        None,
        Add,
        Multiply,
        Divide,
        Subtract
    }

    /// <summary>
    /// Bit engine mode enum
    /// </summary>
    internal enum BitEngineModes
    {
        Signed8bit,
        Signed16bit,
        Signed32bit,
        Signed64bit,
        Unsigned8bit,
        Unsigned16bit,
        Unsigned32bit,
        Unsigned64bit
    }

    internal enum ConstantCategory
    {
        Mathematical,
        Universal,
        ElectroMagnetic,
        Atomic
    }

    /// <summary>
    /// Memory manager interface
    /// </summary>
    public interface IMemManager
    {
        object GetItem(string name);
    }

    /// <summary>
    /// Unit conversion base type
    /// </summary>
    internal class Unit
    {
        /// <summary>
        /// Unit name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Conversion ratio compared to the base unit
        /// </summary>
        public double Ratio { get; set; }

        /// <summary>
        /// Action to do
        /// </summary>
        public Actions Action { get; set; }

        /// <summary>
        /// Offset value to add or subtract
        /// </summary>
        public double offset { get; set; }

        /// <summary>
        /// Creates a new instance of unit
        /// </summary>
        public Unit() { }

        /// <summary>
        /// Creates a new instance of unit
        /// </summary>
        /// <param name="Name">Unit name</param>
        /// <param name="Ratio">Conversion ratio compared to the base unit</param>
        /// <param name="Action">Action to do</param>
        /// <param name="offset">Offset value to add or subtract</param>
        public Unit(string Name, double Ratio = 1, Actions Action = Actions.None, double offset = 0)
        {
            this.Name = Name;
            this.Ratio = Ratio;
            this.Action = Action;
            this.offset = offset;
        }
    }

    /// <summary>
    /// Token types for expression evaluator
    /// </summary>
    internal enum TokenType
    {
        Number,
        Operator,
        Function,
        LeftB,
        RightB,
        Seperator,
        UnaryMinus,
        Variable,
        Constant,
        BitOperator
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
}
