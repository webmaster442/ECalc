using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Api.Controls
{
    class ValueSelector: StackPanel
    {
        static ValueSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueSelector), new FrameworkPropertyMetadata(typeof(ValueSelector)));
        }

        public DependencyProperty ValuesProperty = DependencyProperty.Register("Values", typeof(double[]), typeof(ValueSelector));

        [TypeConverter(typeof(ArrayTypeConverter))]
        public double[] Values
        {
            get { return (double[])GetValue(ValuesProperty); }
            set
            {
                SetValue(ValuesProperty, value);
                Render();
            }
        }

        private void Render()
        {
            if (Values == null) return;
            for (int i = 0; i < Values.Length; i++)
            {
                RadioButton r = new RadioButton();
                r.Content = Values[i];
                r.ToolTip = Values[i];
                this.Children.Add(r);
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
