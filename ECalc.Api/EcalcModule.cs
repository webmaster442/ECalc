using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        /// Control tile color
        /// </summary>
        public virtual SolidColorBrush Color
        {
            get { return new SolidColorBrush(Colors.YellowGreen); }
        }

        /// <summary>
        /// Control tile icon
        /// </summary>
        public virtual BitmapImage Icon
        {
            get { return null; }
        }
    }
}
