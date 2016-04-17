using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECalc.IronPythonEngine.Types
{
    public class Set: List<double>
    {
        public Set(params double[] numbers) : base(numbers.Length)
        {
            AddRange(numbers);
        }

        public Set(int count): base(count) { }

        public static Set Distinct(Set arg)
        {
            return (Set)arg.Distinct().ToList();
        }
        
        public static Set Intersect(Set arg1, Set arg2)
        {
            return (Set)arg1.Intersect(arg2).ToList();
        }

        public static Set Union(Set arg1, Set arg2)
        {
            return (Set)arg1.Union(arg2).ToList();
        }

        public static Set Except(Set arg1, Set arg2)
        {
            return (Set)arg1.Except(arg2).ToList();
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
