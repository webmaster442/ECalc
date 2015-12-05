using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using ECalc.Api;
using System;
using System.Windows.Media;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class CalculatorChooser : UserControl
    {
        private string _link;
        private bool _designtime;

        public CalculatorChooser()
        {
            InitializeComponent();

            _designtime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (_designtime) return;

            CategoryView.ItemsSource = MainWindow.Modules.Categories;
            CategoryView.SelectedIndex = 0;
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            var title = ((Tile)sender).Title;
            UserControl control = null;

            switch (title)
            {
                case "Calculator":
                    control = new Calculator();
                    break;
                case "Statistics":
                    control = new Stat();
                    break;
                case "Equation System Solver":
                    control = new EquationSystemSolver();
                    break;
                case "Unit Converter":
                    control = new UnitConverterPage();
                    break;
                case "Currency Converter":
                    control = new CurrencyConverter();
                    break;
                case "Function Plot":
                    control = new Graphing();
                    break;
                default:
                    return;
            }

            MainWindow.SwithToControl(control);
            MainWindow.SetTitle(title);
        }

        private void Render()
        {
            ModuleDisplay.Children.Clear();
            var filter = CategoryView.SelectedItem.ToString();
            var matchs = MainWindow.Modules.Select(filter);

            foreach (var match in matchs)
            {
                Tile t = new Tile();
                t.ToolTip = match.ModuleName;
                t.Title = match.ModuleName;
                t.Background = match.BackColor;
                Image icon = new Image();
                icon.Source = match.Icon;
                t.Content = icon;
                t.Click += T_Click;
                ModuleDisplay.Children.Add(t);

            }

        }

        private void T_Click(object sender, RoutedEventArgs e)
        {
            Tile t = (Tile)sender;
            var title = t.ToolTip.ToString();
            var ctrl = MainWindow.Modules.RunByName(title);
            MainWindow.SwithToControl(ctrl);
            MainWindow.SetTitle(title);
        }

        private void CategoryView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Render();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime) return;
            try
            {
                BingWallPaperClient bing = new BingWallPaperClient();
                await bing.DownloadAsync();
                var imgbrush = new ImageBrush();
                imgbrush.ImageSource = bing.WPFPhotoOfTheDay;
                imgbrush.Stretch = Stretch.UniformToFill;
                RectImage.Fill = imgbrush;
                BtnBing.ToolTip = string.Format("Background provided by Bing:\r\n{0}", bing.CoppyRightData);
                _link = bing.CoppyRightLink;
            }
            catch (Exception)
            {
                BtnBing.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnBing_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(_link);
        }

        private void ContextNormal_Click(object sender, RoutedEventArgs e)
        {
            Tile t = (Tile)sender;
            var title = t.ToolTip.ToString();
            switch (title)
            {
                case "Statistics":
                case "Equation System Solver":
                case "Unit Converter":
                case "Currency Converter":
                case "Function Plot":
                    Tile_Click(t, new RoutedEventArgs());
                    break;
                default:
                    T_Click(t, new RoutedEventArgs());
                    break;
            }
        }

        private void ContextNewWindow_Click(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
