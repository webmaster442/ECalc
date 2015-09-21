using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Engineering
{
    class NumberSystemConv
    {
        /// <summary>
        /// Converts a single char to binary
        /// </summary>
        /// <param name="dec">decimal char</param>
        /// <returns>binary value</returns>
        private static string DecimalToBin(char dec)
        {
            switch (dec)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'A':
                case 'a':
                    return "1010";
                case 'B':
                case 'b':
                    return "1011";
                case 'C':
                case 'c':
                    return "1100";
                case 'D':
                case 'd':
                    return "1101";
                case 'E':
                case 'e':
                    return "1110";
                case 'F':
                case 'f':
                    return "1111";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert a byte array to hexadecimal representation
        /// </summary>
        /// <param name="array">array to convert</param>
        /// <returns>hexadecimal string</returns>
        public static string ByteArrayToHex(byte[] array)
        {
            StringBuilder ret = new StringBuilder();
            foreach (var b in array)
            {
                ret.Append(Convert.ToString(b, 16));
            }
            return ret.ToString();
        }

        /// <summary>
        /// Convert a byte array to a binary representation
        /// </summary>
        /// <param name="array">array tp convert</param>
        /// <returns>binary string</returns>
        public static string ByteArrayToBin(byte[] array)
        {
            StringBuilder ret = new StringBuilder();
            foreach (var b in array)
            {
                string tmp =Convert.ToString(b, 16);
                for (int i=0; i<tmp.Length; i++)
                {
                    ret.Append(DecimalToBin(tmp[i]));
                }
            }
            return ret.ToString();
        }

        /// <summary>
        /// Convert a decimal value to binary BCD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimalToBCDBin(long value)
        {
            string chars = value.ToString();
            StringBuilder ret = new StringBuilder();
            for (int i=0; i<chars.Length; i++)
            {
                ret.Append(DecimalToBCDBin(chars[i]));
            }
            return ret.ToString();
        }
    }
}
