using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ECalc.Classes;
using ECalc.Maths;

namespace ECalc.Controls
{
    internal class NumberSystemTextBox: TextBox
    {
        public NumberSystemTextBox(): base()
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

            StringBuilder buffer = new StringBuilder();

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
                string octsting = Convert.ToString(Convert.ToInt64(o), 8);
                switch (Engine.BitEngineMode)
                {
                    case BitEngineModes.Signed16bit:
                    case BitEngineModes.Signed32bit:
                    case BitEngineModes.Signed64bit:
                    case BitEngineModes.Signed8bit:
                        dataarray = BitConverter.GetBytes(Convert.ToInt64(o));
                        break;
                    case BitEngineModes.Unsigned16bit:
                    case BitEngineModes.Unsigned32bit:
                    case BitEngineModes.Unsigned64bit:
                    case BitEngineModes.Unsigned8bit:
                        dataarray = BitConverter.GetBytes(Convert.ToUInt64(o));
                        break;
                }
                Array.Reverse(dataarray);
                buffer.Append(DoRow("Bin:   ", dataarray, 2));
                buffer.Append(DoRow("Hex:   ", dataarray, 16));
                buffer.AppendFormat("Oct:   {0,72}\r\n", octsting);
                buffer.AppendFormat("Roman: {0,72}", NumberSystemConversions.IntToRoman(Convert.ToInt32(o)));
                Text = buffer.ToString();
            }
        }

        private StringBuilder DoRow(string label, byte[] data, int system)
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(label);
            ret.Append(" ");
            int startindex = 0;

            switch (Engine.BitEngineMode)
            {
                case BitEngineModes.Signed16bit:
                case BitEngineModes.Unsigned16bit:
                    startindex = 6;
                    break;
                case BitEngineModes.Signed32bit:
                case BitEngineModes.Unsigned32bit:
                    startindex = 4;
                    break;
                case BitEngineModes.Signed64bit:
                case BitEngineModes.Unsigned64bit:
                    startindex = 0;
                    break;
                case BitEngineModes.Signed8bit:
                case BitEngineModes.Unsigned8bit:
                    startindex = 7;
                    break;
            }

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
