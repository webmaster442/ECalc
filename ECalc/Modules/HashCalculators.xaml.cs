using ECalc.Engineering;
using System;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for HashCalculators.xaml
    /// </summary>
    public partial class HashCalculators : UserControl
    {
        private bool _loaded;

        public HashCalculators()
        {
            InitializeComponent();
        }

        private async void ComputeHash()
        {
            if (!_loaded) return;
            var selected = (CbHash.Items[CbHash.SelectedIndex] as ComboBoxItem).Content.ToString();
            HashFunctions.Algorithms algo = HashFunctions.Algorithms.MD5;
            bool convresult = Enum.TryParse<HashFunctions.Algorithms>(selected, out algo);
            if (!convresult) algo = HashFunctions.Algorithms.MD5;
           
            string result = await HashFunctions.HashString(algo, TbInput.Text);
            TbOutput.Text = result;
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

        private void BtnSelectFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "All files | *.*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BtnSelectFile.Content = ofd.FileName;
            }
        }

        private async void BtnStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var selected = (CbHash.Items[CbHash.SelectedIndex] as ComboBoxItem).Content.ToString();
                HashFunctions.Algorithms algo = HashFunctions.Algorithms.MD5;
                bool convresult = Enum.TryParse<HashFunctions.Algorithms>(selected, out algo);
                if (!convresult) algo = HashFunctions.Algorithms.MD5;

                Progress.IsIndeterminate = true;
                string result = await HashFunctions.HashFile(algo, BtnSelectFile.Content.ToString());
                Progress.IsIndeterminate = false;
                TbOutput.Text = result;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }
    }
}
