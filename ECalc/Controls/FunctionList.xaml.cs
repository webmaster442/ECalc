using ECalc.Classes;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;


namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for FunctionList.xaml
    /// </summary>
    public partial class FunctionList : UserControl
    {
        private UsageInfo[] _usage;

        public FunctionList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Functions information
        /// </summary>
        public string[] Functions { get; set; }

        /// <summary>
        /// Function button click event
        /// </summary>
        public event StringEventHandler FunctionButtonCliked;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime) return;
            XmlSerializer xs = new XmlSerializer(typeof(UsageInfo));
            using (StringReader sr = new StringReader(ECalc.Properties.Settings.Default.UsageInfo))
            {
                _usage = (UsageInfo[])xs.Deserialize(sr);
            }
        }

        public void Render()
        {
            var query = from i in Functions orderby i ascending select i;
            Container.Children.Clear();
            foreach (var item in query)
            {
                Button b = new Button();
                b.Content = item;
                b.Click += b_Click;
                Container.Children.Add(b);
            }

        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            if (FunctionButtonCliked != null)
            {
                string s = ((Button)sender).Content.ToString();
                FunctionButtonCliked(sender, new StringEventArgs(s));
            }
        }
    }
}
