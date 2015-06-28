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
    public delegate void StringEventHandler(object sender, StringEventArgs e);

    /// <summary>
    /// String Event Args
    /// </summary>
    public class StringEventArgs: RoutedEventArgs
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
    public interface IFunction
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

    [Serializable]
    public class UsageInfo
    {
        public string Name { get; set; }
        public uint Count { get; set; }
    }

    /// <summary>
    /// Used in memory management
    /// </summary>
    public class MemoryItem
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
}
