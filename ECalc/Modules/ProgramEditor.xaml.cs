using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;

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
            var syntax = Application.GetResourceStream(new Uri("pack://application:,,,/ECalc;component/Ecalc.xshd"));
            Editor.SyntaxHighlighting = HighlightingLoader.Load(new XmlTextReader(syntax.Stream), HighlightingManager.Instance);

            _engine = new IronPythonEngine.Engine();
            _timer = new DispatcherTimer();
            _timer.IsEnabled = false;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_lastcounter != Editor.Text.Length && BtnSave.IsEnabled == false)
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
                Editor.Text = content;
                _lastcounter = Editor.Text.Length;
            }
            _timer.IsEnabled = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var appdir = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(appdir, "UserFunctions.py");

            using (var tx = File.CreateText(file))
            {
                tx.Write(Editor.Text);
            }

            BtnSave.IsEnabled = false;
            _lastcounter = Editor.Text.Length;
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
