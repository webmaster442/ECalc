using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ECalc.Api.Controls
{
    /// <summary>
    /// Base class for converters to be used in the x: xaml namespace
    /// </summary>
    public abstract class BaseConverter : MarkupExtension
    {
        /// <summary>
        /// returns an object that is provided as the value of the target property for this markup extension
        /// </summary>
        /// <param name="serviceProvider"> A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// Converts an object selection value to a bool.
    /// Returns true, if the source object is not null
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class SelectedToEnabledConverter : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return true;
            else return false;
        }

        /// <summary>
        /// Converts a value. In this case it's not implemented
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
