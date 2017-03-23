using ECalc.Engineering;
using System;
using System.Threading;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for HashCalculators.xaml
    /// </summary>
    public partial class HashCalculators : UserControl
    {
        private bool _loaded;
        private AsyncHasher hasher;
        private CancellationTokenSource cts;

        public HashCalculators()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
        }

        private async void ComputeHash()
        {
            if (!_loaded) return;
            try
            {
                var selected = (CbHash.Items[CbHash.SelectedIndex] as ComboBoxItem).Content.ToString();
                var algo = HashAlgorithms.MD5;
                bool convresult = Enum.TryParse<HashAlgorithms>(selected, out algo);
                hasher = new AsyncHasher(algo);
                var result = await hasher.ComputeHash(TbInput.Text);
                TbOutput.Text = AsyncHasher.HashString(result);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private void TbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputeHash();
        }

        private void CbHash_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded) return;
            if (TabMode.SelectedIndex == 0) ComputeHash();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _loaded = true;
        }

        private void ReportProgress(double d)
        {
            Dispatcher.Invoke(() =>
            {
                Progress.Value = d;
            });
        }

        private async void BtnStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var selected = (CbHash.Items[CbHash.SelectedIndex] as ComboBoxItem).Content.ToString();
                var algo = HashAlgorithms.MD5;
                bool convresult = Enum.TryParse<HashAlgorithms>(selected, out algo);
                hasher = new AsyncHasher(algo);

                using (var fs = System.IO.File.OpenRead(InputFile.SelectedFile))
                {
                    var progressIndicator = new Progress<double>(ReportProgress);
                    var hash = await hasher.ComputeHash(fs, cts.Token, progressIndicator);
                    TbOutput.Text = AsyncHasher.HashString(hash);

                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            cts.Cancel();
            Progress.Value = 0;
        }
    }
}
