using AppLib.WPF.Dialogs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ECalc.ExcelInterop
{
    public sealed class ExcelInterop
    {
        private Application _excelapp;


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


        public bool GetInstance()
        {
            try
            {
                _excelapp = (Application)Marshal.GetActiveObject("Excel.Application");
                if (_excelapp != null) _excelapp.Visible = true;
                return true;
            }
            catch (COMException)
            {
                try
                {
                    _excelapp = new Application();
                    if (_excelapp != null) _excelapp.Visible = true;
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

            foreach (var v in value)
            {
                try
                {
                    var dbl = Convert.ToDouble(v);
                    ret.Add(dbl);
                }
                catch (Exception) { }
            }

            ReleaseComObject(selected);


            return ret;
        }

        private void ReleaseComObject(object o)
        {
            if (o != null)
            {
                Marshal.ReleaseComObject(o);
                o = null;
                GC.Collect();
            }
        }

        private void ComCleanUp()
        {
            ReleaseComObject(_excelapp);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
