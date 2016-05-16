using ECalc.Api;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for CalculatorChooser2.xaml
    /// </summary>
    public partial class CalculatorChooser2 : UserControl
    {
        public CalculatorChooser2()
        {
            InitializeComponent();
        }

        #region Navigation & Rendering

        private void Render()
        {
            ModuleView.Children.Clear();
            var filter = CategorySelector.SelectedItem.ToString();

            EcalcModule[] matchs = null;

            if (string.IsNullOrEmpty(SearchBox.Text))
                matchs = App.Modules.SelectCategory(filter);
            else
                matchs = App.Modules.Search(SearchBox.Text);

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
                ModuleView.Children.Add(t);

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

        #endregion

        #region Context Menu
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



        #endregion

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime) return;

            CategorySelector.ItemsSource = App.Modules.Categories;
            CategorySelector.SelectedIndex = 0;

            try
            {
                var bing = new BingWallPaperClient();
                await bing.DownloadAsync();
                var imgbrush = new ImageBrush();
                imgbrush.ImageSource = bing.WPFPhotoOfTheDay;
                imgbrush.Stretch = Stretch.UniformToFill;
                MainGrid.Background = imgbrush;
                BackUri.NavigateUri = new Uri(bing.CoppyRightLink);
                BackUri.ToolTip = bing.CoppyRightData;
            }
            catch (Exception)
            {
                BingText.Visibility = Visibility.Collapsed;
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink s = sender as Hyperlink;
            System.Diagnostics.Process.Start(s.NavigateUri.ToString());
        }
    }
}
