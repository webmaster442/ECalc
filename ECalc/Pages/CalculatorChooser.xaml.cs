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

            CategoryView.ItemsSource = App.Modules.Categories;
            CategoryView.SelectedIndex = 0;
        }

        private void Render()
        {
            ModuleDisplay.Children.Clear();
            var filter = CategoryView.SelectedItem.ToString();
            var matchs = App.Modules.Select(filter);

            foreach (var match in matchs)
            {
                var t = new Tile();
                t.ToolTip = match.ModuleName;
                t.Title = match.ModuleName;
                t.Background = match.BackColor;
                var icon = new Image();
                icon.Source = match.Icon;
                t.Content = icon;
                t.Click += Tile_Click;
                ModuleDisplay.Children.Add(t);

            }
        }

        private UserControl GetControl(Tile t)
        {
            UserControl control = null;

            switch (t.Title)
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
                    var title = t.ToolTip.ToString();
                    control = App.Modules.RunByName(title);
                    break;
            }

            return control;
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            var tile = ((Tile)sender);
            var ctrl = GetControl(tile);
            MainWindow.SwithToControl(ctrl);
            MainWindow.SetTitle(tile.Title);
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
                var bing = new BingWallPaperClient();
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
            var menu = ((MenuItem)sender).Parent as ContextMenu;
            var tile = (Tile)menu.PlacementTarget;
            Tile_Click(tile, e);
        }

        private void ContextNewWindow_Click(object sender, RoutedEventArgs e)
        {
            var mnu = (MenuItem)sender;
            if (mnu != null)
            {
                var tile = ((ContextMenu)mnu.Parent).PlacementTarget as Tile;
                var ctrl = GetControl(tile);
                var fw = new FloatWindow();
                fw.SetWindowContent(ctrl, tile.Title);
                fw.Show();
            }
        }
    }
}
