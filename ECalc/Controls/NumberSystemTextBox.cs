using ECalc.Classes;
using ECalc.IronPythonEngine;
using ECalc.Maths;
using System;
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
            //check for double type
            if (Helpers.IsSpecialType(o))
            {
                Text = "Special types are not supported for conversion";
                return;
            }

            double d = Convert.ToDouble(o);
            float f = Convert.ToSingle(o);
            bool floats = (d - Math.Truncate(d)) != 0;

            var buffer = new StringBuilder();

            //display floats
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

                byte[] dataarray = null;
                dataarray = BitConverter.GetBytes(Convert.ToInt64(o));
                Array.Reverse(dataarray);
                var x = DoRow("Bin:   ", dataarray, 2);
                buffer.Append(x);
                buffer.Append(DoRow("Hex:   ", dataarray, 16));
                var len = x.Length - 2 - "Roman: ".Length;
                buffer.AppendFormat("Roman: {0," + len + "}", NumberSystemConversions.IntToRoman(Convert.ToInt32(o)));
                Text = buffer.ToString();
            }
        }

        private StringBuilder DoRow(string label, byte[] data, int system)
        {
            var ret = new StringBuilder();
            ret.Append(label);
            ret.Append(" ");
            int startindex = 0;

            for (int i=startindex; i<8; i++)
            {
                ret.AppendFormat("{0,8}", Convert.ToString(data[i], system));
                if (i != 7) ret.Append(" ");
            }
            ret.Append("\r\n");
            return ret;
        }
    }
}
