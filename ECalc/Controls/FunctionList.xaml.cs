using ECalc.Classes;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for FunctionList.xaml
    /// </summary>
    internal partial class FunctionList : UserControl
    {
        public static Dictionary<string, uint> UsageStats;

        private bool _loaded;

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
            try
            {
                bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
                if (designTime) return;
                UsageStats = ConfigFileHelpers.DeSerializeUsageStats();
            }
            catch (Exception)
            {
                UsageStats = new Dictionary<string, uint>();
            }
            _loaded = true;
        }

        public void Render()
        {
            string[] query = null;
            if (UsageStats != null)
            {
                if (UsageStats.Count > 0 && RbUsage.IsChecked == true) query = (from i in Functions from j in UsageStats where i == j.Key orderby j.Value descending select i).ToArray();
                else query = (from i in Functions orderby i ascending select i).ToArray();
            }
            else query = (from i in Functions orderby i ascending select i).ToArray();
            Container.Children.Clear();
            foreach (var item in query)
            {
                Button b = new Button();
                b.Content = item;
                b.Click += b_Click;
                Container.Children.Add(b);
            }
        }

        private void IncrementStat(string key)
        {
            if (UsageStats.ContainsKey(key)) UsageStats[key] += 1;
            else UsageStats.Add(key, 1);
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            if (FunctionButtonCliked != null)
            {
                string s = ((Button)sender).Content.ToString();
                FunctionButtonCliked(sender, new StringEventArgs(s));
                IncrementStat(s);
            }
        }

        private void RbChecked(object sender, RoutedEventArgs e)
        {
            if (_loaded) Render();
        }
    }
}
