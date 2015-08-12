using ECalc.Engineering;
using System;
using System.Windows.Controls;

namespace ECalc.Pages
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
            Enum.TryParse<HashFunctions.Algorithms>(selected, out algo);
            string result = await HashFunctions.HashString(algo, TbInput.Text);
            TbOutput.Text = result;
        }

        private void TbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputeHash();
        }

        private void CbHash_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeHash();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _loaded = true;
        }
    }
}
