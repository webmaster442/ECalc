using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFLib.Extensions;

namespace ECalc.Api.Controls
{
    /// <summary>
    /// Creates a value selector, that lets you choose from a range of items
    /// </summary>
    public class ValueSelector : Control
    {
        private double[] _values;
        private StackPanel Content;
        private string toselect;

        static ValueSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueSelector), new FrameworkPropertyMetadata(typeof(ValueSelector)));
        }

        /// <summary>
        /// Selected Item changed event
        /// </summary>
        public event RoutedEventHandler SelectedItemChanged;

        /// <summary>
        /// Aply the control theme
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Content = (StackPanel)Template.FindName("PART_Content", this);
            Render();
        }


        /// <summary>
        /// An array of values that the user can choose from
        /// </summary>
        [TypeConverter(typeof(ArrayTypeConverter))]
        public double[] Values
        {
            get { return _values; }
            set
            {
                _values = value;
                Render();
                if (SelectedItemChanged != null)
                {
                    SelectedItemChanged(this, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the Selected item
        /// </summary>
        public double? SelectedItem
        {
            get
            {
                if (_values == null) return null;
                var radio = (from i in Content.FindChildren<RadioButton>() where i.IsChecked == true select i).FirstOrDefault();
                if (radio == null) return null;
                return double.Parse(radio.Content.ToString());
            }
            set
            {
                toselect = value.ToString();
                DoSelection();
            }
        }

        private void DoSelection()
        {
            if (toselect == null) return;
            var q = (from i in Content.FindChildren<RadioButton>() where i.Content.ToString() == toselect select i).FirstOrDefault();
            if (q == null) return;
            else
            {
                q.IsChecked = true;
                toselect = null;
            }
        }

        private void Render()
        {
            if (Values == null) return;
            if (Content == null) return;
            Content.Children.Clear();
            for (int i = 0; i < Values.Length; i++)
            {
                var r = new RadioButton();
                r.Content = Values[i];
                r.ToolTip = Values[i];
                r.Margin = new Thickness(3);
                r.Checked += R_Checked;
                r.Unchecked += R_Checked;
                Content.Children.Add(r);
            }
            DoSelection();
        }

        private void R_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, new RoutedEventArgs());
            }
        }
    }

    class ArrayTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var list = value as string;
            if (list != null)
            {
                var ret = (from i in list.Split(',') select double.Parse(i)).ToArray();
                return ret;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
    }
}
