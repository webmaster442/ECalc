using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppLib.Common.Extensions;
using AppLib.WPF.Extensions;

namespace Ecalc.FFmpegGui
{
    /// <summary>
    /// Interaction logic for OutputNamer.xaml
    /// </summary>
    public partial class OutputNamer : UserControl
    {
        public OutputNamer()
        {
            InitializeComponent();
        }

        private OutputNamerViewModel ViewModel
        {
            get { return (OutputNamerViewModel)DataContext; }
        }

        public void SetFiles(IEnumerable<string> s)
        {
            ViewModel.Inputs.Clear();
            ViewModel.Inputs.AddRange(s);
        }

        public void SetPreferedExtension(string s)
        {
            ViewModel.Extension = s;
        }

        public void OpenContextMenu(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null ||btn.ContextMenu == null) return;
            btn.ContextMenu.IsOpen = true;
            btn.ContextMenu.DataContext = ViewModel;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CaseTransformMode = Casing.SelectedIndex;
        }

        private void Inputs_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv1 = Inputs.GetDescendantByType<ScrollViewer>();
            var sv2 = Outputs.GetDescendantByType<ScrollViewer>();
            sv2.ScrollToVerticalOffset(sv1.VerticalOffset);

        }

        private void Outputs_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv1 = Inputs.GetDescendantByType<ScrollViewer>();
            var sv2 = Outputs.GetDescendantByType<ScrollViewer>();
            sv1.ScrollToVerticalOffset(sv2.VerticalOffset);
        }
    }
}
