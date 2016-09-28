using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.IronPythonEngine
{
    /// <summary>
    /// Attribute to mark methoods with category description
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class CategoryAttribute: Attribute
    {
        public string Category
        {
            get;
            private set;
        }

        public CategoryAttribute(string cat)
        {
            Category = cat;
        }
    }

    /// <summary>
    /// Attribute to mark classes with loadable functions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class LoadableAttribute: Attribute { }

    /// <summary>
    /// Function information class
    /// </summary>
    [Serializable]
    internal class FunctionInfo
    {
        /// <summary>
        /// Function name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Function full name with class
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Function category
        /// </summary>
        public string Category { get; set; }
    }

    /// <summary>
    /// Trigonometry modes
    /// </summary>
    internal enum TrigMode
    {
        DEG, RAD, GRAD
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

        /// <summary>
        /// Hybernates the memory session to memory
        /// </summary>
        void Hybernate();

        /// <summary>
        /// Restores memory session
        /// </summary>
        void Restore();
    }
}
