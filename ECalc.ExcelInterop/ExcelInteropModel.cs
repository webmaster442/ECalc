using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ECalc.ExcelInterop
{
    public class ExcelInteropModel: ViewModel
    {
        private DispatcherTimer _timer;
        private bool _excelRunning;

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand GetSetCommand { get; private set; }
        public DelegateCommand GetMatrixCommand;

        public bool ExcelRunning
        {
            get { return _excelRunning; }
            private set { SetValue(ref _excelRunning, value); }
        }

        public ExcelInteropModel()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(1500);
            _timer.IsEnabled = true;
            StartCommand = DelegateCommand.ToCommand(ExecuteStart, () => ExcelRunning == false);
            StopCommand = DelegateCommand.ToCommand(ExecuteStop, () => ExcelRunning == true);
            GetSetCommand = DelegateCommand.ToCommand(ExecuteGetSet, () => ExcelRunning == true);
        }

        private void ExecuteGetSet()
        {
            var list = ExcelInterop.Instance.ReadSelectionToList();
        }

        private void ExecuteStop()
        {
            ExcelInterop.Instance.Close();
        }

        private void ExecuteStart()
        {
            ExcelInterop.Instance.GetInstance();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            ExcelRunning = ExcelInterop.Instance.IsExcelRunning;
        }
    }
}
