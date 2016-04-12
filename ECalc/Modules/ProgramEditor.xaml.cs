using System;
using System.Windows;
using System.Windows.Controls;
using WPFLib.Controls;
using System.IO;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ProgramEditor.xaml
    /// </summary>
    public partial class ProgramEditor : UserControl, IDisposable
    {
        private IronPythonEngine.Engine _engine;
        private DispatcherTimer _timer;
        private int _lastcounter;

        public ProgramEditor()
        {
            InitializeComponent();
            var syntax = Application.GetResourceStream(new Uri("pack://application:,,,/ECalc;component/EcalcSyntax.xml"));
            SyntaxBox.CurrentHighlighter = new XmlHighlighter(syntax.Stream);
            _engine = new IronPythonEngine.Engine();
            _timer = new DispatcherTimer();
            _timer.IsEnabled = false;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_lastcounter != SyntaxBox.Text.Length && BtnSave.IsEnabled == false)
            {
                BtnSave.IsEnabled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var appdir = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(appdir, "UserFunctions.py");

            if (File.Exists(file))
            {
                var content = File.ReadAllText(file);
                SyntaxBox.Text = content;
                _lastcounter = SyntaxBox.Text.Length;
            }
            _timer.IsEnabled = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var appdir = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(appdir, "UserFunctions.py");

            using (var tx = File.CreateText(file))
            {
                tx.Write(SyntaxBox.Text);
            }

            BtnSave.IsEnabled = false;
            _lastcounter = SyntaxBox.Text.Length;
        }

        private void BtnCompile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _engine.Compile(SyntaxBox.Text);
                MainWindow.ShowDialog("Compiler", "Code compiled without errors", MessageDialogStyle.Affirmative);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        protected void Dispose(bool native)
        {
            if (_engine != null)
            {
                _engine.Dispose();
                _engine = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
