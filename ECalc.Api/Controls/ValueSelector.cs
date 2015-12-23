using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Api.Controls
{
    public class ValueSelector: Control
    {
        private double[] _values;
        private StackPanel Content;

        static ValueSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueSelector), new FrameworkPropertyMetadata(typeof(ValueSelector)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Content = (StackPanel)Template.FindName("PART_Content", this);
            Render();
        }


        [TypeConverter(typeof(ArrayTypeConverter))]
        public double[] Values
        {
            get { return _values;  }
            set
            {
                _values = value;
                Render();
            }
        }

        private void Render()
        {
            if (Values == null) return;
            if (Content == null) return;
            Content.Children.Clear();
            for (int i = 0; i < Values.Length; i++)
            {
                RadioButton r = new RadioButton();
                r.Content = Values[i];
                r.ToolTip = Values[i];
                r.Margin = new Thickness(3);
                Content.Children.Add(r);
            }
        }
    }

    class ArrayTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string list = value as string;
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
