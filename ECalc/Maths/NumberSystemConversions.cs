﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ECalc.Maths
{
    public static class NumberSystemConversions
    {
        private static Dictionary<string, int> RomanNumbers;
        private static Dictionary<byte, char> Digits;

        static NumberSystemConversions()
        {
            RomanNumbers = new Dictionary<string, int>
            {
                { "M", 1000}, {"CM", 900}, {"D", 500},
                { "CD", 400}, { "C", 100}, {"XC", 90},
                { "L", 50}, {"XL", 40}, {"X", 10},
                { "IX", 9}, {"V", 5}, { "IV", 4},
                { "I", 1}
            };

            Digits = new Dictionary<byte, char>
            {
                {10, 'A'}, {11, 'B'}, {12, 'C'}, {13, 'D'},
                {14, 'E'}, {15, 'F'}, {16, 'G'}, {17, 'H'},
                {18, 'I'}, {19, 'J'}, {20, 'K'}, {21, 'L'},
                {22, 'M'}, {23, 'N'}, {24, 'O'}, {25, 'P'},
                {26, 'Q'}, {27, 'R'}, {28, 'S'}, {29, 'T'},
                {30, 'U'}, {31, 'V'}, {32, 'W'}, {33, 'X'},
                {34, 'Y'}, {35, 'Z'}
            };
        }

        /// <summary>
        /// Convert a roman string number to integer value
        /// </summary>
        /// <param name="input">Roman Input</param>
        /// <returns>an integer value</returns>
        public static int RomanToInt(string input)
        {
            int result = 0;
            string textform = input.ToUpper();

            foreach (var pair in RomanNumbers)
            {
                while (textform.IndexOf(pair.Key) == 0)
                {
                    result += pair.Value;
                    textform = textform.Substring(pair.Key.Length);
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
            if ((number < 0) || (number > 3999)) return "Roman numbers are represented between 1 and 3999";
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

        /// <summary>
        /// Converts a number digit to a number
        /// </summary>
        /// <param name="value">number digit</param>
        /// <returns>vallue associated to number digit</returns>
        private static byte ToDigit(char value)
        {
            switch (value)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return Convert.ToByte(value - 48);
                default:
                    var q = from i in Digits where i.Value == value select i.Key;
                    return q.First();
            }
        }

        /// <summary>
        /// Converts a BCD value to decimal
        /// </summary>
        /// <param name="bcd">BCD value</param>
        /// <returns>decimal value</returns>
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

        /// <summary>
        /// Reverse a stringbuilder's content
        /// </summary>
        /// <param name="sb">StringBuilder to reverse</param>
        /// <returns>Reversed stringBuilder</returns>
        private static StringBuilder Reverse(StringBuilder sb)
        {
            var ret = new StringBuilder(sb.Length);
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
            var stack = new Stack<string>();
            var tmp = new StringBuilder();
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
            var ret = new StringBuilder();
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
            var ret = new StringBuilder();
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
            var ret = new StringBuilder();
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
            var text = new StringBuilder();
            foreach (var part in parts)
            {
                text.Append(BCDToDecimal(part));
            }
            return Convert.ToInt64(text.ToString());
        }

        /// <summary>
        /// Converts a number to a target number system
        /// </summary>
        /// <param name="number">number to convert</param>
        /// <param name="system">Target system between 2 and 36</param>
        /// <returns>The number in the specified system</returns>
        public static string ToSystem(BigInteger number, int system)
        {
            if (system < 2 || system > 36)
                throw new ArgumentException("System must be between 2 and 36");

            var output = new StringBuilder();
            while (number > 0)
            {
                var digit = number % system;
                if (digit > 9) output.Append(Digits[(byte)digit]);
                else output.Append(digit);
                number /= system;
            }
            return Reverse(output).ToString();
        }

        /// <summary>
        /// Converts a number back from a system to decimal
        /// </summary>
        /// <param name="input">Input in system</param>
        /// <param name="system">system</param>
        /// <returns>value in decimal</returns>
        public static BigInteger FromSystem(string input, int system)
        {
            int exponent = 0;
            long value = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                value += ToDigit(input[i]) * (long)Math.Pow(system, exponent);
                exponent++;
            }
            return value;
        }

        public static string FormatBin(string input)
        {
            var buffer = new StringBuilder();
            int counter = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (counter == 4)
                {
                    buffer.Append(" ");
                    counter = 0;
                }
                buffer.Append(input[i]);
                counter++;
            }
            for (int i = 0; i < (4 - counter); i++) buffer.Append(" ");
            return Reverse(buffer).ToString();
        }
    }
}
