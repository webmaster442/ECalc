using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace ECalc.ExcelInterop
{
    /// <summary>
    /// CSV reader class
    /// </summary>
    internal sealed class CSVReader
    {
        private string _filename;
        private char _delimiter;

        public CSVReader(string file, char delimiter)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);

            _filename = file;
            _delimiter = delimiter;
        }

        public int Columns
        {
            get
            {
                using (var file = File.OpenText(_filename))
                {
                    var line = file.ReadLine();
                    return line.Count(c => c == _delimiter) + 1;
                }
            }
        }

        public IEnumerable<double> ReadList(int x, int y, int ex = -1, int ey = -1)
        {
            var ret = new List<double>(20);
            int i = 0;
            int errors = 0;
            using (var file = File.OpenText(_filename))
            {
                while (file.Peek() >= 0)
                {
                    var line = file.ReadLine();
                    if (i >= x)
                    {
                        if (ex != -1 && i > ex) break;
                        var items = line.Split(_delimiter);
                        if (ey == -1) ey = items.Length;
                        for (int j=y; j<ey; j++)
                        {
                            double val = 0;
                            if (double.TryParse(items[j], out val))
                                ret.Add(val);
                            else
                                errors++;
                        }
                    }
                }
            }

            if (errors != 0)
            {
                var error = string.Format("Failed to parse {0} items", errors);
                MessageBox.Show(error, "Parser warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return ret;
        }

        public double[,] ReadMatrix(int x, int y, int ex, int ey)
        {
            var ret = new double[ex - x, ey - y];
            int i = 0;
            int errors = 0;
            using (var file = File.OpenText(_filename))
            {
                while (file.Peek() >= 0)
                {
                    var line = file.ReadLine();
                    if (i >= x)
                    {
                        if (ex != -1 && i > ex) break;
                        var items = line.Split(_delimiter);
                        if (ey == -1) ey = items.Length;
                        for (int j = y; j < ey; j++)
                        {
                            double val = 0;
                            if (double.TryParse(items[j], out val))
                            {
                                ret[i, j] = val;
                            }
                            else
                            {
                                ret[i, j] = 0;
                                errors++;
                            }   
                        }
                    }
                }
            }

            if (errors != 0)
            {
                var error = string.Format("Failed to parse {0} items", errors);
                MessageBox.Show(error, "Parser warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return ret;
        }
    }
}
