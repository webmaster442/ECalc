using ECalc.MNBConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for CurrencyConverter.xaml
    /// </summary>
    public partial class CurrencyConverter : UserControl
    {
        private Dictionary<string, double> _rates;

        public CurrencyConverter()
        {
            InitializeComponent();
            _rates = new Dictionary<string, double>();
        }

        private void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            var q1 = (from i in _rates where i.Key == CbSource.SelectedItem.ToString() select i.Value);
            var q2 = (from i in _rates where i.Key == CbDestination.SelectedItem.ToString() select i.Value);
            var huf = Convert.ToDouble(TbInput.Text) * q1.FirstOrDefault();
            var curr = huf / q2.FirstOrDefault();
            TbResult.Text = curr.ToString();

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                MNBArfolyamServiceSoapClient client2 = new MNBArfolyamServiceSoapClient();

                dloadpanel.Visibility = Visibility.Visible;

                var response = await client2.GetCurrentExchangeRatesAsync(new GetCurrentExchangeRatesRequestBody());
                var xml = response.GetCurrentExchangeRatesResponse1;

                var doc = XDocument.Parse(xml.GetCurrentExchangeRatesResult);

                foreach (var item in doc.Elements().Elements().Elements())
                {
                    _rates.Add(item.Attribute("curr").Value, Convert.ToDouble(item.Value));
                }

                CbSource.ItemsSource = _rates.Keys;
                CbDestination.ItemsSource = _rates.Keys;
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
            finally
            {
                dloadpanel.Visibility = Visibility.Collapsed;
                _rates.Add("HUF", 1);
            }
        }
    }
}
