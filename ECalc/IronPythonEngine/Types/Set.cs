using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.IronPythonEngine.Types
{
    public class Set: List<double>
    {
        public Set(params double[] numbers) : base(numbers.Length)
        {
            AddRange(numbers);
        }

        public Set(List<double> itms) : base(itms) { }

        public Set(int count): base(count) { }

        public static Set Distinct(Set arg)
        {
            var res = arg.Distinct().ToList();
            return new Set(res);
        }
        
        public static Set Intersect(Set arg1, Set arg2)
        {
            var res = arg1.Intersect(arg2).ToList();
            return new Set(res);
        }

        public static Set Union(Set arg1, Set arg2)
        {
            var res = arg1.Union(arg2).ToList();
            return new Set(res);
        }

        public static Set Except(Set arg1, Set arg2)
        {
            var res = arg1.Except(arg2).ToList();
            return new Set(res);
        }

        public static Set operator + (Set input, double number)
        {
            var copy = new Set(input.Count);
            Parallel.For(0, input.Count, i =>
            {
                copy[i] = input[i] + number;
            });
            return copy;
        }

        public static Set operator + (Set a, Set b)
        {
            var min = System.Math.Min(a.Count, b.Count);
            var result = new Set(min);
            Parallel.For(0, min, i =>
            {
                result[i] = a[i] + b[i];
            });
            return result;
        }

        public static Set operator -(Set input, double number)
        {
            var copy = new Set(input.Count);
            Parallel.For(0, input.Count, i =>
            {
                copy[i] = input[i] + number;
            });
            return copy;
        }

        public static Set operator -(Set a, Set b)
        {
            var min = System.Math.Min(a.Count, b.Count);
            var result = new Set(min);
            Parallel.For(0, min, i =>
            {
                result[i] = a[i] - b[i];
            });
            return result;
        }

        public static Set operator *(Set input, double number)
        {
            var copy = new Set(input.Count);
            Parallel.For(0, input.Count, i =>
            {
                copy[i] = input[i] * number;
            });
            return copy;
        }

        public static Set operator *(Set a, Set b)
        {
            var min = System.Math.Min(a.Count, b.Count);
            var result = new Set(min);
            Parallel.For(0, min, i =>
            {
                result[i] = a[i] * b[i];
            });
            return result;
        }

        public static Set operator /(Set input, double number)
        {
            var copy = new Set(input.Count);
            Parallel.For(0, input.Count, i =>
            {
                copy[i] = input[i] + number;
            });
            return copy;
        }

        public static Set operator /(Set a, Set b)
        {
            var min = System.Math.Min(a.Count, b.Count);
            var result = new Set(min);
            Parallel.For(0, min, i =>
            {
                result[i] = a[i] / b[i];
            });
            return result;
        }

        public static Set operator %(Set input, double number)
        {
            var copy = new Set(input.Count);
            Parallel.For(0, input.Count, i =>
            {
                copy[i] = input[i] % number;
            });
            return copy;
        }

        public static Set operator %(Set a, Set b)
        {
            var min = System.Math.Min(a.Count, b.Count);
            var result = new Set(min);
            Parallel.For(0, min, i =>
            {
                result[i] = a[i] % b[i];
            });
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int limit = Count;
            bool fulllist = true;
            if (Count > 12)
            {
                limit = 12;
                fulllist = false;
            }
            for (int i=0; i<limit; i++)
            {
                sb.AppendFormat("{0}, ", this[i]);
            }
            if (!fulllist) sb.Append("...");
            return sb.ToString();

        }
    }
}
