using ECalc.Engineering;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for LogicFunctionMinimizer.xaml
    /// </summary>
    public partial class LogicFunctionMinimizer : UserControl
    {
        private bool _loaded;

        public LogicFunctionMinimizer()
        {
            InitializeComponent();
        }

        private void VarnameSet(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            Minterm2x.SwapVarnames();
            Minterm3x.SwapVarnames();
            Minterm4x.SwapVarnames();
            Minterm5x.SwapVarnames();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }

        private void BtnSetAll_Click(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            Minterm2x.SetAll(true);
            Minterm3x.SetAll(true);
            Minterm4x.SetAll(true);
            Minterm5x.SetAll(true);
        }

        private void BtnUnsetAll_Click(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            Minterm2x.SetAll(false);
            Minterm3x.SetAll(false);
            Minterm4x.SetAll(false);
            Minterm5x.SetAll(false);
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<LogicItem> items = null;
                int variables = 2;
                switch (TabInputMode.SelectedIndex)
                {
                    case 0:
                        items = Minterm2x.Selected;
                        variables = 2;
                        break;
                    case 1:
                        items = Minterm3x.Selected;
                        variables = 3;
                        break;
                    case 2:
                        items = Minterm4x.Selected;
                        variables = 4;
                        break;
                    case 3:
                        items = Minterm5x.Selected;
                        variables = 5;
                        break;
                    case 4:
                        variables = (int)EsListVarCount.Value;
                        items = new List<LogicItem>();
                        foreach (var line in TbListSet.Text.Split('\n'))
                        {
                            if (string.IsNullOrEmpty(line)) continue;
                            items.Add(LogicItem.CreateFromMintermIndex(Convert.ToInt32(line.Replace("\r", "")), variables, true));
                        }
                        foreach (var line in TbListDontCare.Text.Split('\n'))
                        {
                            if (string.IsNullOrEmpty(line)) continue;
                            items.Add(LogicItem.CreateFromMintermIndex(Convert.ToInt32(line.Replace("\r", "")), variables, null));
                        }
                        break;
                }
                string result = QuineMcclusky.GetSimplified(items, variables, (bool)CbHazardFree.IsChecked, (bool)RadioLSB.IsChecked);
                TbResults.Text = result;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog("Minimize Error: " + ex.Message);
            }
        }
    }
}