using AppLib.WPF.Dialogs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxImage = System.Windows.MessageBoxImage;


namespace ECalc.ExcelInterop
{
    public sealed class ExcelInterop
    {
        private Application _excelapp;
        private Workbooks _workbooks;

        private ExcelInterop() { }

        private static ExcelInterop _instance;

        public static ExcelInterop Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ExcelInterop();
                return _instance;
            }
        }

        public static void Error(string msg)
        {
            System.Windows.MessageBox.Show(msg, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }

        private void InitExcel()
        {
            if (_excelapp != null) _excelapp.Visible = true;
            _workbooks = _excelapp.Workbooks;
            if (_workbooks.Count == 0)
                _workbooks.Add(Missing.Value);
        }

        public bool GetInstance()
        {
            try
            {
                _excelapp = (Application)Marshal.GetActiveObject("Excel.Application");
                InitExcel();
                return true;
            }
            catch (COMException)
            {
                try
                {
                    _excelapp = new Application();
                    InitExcel();
                    return true;
                }
                catch (COMException ex)
                {
                    ErrorDialog.Show(ex);
                    ComCleanUp();
                    return false;
                }
            }
        }

        public void Close(bool kill = false)
        {
            ComCleanUp();
            if (kill)
            {
                var excels = Process.GetProcessesByName("excel");
                foreach (var x in excels) x.Kill();
            }
        }

        public List<double> ReadSelectionToList()
        {
            if (_excelapp == null)
            {
                Error("Excel Not running. Try to connect to Excel again");
                return null;
            }

            Range selected = _excelapp.Selection as Range;

            if (selected == null)
            {
                Error("No cells selected. Please select cells before continuing");
                return null;
            }

            var value = selected.Cells.Value as Array;
            var ret = new List<double>(value.GetLength(0) * value.GetLength(1));

            int errors = 0;

            foreach (var v in value)
            {
                try
                {
                    var dbl = Convert.ToDouble(v);
                    ret.Add(dbl);
                }
                catch (Exception)
                {
                    errors++;
                }
            }

            ReleaseComObject(selected);
            if (errors > 0)
            {
                var msg = string.Format("Failed to parse {0} cell values", errors);
                MessageBox.Show(msg, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return ret;
        }

        public double[,] ReadSelectionToMatrix()
        {
            if (_excelapp == null)
            {
                Error("Excel Not running. Try to connect to Excel again");
                return null;
            }

            Range selected = _excelapp.Selection as Range;

            if (selected == null)
            {
                Error("No cells selected. Please select cells before continuing");
                return null;
            }

            var value = selected.Cells.Value as Array;

            var ret = new double[value.GetLength(0), value.GetLength(1)];

            int errors = 0;

            for (int i = 1; i <= value.GetLength(0); i++)
            {
                for (int j = 1; j <= value.GetLength(1); j++)
                {
                    try
                    {
                        var val = value.GetValue(i, j);
                        ret[i - 1, j - 1] = Convert.ToDouble(val);
                    }
                    catch (Exception)
                    {
                        ret[i - 1, j - 1] = 0;
                        errors++;
                    }
                }
            }

            ReleaseComObject(selected);

            if (errors > 0)
            {
                var msg = string.Format("Failed to parse {0} cell values", errors);
                MessageBox.Show(msg, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return ret;

        }

        private void ReleaseComObject(object o, bool collect = false)
        {
            if (o != null)
            {
                Marshal.ReleaseComObject(o);
                o = null;
            }
            if (collect) GC.Collect();
        }

        public void ComCleanUp()
        {
            ReleaseComObject(_excelapp, false);
            ReleaseComObject(_workbooks, false);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
