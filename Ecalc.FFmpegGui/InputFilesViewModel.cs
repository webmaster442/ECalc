using AppLib.Common.Extensions;
using AppLib.WPF.MVVM;
using System.Collections.ObjectModel;

namespace Ecalc.FFmpegGui
{
    public class InputFilesViewModel: ViewModel
    {
        public ObservableCollection<string> Files { get; private set; }
        public ObservableCollection<string> Selected { get; private set; }

        public DelegateCommand AddFolder { get; private set; }
        public DelegateCommand AddFiles { get; private set; }
        public DelegateCommand RemoveSelected { get; private set; }
        public DelegateCommand RemoveAll { get; private set; }

        public InputFilesViewModel()
        {
            Files = new ObservableCollection<string>();
            Selected = new ObservableCollection<string>();
            AddFiles = DelegateCommand.ToCommand(ExecAddFiles);
            AddFolder = DelegateCommand.ToCommand(ExecAddFolder);
            RemoveSelected = DelegateCommand.ToCommand(ExecRemoveSelected, () => Selected.Count > 0);
            RemoveAll = DelegateCommand.ToCommand(ExecRemoveAll, () => Files.Count > 0);
        }

        private void ExecRemoveAll()
        {
            Files.Clear();
        }

        private void ExecRemoveSelected()
        {
            while (Selected.Count > 0)
            {
                Files.Remove(Selected[0]);
            }
        }

        private void ExecAddFolder()
        {
            var fb = new System.Windows.Forms.FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var files = System.IO.Directory.GetFiles(fb.SelectedPath, "*.*");
                Files.AddRange(files);
            }
        }

        private void ExecAddFiles()
        {
            var fd = new System.Windows.Forms.OpenFileDialog();
            fd.Filter = "Files |*.*";
            fd.Multiselect = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Files.AddRange(fd.FileNames);
            }
        }
    }
}
