using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ECalc.Classes
{
    public class CSVReader : IEnumerable<string[]>, IDisposable
    {
        private TextReader _tx;
        private char _seperator;

        public CSVReader(Stream s, char seperator)
        {
            _tx = new StreamReader(s);
            _seperator = seperator;
        }

        public CSVReader(string text, char seperator)
        {
            _tx = new StringReader(text);
            _seperator = seperator;
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            string line;
            do
            {
                line = _tx.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                yield return Parse(line, _seperator);
            }
            while (line != null);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected virtual void Dispose(bool native)
        {
            if (_tx != null)
            {
                _tx.Dispose();
                _tx = null;
                GC.SuppressFinalize(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Splits input string by separator. Values in "" marks won't be splitted
        /// </summary>
        /// <param name="Line">A string</param>
        /// <param name="seperator">Seperator character</param>
        /// <returns>an array of strings</returns>
        private string[] Parse(string Line, char seperator = ' ')
        {
            char[] parmChars = Line.ToCharArray();
            bool inQuote = false;
            for (int index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"')
                    inQuote = !inQuote;
                if (!inQuote && parmChars[index] == seperator)
                    parmChars[index] = '\n';
            }
            return (new string(parmChars)).Replace("\"", "").Split('\n');
        }
    }

    public class CSVWriter: IDisposable
    {
        private StreamWriter _sw;
        private char _seperator;

        public CSVWriter(string file, char seperator)
        {
            _sw = File.CreateText(file);
            _seperator = seperator;
        }

        public void WriteLine(params string[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i=0; i<array.Length; i++)
            {
                sb.Append(array[i]);
                if (i != array.Length -1) sb.Append(_seperator);
            }
            _sw.WriteLine(sb.ToString());
        }

        public void WriteLine(params object[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString());
                if (i != array.Length - 1) sb.Append(_seperator);
            }
            _sw.WriteLine(sb.ToString());
        }

        protected virtual void Dispose(bool native)
        {
            if (_sw != null)
            {
                _sw.Dispose();
                _sw = null;
                GC.SuppressFinalize(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

}
