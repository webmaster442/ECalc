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
}
