using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ECalc.Maths
{
    class NumberTester
    {
        private long _int;

        public void Test(object param)
        {
            double number = Convert.ToDouble(param);
            double num = Math.Truncate(number);
            IsInteger = (number - num) == 0;
            if (IsInteger) _int = Convert.ToInt64(num);

            if (!IsInteger) return;

            #region Odd/Even test
            IsEven = (_int % 2) == 0;
            IsOdd = !IsEven;
            #endregion

            #region Prime Test
            if (_int == 2 || _int == 3) IsPrime = true;
            else if (IsEven) IsPrime = false;
            else
            {
                IsPrime = true;
                for (long i = 3; i < Math.Sqrt(_int); i++)
                {
                    if (_int % i == 0)
                    {
                        IsPrime = false;
                        break;
                    }
                }
            }
            #endregion

            #region BitLength Test
            string s = Convert.ToString(_int, 2);
            BitLength = s.Length;
            #endregion

            #region Divisor test
            if (IsPrime || !IsInteger) Divisiors = new long[] { 1, _int };
            else
            {
                var divisors = new List<long>(20);
                for (long i = 2; i < 200; i++)
                {
                    if (_int < i) break;
                    if (_int % i == 0) divisors.Add(i);
                }
                Divisiors = divisors.ToArray();
            }
            #endregion
        }

        private string RenderText()
        {
            var render = new StringBuilder();
            Type current = this.GetType();
            foreach (var prop in current.GetProperties())
            {
                object value = prop.GetValue(this);
                var enumerable = value as IEnumerable;
                if (enumerable != null)
                {
                    var sub = new StringBuilder();
                    foreach (var item in enumerable)
                    {
                        sub.AppendFormat("{0}; ", item);
                    }
                    render.AppendFormat("{0,13} => {1}\n", prop.Name, sub);
                }
                else
                    render.AppendFormat("{0,13} => {1}\n", prop.Name, value);
            }
            return render.ToString();
        }

        public bool IsInteger
        {
            get; private set;
        }

        public bool IsEven
        {
            get; private set;
        }

        public bool IsOdd
        {
            get; private set;
        }

        public bool IsPrime
        {
            get; private set;
        }

        public int BitLength
        {
            get; private set;
        }

        public long[] Divisiors
        {
            get; private set;
        }

        public override string ToString()
        {
            return RenderText();
        }
    }
}
