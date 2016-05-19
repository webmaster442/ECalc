using ECalc.IronPythonEngine;
using System.Linq;
using System;

namespace ECalc.Maths
{
    [Loadable]
    public static class SetFunctions
    {
        [Category("Sets")]
        public static IronPythonEngine.Types.Set Set(params double[] d)
        {
            return new IronPythonEngine.Types.Set(d);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Load()
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                var set = new IronPythonEngine.Types.Set(10);
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    using (var stream = System.IO.File.OpenText(openFileDialog.FileName))
                    {
                        string line;
                        while ((line = stream.ReadLine()) != null)
                        {
                            var d = Convert.ToDouble(line);
                            set.Add(d);
                        }
                    }
                    return set;
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
            return new IronPythonEngine.Types.Set(0);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Distinct(IronPythonEngine.Types.Set s1)
        {
            return IronPythonEngine.Types.Set.Distinct(s1);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Intersect(IronPythonEngine.Types.Set s1, IronPythonEngine.Types.Set s2)
        {
            return IronPythonEngine.Types.Set.Intersect(s1, s2);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Union(IronPythonEngine.Types.Set s1, IronPythonEngine.Types.Set s2)
        {
            return IronPythonEngine.Types.Set.Union(s1, s2);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Except(IronPythonEngine.Types.Set s1, IronPythonEngine.Types.Set s2)
        {
            return IronPythonEngine.Types.Set.Except(s1, s2);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Series(double start, double d, double items)
        {
            var set = new IronPythonEngine.Types.Set();
            set.Add(start);
            var current = start;
            for (int i=0; i<items; i++)
            {
                current += d;
                set.Add(current);
            }
            return set;
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set GeometricSeries(double start, double q, double items)
        {
            var set = new IronPythonEngine.Types.Set(items);
            set.Add(start);
            var current = start;
            for (int i = 0; i < items; i++)
            {
                current *= q;
                set.Add(current);
            }
            return set;
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set RandomSet(double min, double max, double items)
        {
            var set = new IronPythonEngine.Types.Set(items);
            Random r = new Random();
            var imin = Convert.ToInt32(min);
            var imax = Convert.ToInt32(max);
            for (int i = 0; i < items; i++)
            {

                var item = r.Next(imin, imax);
                set.Add(item);
            }
            return set;
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set SortA(IronPythonEngine.Types.Set set)
        {
            var q = from i in set orderby i ascending select i;
            return new IronPythonEngine.Types.Set(q.ToList());
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set SortD(IronPythonEngine.Types.Set set)
        {
            var q = from i in set orderby i descending select i;
            return new IronPythonEngine.Types.Set(q.ToList());
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set SRandomSet(double min, double max, double items)
        {
            var set = new IronPythonEngine.Types.Set(items);
            var r = new Lib.CryptoRNG();
            var imin = Convert.ToInt32(min);
            var imax = Convert.ToInt32(max);
            for (int i = 0; i < items; i++)
            {

                var item = r.Next(imin, imax);
                set.Add(item);
            }
            return set;
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set QRandomSet(double min, double max, double items)
        {
            var set = new IronPythonEngine.Types.Set(items);
            var r = new Sublight.Utilities.QuantumRandomNumberGenerator();
            var imin = Convert.ToInt32(min);
            var imax = Convert.ToInt32(max);
            for (int i = 0; i < items; i++)
            {

                var item = r.Next(imin, imax);
                set.Add(item);
            }
            return set;
        }
    }
}
