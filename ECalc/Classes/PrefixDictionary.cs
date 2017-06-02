using System.Collections.Generic;
using System.Linq;

namespace ECalc.Classes
{
    /// <summary>
    /// Dictionary override with prefixes
    /// </summary>
    internal class PrefixDictionary : Dictionary<string, double>
    {
        public PrefixDictionary() : base(19)
        {
            this.Add("y", 1E-24);
            this.Add("z", 1E-21);
            this.Add("a", 1E-18);
            this.Add("f", 1E-15);
            this.Add("p", 1E-12);
            this.Add("n", 1E-9);
            this.Add("u", 1E-6);
            this.Add("m", 1E-3);
            this.Add("c", 1E-2);
            this.Add("d", 1E-1);
            this.Add("da", 1E1);
            this.Add("h", 1E2);
            this.Add("k", 1E3);
            this.Add("M", 1E6);
            this.Add("G", 1E9);
            this.Add("T", 1E12);
            this.Add("P", 1E15);
            this.Add("E", 1E18);
            this.Add("Z", 1E21);
            this.Add("Y", 1E24);
        }

        /// <summary>
        /// Divides a double value to the nearest corresponding prefix value
        /// </summary>
        /// <param name="value">value to divide</param>
        /// <returns>return string</returns>
        public string DivideToPrefix(double value)
        {
            if (((value < 999) && (value >= 0.001)) || value == 1 || value == 0) return value.ToString();
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
