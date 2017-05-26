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
        public DelegateCommand GetMatrixCommand;

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
            ConnectCommand = DelegateCommand.ToCommand(ExecuteConnect);
            DisconnectCommand = DelegateCommand.ToCommand(ExecuteDisconnect);
            TerminateCommand = DelegateCommand.ToCommand(ExecuteTerminate);
            GetSetCommand = DelegateCommand.ToCommand(ExecuteGetSet);
        }

        private void ExecuteGetSet()
        {
            try
            {
                var list = ExcelInterop.Instance.ReadSelectionToList();
                MessageSender.Instance.SendMessage(list);
                
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
                StatusText = "Failed to connect. Please retry.";
        }

        private void ExecuteDisconnect()
        {
            ExcelInterop.Instance.Close();
            StatusText = "Disconnected from excel";
        }

        private void ExecuteTerminate()
        {
            var msg = MessageBox.Show("Close all excel instances?", "Close Excel", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                ExcelInterop.Instance.Close(true);
                StatusText = "Closed all Excel instances";
            }
        }
    }
}
