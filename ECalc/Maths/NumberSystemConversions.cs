using System;
using System.Collections.Generic;
using System.Text;

namespace ECalc.Maths
{
    public static class NumberSystemConversions
    {
        /// <summary>
        /// Convert a roman string number to integer value
        /// </summary>
        /// <param name="input">Roman Input</param>
        /// <returns>an integer value</returns>
        public static int RomanToInt(string input)
        {
            Dictionary<string, int> RomanNumbers = new Dictionary<string, int>();
            RomanNumbers.Add("M", 1000);
            RomanNumbers.Add("CM", 900);
            RomanNumbers.Add("D", 500);
            RomanNumbers.Add("CD", 400);
            RomanNumbers.Add("C", 100);
            RomanNumbers.Add("XC", 90);
            RomanNumbers.Add("L", 50);
            RomanNumbers.Add("XL", 40);
            RomanNumbers.Add("X", 10);
            RomanNumbers.Add("IX", 9);
            RomanNumbers.Add("V", 5);
            RomanNumbers.Add("IV", 4);
            RomanNumbers.Add("I", 1);

            int result = 0;
            string textform = input.ToUpper();

            foreach (var pair in RomanNumbers)
            {
                while (textform.IndexOf(pair.Key.ToString()) == 0)
                {
                    result += int.Parse(pair.Value.ToString());
                    textform = input.Substring(pair.Key.ToString().Length);
                }
            }

            return result;
        }

        /// <summary>
        /// Convert an integer to a Roman number
        /// </summary>
        /// <param name="number">Number to convert</param>
        /// <returns>A roman number representation of the input</returns>
        public static string IntToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + IntToRoman(number - 1000);
            if (number >= 900) return "CM" + IntToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + IntToRoman(number - 500);
            if (number >= 400) return "CD" + IntToRoman(number - 400);
            if (number >= 100) return "C" + IntToRoman(number - 100);
            if (number >= 90) return "XC" + IntToRoman(number - 90);
            if (number >= 50) return "L" + IntToRoman(number - 50);
            if (number >= 40) return "XL" + IntToRoman(number - 40);
            if (number >= 10) return "X" + IntToRoman(number - 10);
            if (number >= 9) return "IX" + IntToRoman(number - 9);
            if (number >= 5) return "V" + IntToRoman(number - 5);
            if (number >= 4) return "IV" + IntToRoman(number - 4);
            if (number >= 1) return "I" + IntToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

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

        private static char BCDToDecimal(string bcd)
        {
            switch (bcd)
            {
                case "0000":
                    return '0';
                case "0001":
                    return '1';
                case "0010":
                    return '2';
                case "0011":
                    return '3';
                case "0100":
                    return '4';
                case "0101":
                    return '5';
                case "0110":
                    return '6';
                case "0111":
                    return '7';
                case "1000":
                    return '8';
                case "1001":
                    return '9';
                default:
                    return '-';
            }
        }

        private static StringBuilder Reverse(StringBuilder sb)
        {
            StringBuilder ret = new StringBuilder(sb.Length);
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                ret.Append(sb[i]);
            }
            return ret;
        }

        /// <summary>
        /// Slices a string array to the specified length from backwards
        /// </summary>
        /// <param name="input">string input</param>
        /// <param name="slicelen">slice length</param>
        /// <returns>an array of slices</returns>
        private static string[] Slice(string input, int slicelen)
        {
            Stack<string> stack = new Stack<string>();
            StringBuilder tmp = new StringBuilder();
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (tmp.Length >= slicelen)
                {
                    tmp = Reverse(tmp);
                    stack.Push(tmp.ToString());
                    tmp.Clear();
                }
                tmp.Append(input[i]);

            }

            if (tmp.Length > 0)
            {
                if (tmp.Length < slicelen)
                {
                    for (int i = 0; i < slicelen; i++) tmp.Append('0');
                }
                tmp = Reverse(tmp);
                stack.Push(tmp.ToString());
                tmp.Clear();
            }

            return stack.ToArray();
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
                string s = Convert.ToString(b, 16);
                if (s.Length < 2) s = "0" + s;
                ret.Append(s);
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
                string tmp = Convert.ToString(b, 16);
                if (tmp.Length < 2) tmp = "0" + tmp;
                for (int i = 0; i < tmp.Length; i++)
                {
                    ret.Append(DecimalToBin(tmp[i]));
                }
            }
            return ret.ToString();
        }

        /// <summary>
        /// Convert a decimal value to binary BCD
        /// </summary>
        /// <param name="value">a number to convert</param>
        /// <returns>BCD value</returns>
        public static string DecimalToBCDBin(long value)
        {
            string chars = value.ToString();
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < chars.Length; i++)
            {
                ret.Append(DecimalToBin(chars[i]));
            }
            return ret.ToString();
        }

        /// <summary>
        /// Convert a binary BCD number to decimal
        /// </summary>
        /// <param name="value">BCD value to convert</param>
        /// <returns>a number</returns>
        public static long BCDBinToDecimal(string value)
        {
            var parts = Slice(value, 4);
            StringBuilder text = new StringBuilder();
            foreach (var part in parts)
            {
                text.Append(BCDToDecimal(part));
            }
            return Convert.ToInt64(text.ToString());
        }
    }
}
