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
                WaitText.Visibility = Visibility.Visible;
                if (CbNormal.IsChecked.Value)
                {
                    var response = await RandomGens.DefaultRandom((int)Count.Value, (int)Minimum.Value, (int)Maximum.Value);
                    TbResults.Text = response;
                }
                else if (CbCrypto.IsChecked.Value)
                {
                    var response = await RandomGens.CryptoRandom((int)Count.Value, (int)Minimum.Value, (int)Maximum.Value);
                    TbResults.Text = response;
                }
                else
                {
                    var response = await RandomGens.QuantumRandom((int)Count.Value, 
                                                                  (int)Minimum.Value, 
                                                                  (int)Maximum.Value);
                    TbResults.Text = response;
                }
                WaitText.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
                WaitText.Visibility = Visibility.Collapsed;
            }
        }

        private async void BtnGenerateIpsum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await RandomGens.LoremIpsum((int)MinWords.Value, 
                                                           (int)MaxWords.Value,
                                                           (int)MinSentences.Value,
                                                           (int)MaxSentences.Value,
                                                           (int)Paragraphs.Value);
                TbIpsum.Text = response;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private async void BtnGeneratePasswords_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await RandomGens.Passwords((int)PassLength.Value,
                                                        (int)NumPassword.Value,
                                                        (bool)PasswdLowecase.IsChecked,
                                                        (bool)PasswdUppercase.IsChecked,
                                                        (bool)PasswdNumbers.IsChecked,
                                                        (bool)PasswdSpecials.IsChecked);
                TbPasswords.Text = result;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }
    }
}
