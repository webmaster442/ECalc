using ECalc.CurrencyConverter;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for CurrencyConverter.xaml
    /// </summary>
    public partial class CurrencyConverter : UserControl
    {
        public CurrencyConverter()
        {
            InitializeComponent();
        }

        private async void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = Convert.ToDouble(TbInput.Text);
                CurrencyConvertorSoap client = new CurrencyConvertorSoapClient();

                Currency source, target;
                Enum.TryParse<Currency>(CbSource.SelectedItem.ToString(), out source);
                Enum.TryParse<Currency>(CbDestination.SelectedItem.ToString(), out target);
                double rate = await client.ConversionRateAsync(source, target);
                value *= rate;
                TbResult.Text = value.ToString();

            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }
    }
}
