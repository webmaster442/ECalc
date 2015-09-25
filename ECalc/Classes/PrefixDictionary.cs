using System;
using System.Linq;
using System.Collections.Generic;

namespace ECalc.Classes
{
    /// <summary>
    /// Dictionary override with prefixes
    /// </summary>
    internal class PrefixDictionary: Dictionary<string, double>
    {
        public PrefixDictionary(): base(19)
        {
            this.Add("da", Math.Pow(10, 1));
            this.Add("y", Math.Pow(10, -24));
            this.Add("z", Math.Pow(10, -21));
            this.Add("f", Math.Pow(10, -18));
            this.Add("n", Math.Pow(10, -9));
            this.Add("u", Math.Pow(10, -6));
            this.Add("m", Math.Pow(10, -3));
            this.Add("c", Math.Pow(10, -2));
            this.Add("d", Math.Pow(10, -1));
            this.Add("a", Math.Pow(10, -18));
            this.Add("h", Math.Pow(10, 2));
            this.Add("k", Math.Pow(10, 3));
            this.Add("M", Math.Pow(10, 6));
            this.Add("G", Math.Pow(10, 9));
            this.Add("T", Math.Pow(10, 12));
            this.Add("P", Math.Pow(10, 15));
            this.Add("E", Math.Pow(10, 18));
            this.Add("Z", Math.Pow(10, 21));
            this.Add("Y", Math.Pow(10, 24));
        }

        /// <summary>
        /// Divides a double value to the nearest corresponding prefix value
        /// </summary>
        /// <param name="value">value to divide</param>
        /// <returns>return string</returns>
        public string DivideToPrefix(double value)
        {
            if (value == 1) return value.ToString();
            double final = value;
            string text = "";
            var sorted = from i in this where i.Value > 999 || i.Value <= 0.001 orderby i.Value descending select i;
            foreach (var prefix in sorted)
            {
                if (final > prefix.Value)
                {
                    final /= prefix.Value;
                    text = prefix.Key;
                    break;
                }
            }
            return string.Format("{0} {1}", final, text);
        }
    }
}
