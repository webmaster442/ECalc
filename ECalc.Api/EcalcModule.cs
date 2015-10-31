using System.Windows.Controls;
using System.Windows.Media;

namespace ECalc.Api
{
    /// <summary>
    /// Engineer Calculator module
    /// </summary>
    public abstract class EcalcModule
    {
        /// <summary>
        /// Module name
        /// </summary>
        public abstract string  ModuleName { get; }

        /// <summary>
        /// Module category
        /// </summary>
        public abstract string ModuleCategory { get; }

        /// <summary>
        /// Get the Control of the module
        /// </summary>
        /// <returns>The module UserControl</returns>
        public abstract UserControl GetControl();

        /// <summary>
        /// Control tile color. Can be chosen from the TileColor enum, or from a user value
        /// </summary>
        public virtual int Color
        {
            get { return (int)TileColor.Default;  }
        }

        /// <summary>
        /// Used to set tile background color
        /// </summary>
        public SolidColorBrush BackColor
        {
            get
            {
                int value = (int)Color;
                byte red = (byte)((value & 0x00FF0000) >> 16);
                byte green = (byte)((value & 0x0000FF00) >> 8);
                byte blue = (byte)((value & 0x000000FF));
                return new SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
            }
        }

        /// <summary>
        /// Control tile icon will be used later.
        /// </summary>
        public virtual ImageSource Icon
        {
            get { return null; }
        }
    }

    /// <summary>
    /// Tile Color Enumeration
    /// </summary>
    public enum TileColor: int
    {
        /// <summary>
        /// Default Gray color
        /// </summary>
        Default = 0xCCCCCC,
        /// <summary>
        /// Lime
        /// </summary>
        W8Lime = 0xA4C400,
        /// <summary>
        /// Green
        /// </summary>
        W8Green = 0x60A917,
        /// <summary>
        /// Emerald
        /// </summary>
        W8Emerald = 0x008A00,
        /// <summary>
        /// Teal
        /// </summary>
        W8Teal = 0x00ABA9,
        /// <summary>
        /// Cyan
        /// </summary>
        W8Cyan = 0x1BA1E2,
        /// <summary>
        /// Cobalt
        /// </summary>
        W8Cobalt = 0x0050EF,
        /// <summary>
        /// Indigo
        /// </summary>
        W8Indigo = 0x6A00FF,
        /// <summary>
        /// Violet
        /// </summary>
        W8Violet = 0xAA00FF,
        /// <summary>
        /// Pink
        /// </summary>
        W8Pink = 0xF472D0,
        /// <summary>
        /// Magenta
        /// </summary>
        W8Magenta = 0xD80073,
        /// <summary>
        /// Crimson
        /// </summary>
        W8Crimson = 0xA20025,
        /// <summary>
        /// Red
        /// </summary>
        W8Red = 0xE51400,
        /// <summary>
        /// Orange
        /// </summary>
        W8Orange = 0xFA6800,
        /// <summary>
        /// Amber
        /// </summary>
        W8GAmber = 0xF0A30A,
        /// <summary>
        /// Yellow
        /// </summary>
        W8Yellow = 0xE3C800,
        /// <summary>
        /// Brown
        /// </summary>
        W8Brown = 0x825A2C,
        /// <summary>
        /// Olive
        /// </summary>
        W8Olive = 0x6D8764,
        /// <summary>
        /// Steel
        /// </summary>
        W8Steel = 0x647687,
        /// <summary>
        /// Mauve
        /// </summary>
        W8Mauve = 0x76608A,
        /// <summary>
        /// Taupe
        /// </summary>
        W8Taupe = 0x87794E
    }
}
