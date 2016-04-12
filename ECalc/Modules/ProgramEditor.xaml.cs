using System;
using System.Windows;
using System.Windows.Controls;
using WPFLib.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ProgramEditor.xaml
    /// </summary>
    public partial class ProgramEditor : UserControl
    {
        public ProgramEditor()
        {
            InitializeComponent();
            var syntax = Application.GetResourceStream(new Uri("pack://application:,,,/ECalc;component/EcalcSyntax.xml"));
            SyntaxBox.CurrentHighlighter = new XmlHighlighter(syntax.Stream);
        }
    }
}
