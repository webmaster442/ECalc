using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for FunctionList2.xaml
    /// </summary>
    public partial class FunctionList2 : UserControl
    {
        private IFunction[] _functions;
        public static Dictionary<string, uint> UsageStats;

        public FunctionList2()
        {
            InitializeComponent();
        }

        internal IFunction[] Funtions
        {
            get { return _functions; }
            set
            {
                _functions = value;
                RenderCategoryView();
                RenderAlphabeticalViewHeader();
                RenderAlphabeticalView("all");
            }
        }

        public event StringEventHandler FunctionButtonCliked;

        private void RenderButton(string function, WrapPanel wp)
        {
            var btn = new Button();
            btn.Content = function;
            btn.Click += Btn_Click;
            wp.Children.Add(btn);
        }


        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (FunctionButtonCliked != null)
            {
                string key = ((Button)sender).Content.ToString();
                FunctionButtonCliked(sender, new StringEventArgs(key));
                if (UsageStats.ContainsKey(key)) UsageStats[key] += 1;
                else UsageStats.Add(key, 1);
            }
        }

        private void RenderCategoryView()
        {
            var categories = (from i in _functions orderby i.Category ascending select i.Category).Distinct();
            foreach (var category in categories)
            {
                var functions = from i in _functions where i.Category == category orderby i.Name ascending select i.Name;
                var group = new GroupBox();
                group.Header = category;
                var wp = new WrapPanel();
                group.Content = wp;
                foreach (var function in functions)
                {
                    RenderButton(function, wp);
                }
                CategoryPanel.Children.Add(group);
            }

        }

        private void RenderAlphabeticalViewHeader()
        {

            var q1 = (from i in _functions orderby i.Name ascending select i.Name.Substring(0, 1).ToUpper()).Distinct().ToList();
            q1.Add("All");
            q1.Add("Usage");
            LetterSelector.ItemsSource = q1;
        }

        private void RenderAlphabeticalView(string letter)
        {
            string[] items = null;
            if (letter == "All")
            {
                var q1 = from i in _functions orderby i.Name ascending select i.Name;
                items = q1.ToArray();
            }
            else
            {
                var q1 = from i in _functions where i.Name.StartsWith(letter) orderby i.Name ascending select i.Name;
                items = q1.ToArray();
            }
            AlphabetPanel.Children.Clear();
            foreach (var item in items)
            {
                RenderButton(item, AlphabetPanel);
            }
        }

        private void BtnAlphabet_Click(object sender, RoutedEventArgs e)
        {
            var content = ((Button)sender).Content.ToString();
            if (content == "Usage") RenderUsageView();
            else RenderAlphabeticalView(content);
        }

        private void RenderUsageView()
        {

            var names = _functions.Select(q => q.Name);

            var usageinfo = (from i in names from j in UsageStats
                             where i == j.Key
                             orderby j.Value descending, i ascending
                             select i).ToArray();
            AlphabetPanel.Children.Clear();
            foreach (var item in usageinfo)
            {
                RenderButton(item, AlphabetPanel);
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
                if (designTime) return;
                UsageStats = ConfigFileHelpers.DeSerializeFunctionUsageStats();
            }
            catch (Exception)
            {
                UsageStats = new Dictionary<string, uint>();
            }
        }
    }
}
