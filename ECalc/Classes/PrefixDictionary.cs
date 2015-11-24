using System;
using System.Linq;
using System.Collections.Generic;

namespace ECalc.Classes
{
    /// <summary>
    /// Dictionary override with prefixes
    /// </summary>
    internal class PrefixDictionary : Dictionary<string, double>
    {
        public PrefixDictionary() : base(19)
        {
            this.Add("y", 10E-24);
            this.Add("z", 10E-21);
            this.Add("a", 10E-18);
            this.Add("f", 10E-15);
            this.Add("p", 10E-12);
            this.Add("n", 10E-9);
            this.Add("u", 10E-6);
            this.Add("m", 10E-3);
            this.Add("c", 10E-2);
            this.Add("d", 10E-1);
            this.Add("da", 10E1);
            this.Add("h", 10E2);
            this.Add("k", 10E3);
            this.Add("M", 10E6);
            this.Add("G", 10E9);
            this.Add("T", 10E12);
            this.Add("P", 10E15);
            this.Add("E", 10E18);
            this.Add("Z", 10E21);
            this.Add("Y", 10E24);
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
                if (final >= prefix.Value)
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
