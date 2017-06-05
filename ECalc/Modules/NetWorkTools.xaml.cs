using PowerShellEngine;
using System;
using System.Management.Automation.Runspaces;
using System.Windows.Controls;
using System.Windows.Media;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for NetWorkTools.xaml
    /// </summary>
    public partial class NetWorkTools : UserControl
    {
        private PowerShellHelper _shell;

        public NetWorkTools()
        {
            InitializeComponent();
        }

        private void StatusLedColor(Color c)
        {
            StatusLed.Fill = new SolidColorBrush(c);
        }

        private void InitEngine()
        {
            _shell = new PowerShellHelper();
            _shell.PipelineErrorRaised += _shell_PipelineErrorRaised;
            _shell.PipelineStateChangedReceived += _shell_PipelineStateChangedReceived;
            _shell.PowerShellErrorRaised += _shell_PowerShellErrorRaised;
            _shell.RunspaceStateChangedReceived += _shell_RunspaceStateChangedReceived;
            _shell.ScriptStarted += _shell_ScriptStarted;
        }

        private void _shell_ScriptStarted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Progress.IsIndeterminate = true;
                StatusText.Text = "Running ...";
            });
        }

        private void _shell_PipelineStateChangedReceived(object sender, PipelineStateEventArg arg)
        {
            Dispatcher.Invoke(() =>
            {
                switch (arg.State)
                {
                    case PipelineState.Stopped:
                    case PipelineState.Failed:
                    case PipelineState.Completed:
                        StatusLedColor(Colors.Transparent);
                        Progress.IsIndeterminate = false;
                        StatusText.Text = "Completed";
                        break;
                    case PipelineState.Stopping:
                        StatusText.Text = "Stopping";
                        StatusLedColor(Colors.Orange);
                        break;
                    case PipelineState.Running:
                        StatusText.Text = "Running ...";
                        StatusLedColor(Colors.LightGreen);
                        break;
                }

            });
        }

        private void _shell_RunspaceStateChangedReceived(object sender, RunspaceStateEventArg arg)
        {
            Dispatcher.Invoke(() =>
            {
                switch (arg.State)
                {
                    case RunspaceState.Opening:
                    case RunspaceState.Closing:
                        StatusLedColor(Colors.Orange);
                        break;
                    case RunspaceState.Opened:
                        StatusLedColor(Colors.LightGreen);
                        break;
                    case RunspaceState.Closed:
                        StatusLedColor(Colors.Transparent);
                        break;
                    case RunspaceState.Broken:
                        Progress.IsIndeterminate = false;
                        StatusLedColor(Colors.Red);
                        break;

                }
            });
        }

        private void _shell_PowerShellErrorRaised(object sender, PowerShellErrorEventArg arg)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = "Error";
                MainWindow.ErrorDialog(arg.PowerShellException.Message);
            });
        }

        private void _shell_PipelineErrorRaised(object sender, PipelineErrorEventArg arg)
        {
            StatusText.Text = "Error";
            Progress.IsIndeterminate = false;
            StatusLedColor(Colors.Red);
        }
    }
}
