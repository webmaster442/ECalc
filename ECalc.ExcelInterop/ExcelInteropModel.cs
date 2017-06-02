using AppLib.WPF.MVVM;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AppLib.Common.MessageHandler;

namespace ECalc.ExcelInterop
{
    public class ExcelInteropModel: ViewModel
    {

        public DelegateCommand ConnectCommand { get; private set; }
        public DelegateCommand DisconnectCommand { get; private set; }
        public DelegateCommand TerminateCommand { get; private set; }
        public DelegateCommand GetSetCommand { get; private set; }
        public DelegateCommand GetMatrixCommand { get; private set; }

        private StringBuilder _statuses;

        public string StatusText
        {
            get { return _statuses.ToString(); }
            set
            {
                _statuses.Append(value);
                NotifyPropertyChanged();
            }
        }

        public ExcelInteropModel()
        {
            _statuses = new StringBuilder();
            StatusText = "Not connected to Excel\r\n";
            ConnectCommand = DelegateCommand.ToCommand(ExecuteConnect);
            DisconnectCommand = DelegateCommand.ToCommand(ExecuteDisconnect);
            TerminateCommand = DelegateCommand.ToCommand(ExecuteTerminate);
            GetSetCommand = DelegateCommand.ToCommand(ExecuteGetSet);
            GetMatrixCommand = DelegateCommand.ToCommand(ExecuteGetMatrix);
        }

        private void ExecuteGetMatrix()
        {
            try
            {
                StatusText = "Starting matrix import...\r\n";
                var m = ExcelInterop.Instance.ReadSelectionToMatrix();
                Messager.Instance.SendMessage(m);
                StatusText = string.Format("Imported {0}x{1} matrix\r\n", m.GetLength(0), m.GetLength(1));

            }
            catch (Exception ex)
            {
                ExcelInterop.Error(ex.Message);
            }
        }

        private void ExecuteGetSet()
        {
            try
            {
                StatusText = "Starting set import...\r\n";
                var list = ExcelInterop.Instance.ReadSelectionToList();
                Messager.Instance.SendMessage(list);
                StatusText = string.Format("Imported {0} items\r\n", list.Count);
                
            }
            catch (Exception ex)
            {
                ExcelInterop.Error(ex.Message);
            }
        }

        private async void ExecuteConnect()
        {
            StatusText = "Connecting to excel";
            bool succes = false;
            for (int i = 0; i < 5; i++)
            {
                StatusText = ".";
                succes = ExcelInterop.Instance.GetInstance();
                if (succes)
                {
                    StatusText = " Connected\r\n";
                    break;
                }
                await Task.Delay(10);
            }
            if (!succes)
                StatusText = "Failed to connect. Please retry.\r\n";
        }

        private void ExecuteDisconnect()
        {
            ExcelInterop.Instance.Close();
            StatusText = "Disconnected from excel\r\n";
        }

        private void ExecuteTerminate()
        {
            var msg = MessageBox.Show("Close all excel instances?", "Close Excel", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                ExcelInterop.Instance.Close(true);
                StatusText = "Closed all Excel instances\r\n";
            }
        }
    }
}
