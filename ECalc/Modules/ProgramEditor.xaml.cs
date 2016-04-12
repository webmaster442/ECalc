using System;
using System.Windows;
using System.Windows.Controls;
using WPFLib.Controls;
using System.IO;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ProgramEditor.xaml
    /// </summary>
    public partial class ProgramEditor : UserControl, IDisposable
    {
        private IronPythonEngine.Engine _engine;

        public ProgramEditor()
        {
            InitializeComponent();
            var syntax = Application.GetResourceStream(new Uri("pack://application:,,,/ECalc;component/EcalcSyntax.xml"));
            SyntaxBox.CurrentHighlighter = new XmlHighlighter(syntax.Stream);
            _engine = new IronPythonEngine.Engine();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var appdir = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(appdir, "UserFunctions.py");

            if (File.Exists(file))
            {
                var content = File.ReadAllText(file);
                SyntaxBox.Text = content;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var appdir = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(appdir, "UserFunctions.py");

            using (var tx = File.CreateText(file))
            {
                tx.Write(SyntaxBox.Text);
            }
        }

        private void BtnCompile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _engine.Compile(SyntaxBox.Text);
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
