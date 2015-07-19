using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
