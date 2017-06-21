using System.Collections.Generic;
using System.Windows.Controls;

namespace Ecalc.FFmpegGui
{
    /// <summary>
    /// Interaction logic for InputFiles.xaml
    /// </summary>
    public partial class InputFiles : UserControl
    {
        public InputFiles()
        {
            InitializeComponent();
        }

        private InputFilesViewModel ViewModel
        {
            get { return (InputFilesViewModel)DataContext; }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Selected.Clear();
            foreach (string f in FilesList.SelectedItems)
            {
                ViewModel.Selected.Add(f);
            }
        }

        public IEnumerable<string> Files
        {
            get { return ViewModel.Files; }
        }
    }
}
