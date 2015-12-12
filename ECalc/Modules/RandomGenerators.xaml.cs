using ECalc.Engineering;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for RandomGenerators.xaml
    /// </summary>
    public partial class RandomGenerators : UserControl
    {
        public RandomGenerators()
        {
            InitializeComponent();
        }

        private async void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CbNormal.IsChecked == true)
                {
                    var response = await RandomGens.DefaultRandom((int)Count.Value, (int)Minimum.Value, (int)Maximum.Value);
                    TbResults.Text = response;
                }
                else
                {
                    var response = await RandomGens.CryptoRandom((int)Count.Value, (int)Minimum.Value, (int)Maximum.Value);
                    TbResults.Text = response;
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private async void BtnGenerateIpsum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await RandomGens.LoremIpsum((int)MinWords.Value, (int)MaxWords.Value, (int)MinSentences.Value, (int)MaxSentences.Value, (int)Paragraphs.Value);
                TbIpsum.Text = response;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }
    }
}
