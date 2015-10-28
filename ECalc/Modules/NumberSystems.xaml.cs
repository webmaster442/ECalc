using ECalc.Maths;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
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
                    case "BCD":
                        input = NumberSystemConversions.BCDBinToDecimal(InputNumber.Text);
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
                    case "BCD":
                        OutputNumber.Text = NumberSystemConversions.DecimalToBCDBin(input);
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

                byte[] singlebytes = BitConverter.GetBytes(single);
                Array.Reverse(singlebytes);

                byte[] doublebytes = BitConverter.GetBytes(d);
                Array.Reverse(doublebytes);

                string singlebin = NumberSystemConversions.ByteArrayToBin(singlebytes);
                string singlehex = NumberSystemConversions.ByteArrayToHex(singlebytes);
                string doublebin = NumberSystemConversions.ByteArrayToBin(doublebytes);
                string doublehex = NumberSystemConversions.ByteArrayToHex(doublebytes);
                buffer.AppendFormat("Hexadecimal single value:   {0}\n", singlehex);
                buffer.AppendFormat("Binary single value:        {0}\n", singlebin);
                buffer.AppendFormat("Sign:                       {0}\n", singlebin.Substring(0, 1));
                buffer.AppendFormat("Exponent:                   {0}\n", singlebin.Substring(1, 8));
                buffer.AppendFormat("Fraction:                   {0}\n", singlebin.Substring(8, 23));
                buffer.Append("--------------------------------------------------\n");
                buffer.AppendFormat("Hexadecimal double value:   {0}\n", doublehex);
                buffer.AppendFormat("Binary double value:        {0}\n", doublebin);
                buffer.AppendFormat("Sign:                       {0}\n", doublebin.Substring(0, 1));
                buffer.AppendFormat("Exponent:                   {0}\n", doublebin.Substring(1, 11));
                buffer.AppendFormat("Fraction:                   {0}\n", doublebin.Substring(11, 52));
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
