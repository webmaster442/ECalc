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
        public StringEventArgs()  { }
        
        public StringEventArgs(string param)
        {
            Text = param;
        }

        /// <summary>
        /// Parameter String
        /// </summary>
        public string Text { get; set; }
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
        Atomic,
        Favorites,
    }

    /// <summary>
    /// Memory manager interface
    /// </summary>
    public interface IMemManager
    {
        /// <summary>
        /// Gets the value of a register item
        /// </summary>
        /// <param name="name">item to get</param>
        /// <returns>the value of the item</returns>
        object GetItem(string name);

        /// <summary>
        /// Lists register names
        /// </summary>
        /// <param name="query">query string. If null or empty all registers will be returned</param>
        /// <returns>An array of register names</returns>
        string[] ListRegisters(string query);

        /// <summary>
        /// Set an item with name
        /// </summary>
        /// <param name="name">name of variable</param>
        /// <param name="value">value of variable</param>
        void SetItem(string name, object value);

        /// <summary>
        /// Set an item with default name
        /// </summary>
        /// <param name="value">value of variable</param>
        void SetItem(object value);
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
}
