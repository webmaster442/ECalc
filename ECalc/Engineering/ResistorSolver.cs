using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECalc.Engineering
{
    internal static class ResistorValueSolver
    {
        public static string Solve(double desiredvalue, ResistorSeries serie)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--------------------------------------------------------------------");
            sb.AppendLine("Serial configuration:");
            sb.AppendLine("--------------------------------------------------------------------");
            double remain = desiredvalue;
            double sum = 0;
            var list = ResitorListGenerator.GenerateList(serie);
            int i, j;

            for (i = 0; i < 3; i++)
            {
                var q = (from item in list where item <= remain orderby item descending select item).FirstOrDefault();
                if (q != 0)
                {
                    remain -= q;
                    sum += q;
                    sb.AppendLine(q.ToString() + " Ω");
                }
            }
            sb.AppendLine("--------------------------------------------------------------------");
            sb.AppendFormat("Sum value: {0} Ω\r\n", sum);
            sb.AppendFormat("Error: {0} Ω\r\n", remain);
            sb.AppendFormat("Error(%): {0} %\r\n", Math.Round(remain / sum, 4) * 100);

            sb.Append("\r\n\r\n\r\n");

            sb.AppendLine("--------------------------------------------------------------------");
            sb.AppendLine("Parallel configuration:");
            sb.AppendLine("--------------------------------------------------------------------");

            list = (from item in ResitorListGenerator.GenerateList(serie) where item > (desiredvalue / 2) select item).ToList();
            Dictionary<double, double[]> _results = new Dictionary<double, double[]>();
            double marginp = desiredvalue + (desiredvalue * ResitorListGenerator.GetTolerance(serie));
            double marginn = desiredvalue - (desiredvalue * ResitorListGenerator.GetTolerance(serie));

            for (i = 0; i < list.Count; i++)
            {
                for (j = 0; j < list.Count; j++)
                {
                    double value = (list[i] * list[j]) / (list[i] + list[j]);
                    if ((value > marginn) && (value < marginp))
                    {
                        if (_results.ContainsKey(value)) _results[value] = new double[] { list[i], list[j] };
                        else _results.Add(value, new double[] { list[i], list[j] });
                    }
                }
            }

            var best = (from result in _results where result.Key <= desiredvalue orderby (desiredvalue - result.Key) ascending select result).FirstOrDefault();
            if (best.Value != null)
            {
                sb.AppendLine(best.Value[0].ToString() + " Ω");
                sb.AppendLine(best.Value[1].ToString() + " Ω");
            }
            sb.AppendLine("--------------------------------------------------------------------");
            sb.AppendFormat("Paralell value: {0} Ω\r\n", best.Key);
            sb.AppendFormat("Error: {0} Ω\r\n", desiredvalue - best.Key);
            sb.AppendFormat("Error(%): {0} %\r\n", Math.Round((desiredvalue - best.Key) / desiredvalue, 4) * 100);

            return sb.ToString();

        }

        public static double StandardResistorValueApproximation(double val, int tolerance)
        {
            double series = 0;

            if (tolerance == 20) series = 6;
            else if (tolerance == 10) series = 12;
            else if (tolerance == 5) series = 24;
            else if (tolerance == 2) series = 48;
            else if (tolerance == 1) series = 96;
            else series = 192;

            var l = Math.Log10(val);

            var decplaces = series < 48 ? 10 : 100;

            var pref_val = (Math.Round((Math.Pow(10, (Math.Round(series * l) / series)) / Math.Pow(10, Math.Floor(Math.Log10((Math.Pow(10, (Math.Round(series * l) / series))))))) * decplaces) / decplaces) * Math.Pow(10, Math.Floor(Math.Log10((Math.Pow(10, (Math.Round(series * l) / series))))));

            var rounded = Math.Round(pref_val);
            var abs = Math.Abs(rounded - pref_val);
            if (abs > 0.999 || abs < 0.0001)
                pref_val = rounded;

            if (pref_val >= 260 && pref_val <= 460) pref_val += 10; // fix for E24/E12/E6 series   
            else if (pref_val == 830) pref_val -= 10;               // fix for E24/E12/E6 series
            else if (pref_val == 919) pref_val++;                   // fix for E192 series

            return pref_val;

        }
    }
}
