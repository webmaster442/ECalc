using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for ResistorColorSelector.xaml
    /// </summary>
    public partial class ResistorColorSelector : UserControl
    {

        private double[] multipliers =  { 0.01, 0.1, 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 };
        private double[] tolerance =  { 10, 5, 1, 2, 0.5, 0.25, 0.1 };
        private double[] temp = { 100, 50, 15, 25 };

        public class Item
        {
            public string Name { get; set; }
            public SolidColorBrush Color { get; set; }
        }

        public event RoutedEventHandler SelectionChanged; 

        private ObservableCollection<Item> _items;

        public static DependencyProperty SelectorTypeProperty = DependencyProperty.Register("SelectorType", typeof(SelectorTypes), typeof(ResistorColorSelector), new PropertyMetadata(SelectorTypes.Digit, new PropertyChangedCallback(SelectorTypeCallback)));

        public ResistorColorSelector()
        {
            InitializeComponent();
            _items = new ObservableCollection<Item>();
            ColorList.ItemsSource = _items;
            Redraw();
        }

        public SelectorTypes SelectorType
        {
            get { return (SelectorTypes)GetValue(SelectorTypeProperty); }
            set
            {
                SetValue(SelectorTypeProperty, value);
            }
        }

        private static void SelectorTypeCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ResistorColorSelector)sender).Redraw();
        }

        private void Redraw()
        {
            _items.Clear();
            switch (SelectorType)
            {
                case SelectorTypes.Digit:
                    _items.Add(new Item { Name = "Black", Color = new SolidColorBrush(Colors.Black) });
                    _items.Add(new Item { Name = "Brown", Color = new SolidColorBrush(Colors.Brown) });
                    _items.Add(new Item { Name = "Red", Color = new SolidColorBrush(Colors.Red) });
                    _items.Add(new Item { Name = "Orange", Color = new SolidColorBrush(Colors.Orange) });
                    _items.Add(new Item { Name = "Yellow", Color = new SolidColorBrush(Colors.Yellow) });
                    _items.Add(new Item { Name = "Green", Color = new SolidColorBrush(Colors.Green) });
                    _items.Add(new Item { Name = "Blue", Color = new SolidColorBrush(Colors.Blue) });
                    _items.Add(new Item { Name = "Purple", Color = new SolidColorBrush(Colors.Purple) });
                    _items.Add(new Item { Name = "Gray", Color = new SolidColorBrush(Colors.Gray) });
                    _items.Add(new Item { Name = "White", Color = new SolidColorBrush(Color.FromRgb(240, 240, 240)) });
                    break;
                case SelectorTypes.Multiplier:
                    _items.Add(new Item { Name = "Silver", Color = new SolidColorBrush(Colors.Silver) });
                    _items.Add(new Item { Name = "Gold", Color = new SolidColorBrush(Colors.Gold) });
                    _items.Add(new Item { Name = "Brown", Color = new SolidColorBrush(Colors.Brown) });
                    _items.Add(new Item { Name = "Red", Color = new SolidColorBrush(Colors.Red) });
                    _items.Add(new Item { Name = "Orange", Color = new SolidColorBrush(Colors.Orange) });
                    _items.Add(new Item { Name = "Yellow", Color = new SolidColorBrush(Colors.Yellow) });
                    _items.Add(new Item { Name = "Green", Color = new SolidColorBrush(Colors.Green) });
                    _items.Add(new Item { Name = "Blue", Color = new SolidColorBrush(Colors.Blue) });
                    _items.Add(new Item { Name = "Purple", Color = new SolidColorBrush(Colors.Purple) });
                    break;
                case SelectorTypes.Tolerance:
                    _items.Add(new Item { Name = "Silver", Color = new SolidColorBrush(Colors.Silver) });
                    _items.Add(new Item { Name = "Gold", Color = new SolidColorBrush(Colors.Gold) });
                    _items.Add(new Item { Name = "Brown", Color = new SolidColorBrush(Colors.Brown) });
                    _items.Add(new Item { Name = "Red", Color = new SolidColorBrush(Colors.Red) });
                    _items.Add(new Item { Name = "Green", Color = new SolidColorBrush(Colors.Green) });
                    _items.Add(new Item { Name = "Blue", Color = new SolidColorBrush(Colors.Blue) });
                    _items.Add(new Item { Name = "Purple", Color = new SolidColorBrush(Colors.Purple) });
                    break;
                case SelectorTypes.Temperature:
                    _items.Add(new Item { Name = "Brown", Color = new SolidColorBrush(Colors.Brown) });
                    _items.Add(new Item { Name = "Red", Color = new SolidColorBrush(Colors.Red) });
                    _items.Add(new Item { Name = "Orange", Color = new SolidColorBrush(Colors.Orange) });
                    _items.Add(new Item { Name = "Yellow", Color = new SolidColorBrush(Colors.Yellow) });
                    break;
            }
            ColorList.SelectedIndex = 0;
        }

        public double Value
        {
            get
            {
                int index = ColorList.SelectedIndex;

                if (index < 0) return double.NaN;
                var item = _items[index];
                switch (SelectorType)
                {
                    case SelectorTypes.Digit:
                        return index;
                    case SelectorTypes.Multiplier:
                        return multipliers[index];
                    case SelectorTypes.Tolerance:
                        return tolerance[index];
                    case SelectorTypes.Temperature:
                        return temp[index];
                    default:
                        return double.NaN;
                }
            }
        }

        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(sender, null);
            }
        }
    }

    public enum SelectorTypes
    {
        Digit,
        Tolerance,
        Multiplier,
        Temperature
    }
}
