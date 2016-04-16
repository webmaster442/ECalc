using ECalc.Classes;
using ECalc.IronPythonEngine;
using ECalc.Maths;
using System;
using System.Numerics;
using System.Text;
using System.Windows.Controls;

namespace ECalc.Controls
{
    internal class NumberSystemTextBox: TextBox
    {
        public NumberSystemTextBox()
        {
            AcceptsReturn = true;
            AcceptsTab = true;
        }

        public void DisplayNumber(object o)
        {
            if (Helpers.IsSpecialType(o))
            {
                Text = "Special types are not supported for conversion";
                return;
            }
            double d = Convert.ToDouble(o);
            float f = Convert.ToSingle(o);
            bool floats = (d - Math.Truncate(d)) != 0;
            var buffer = new StringBuilder();
            if (floats)
            {
                byte[] singlebytes = BitConverter.GetBytes(f);
                Array.Reverse(singlebytes);

                byte[] doublebytes = BitConverter.GetBytes(d);
                Array.Reverse(doublebytes);

                buffer.AppendFormat("IEEE 754 Double: {0}\r\n", NumberSystemConversions.ByteArrayToHex(doublebytes));
                buffer.AppendFormat("IEEE 754 Single: {0}", NumberSystemConversions.ByteArrayToHex(singlebytes));
                Text = buffer.ToString();
                return;
            }
            else
            {
                string bin, oct, hex;
                var bi = new BigInteger(d);
                bin = NumberSystemConversions.ToSystem(bi, 2);
                oct = NumberSystemConversions.ToSystem(bi, 8);
                hex = NumberSystemConversions.ToSystem(bi, 16);

                int bits = bin.Length;
                bin = NumberSystemConversions.FormatBin(bin);

                buffer.AppendFormat("DEC: {0}\n", bi);
                buffer.AppendFormat("BIN: {0}\n", bin);
                buffer.AppendFormat("OCT: {0}\n", oct);
                buffer.AppendFormat("HEX: {0}\n", hex);
                buffer.AppendFormat("-------------------------------------\n");
                buffer.AppendFormat("Bits: {0}", bits);
            }
            Text = buffer.ToString();
        }
    }
}
