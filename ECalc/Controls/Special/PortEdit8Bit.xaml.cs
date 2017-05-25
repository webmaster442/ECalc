using AppLib.WPF.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for PortEdit8Bit.xaml
    /// </summary>
    public partial class PortEdit8Bit : UserControl
    {
        public RoutedEventHandler ValueChanged;

        public PortEdit8Bit()
        {
            InitializeComponent();
        }

        private int Recalculate()
        {
            int value = 0;
            var buttons = EditGrid.FindChildren<ToggleButton>();
            foreach (var button in buttons)
            {
                if (button.IsChecked == true)
                    value |= button.TabIndex;
            }
            return value;
        }

        private void Set(int value)
        {
            if (value > 255) value = 255;
            if (value < 0) value = 0;

            var buttons = EditGrid.FindChildren<ToggleButton>();
            foreach (var button in buttons)
            {
                button.IsChecked = false;
                if (value - button.TabIndex >= 0)
                {
                    value -= button.TabIndex;
                    button.IsChecked = true;
                }
            }
        }

        public void Reset()
        {
            Value = 0;
            ValueBox.Text = Convert.ToString(Value, 16);
        }

        public void Invert()
        {
            var current = Value;
            Value = (~current & 0xff);
            ValueBox.Text = Convert.ToString(Value, 16).PadLeft(2, '0');
        }

        public int Value
        {
            get { return Recalculate(); }
            set { Set(value); }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ValueBox.Text = Convert.ToString(Value, 16).PadLeft(2, '0');
            ValueChanged?.Invoke(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Reset();
        }
    }
}
