using System;
using System.Windows;
using System.Windows.Controls;
using ECalc.MNBConverter;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

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

        private async void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
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

                dloadpanel.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }
    }
}
