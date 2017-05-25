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


        private ExcelInterop()
        {
        }

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

        public bool IsExcelRunning
        {
            get
            {
                return Process.GetProcessesByName("excel").Length > 0;
            }
        }

        public void GetInstance()
        {
            try
            {
                if (IsExcelRunning)
                    _excelapp = (Application)Marshal.GetActiveObject("Excel.Application");
                else
                    _excelapp = new Application();
            }
            catch (COMException ex)
            {
                ErrorDialog.Show(ex);
                ComCleanUp();
            }
        }

        public void Close()
        {
            ComCleanUp();
            var procs = Process.GetProcessesByName("excel");
            foreach (var proc in procs)
                proc.Kill();
        }

        public List<double> ReadSelectionToList()
        {
            Range selected = _excelapp.Selection as Range;
            var value = selected.Cells.Value as Array;
            var ret = new List<double>(value.GetLength(0) * value.GetLength(1));
            for (int i=1; i<value.GetLength(0); i++)
            {
                for (int j=1; j<value.GetLength(1); j++)
                {
                    try
                    {
                        var val = value.GetValue(i, j);
                        ret.Add(Convert.ToDouble(val));
                    }
                    catch (Exception) { }
                }
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
