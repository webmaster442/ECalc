using ECalc.Engineering;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for NumberSystems.xaml
    /// </summary>
    public partial class NumberSystems : UserControl
    {
        public NumberSystems()
        {
            InitializeComponent();
        }

        private void ConverInt()
        {
            try
            {
                long input = 0;
                var input_radio = InputSelector.Children.OfType<RadioButton>().FirstOrDefault(i => i.IsChecked.Value);
                var output_radio = OutputSelector.Children.OfType<RadioButton>().FirstOrDefault(i => i.IsChecked.Value);
                switch (input_radio.Content.ToString())
                {
                    case "Decimal":
                        input = Convert.ToInt64(InputNumber.Text, 10);
                        break;
                    case "Binary":
                        input = Convert.ToInt64(InputNumber.Text, 2);
                        break;
                    case "Octal":
                        input = Convert.ToInt64(InputNumber.Text, 8);
                        break;
                    case "Hexa":
                        input = Convert.ToInt64(InputNumber.Text, 16);
                        break;
                }
                switch (output_radio.Content.ToString())
                {
                    case "Decimal":
                        OutputNumber.Text = Convert.ToString(input, 10);
                        break;
                    case "Binary":
                        OutputNumber.Text = Convert.ToString(input, 2);
                        break;
                    case "Octal":
                        OutputNumber.Text = Convert.ToString(input, 8);
                        break;
                    case "Hexa":
                        OutputNumber.Text = Convert.ToString(input, 16);
                        break;
                }
            }
            catch (Exception)
            {
                if (OutputNumber != null) OutputNumber.Text = "Conversion Error";
            }

        }

        private void ConvertIEEE754(string text)
        {
            try
            {
                StringBuilder buffer = new StringBuilder();
                float single = Convert.ToSingle(text);
                double d = Convert.ToDouble(text);
                string singlebin = NumberSystemConv.ByteArrayToBin(BitConverter.GetBytes(single));
                string singlehex = NumberSystemConv.ByteArrayToHex(BitConverter.GetBytes(single));
                string doublebin = NumberSystemConv.ByteArrayToBin(BitConverter.GetBytes(d));
                string doublehex = NumberSystemConv.ByteArrayToHex(BitConverter.GetBytes(d));
                buffer.AppendFormat("Hexadecimal single value:   {0}\n", singlehex);
                buffer.AppendFormat("Binary single value:        {0}\n", singlebin);
                buffer.Append("--------------------------------------------------\n");
                buffer.AppendFormat("Hexadecimal double value:   {0}\n", doublehex);
                buffer.AppendFormat("Binary double value:        {0}\n", doublebin);
                IEEE754Output.Text = buffer.ToString();
            }
            catch  (Exception)
            {
                IEEE754Output.Text = "Input error";
            }
        }

        private void InputNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConverInt();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ConverInt();
        }

        private void IEEE754Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConvertIEEE754(IEEE754Input.Text);
        }
    }
}
