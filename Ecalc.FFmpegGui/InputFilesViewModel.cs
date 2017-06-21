using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecalc.FFmpegGui
{
    public class InputFilesViewModel: ViewModel
    {
        public ObservableCollection<string> Files { get; private set; }
        public DelegateCommand AddFolder { get; private set; }
        public DelegateCommand AddFiles { get; private set; }
        public DelegateCommand RemoveSelected { get; private set; }
        public DelegateCommand RemoveAll { get; private set; }

        public InputFilesViewModel()
        {
            Files = new ObservableCollection<string>();
            AddFiles = DelegateCommand.ToCommand(ExecAddFiles);
            AddFolder = DelegateCommand.ToCommand(ExecAddFolder);
            RemoveSelected = DelegateCommand.ToCommand(ExecRemoveSelected);
            RemoveAll = DelegateCommand.ToCommand(ExecRemoveAll);
        }

        private void ExecRemoveAll()
        {
            throw new NotImplementedException();
        }

        private void ExecRemoveSelected()
        {
            throw new NotImplementedException();
        }

        private void ExecAddFolder()
        {
            throw new NotImplementedException();
        }

        private void ExecAddFiles()
        {
            throw new NotImplementedException();
        }
    }
}
