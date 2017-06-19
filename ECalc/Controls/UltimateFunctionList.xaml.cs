using AppLib.Common.Extensions;
using AppLib.WPF.Extensions;
using ECalc.Classes;
using ECalc.IronPythonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for UltimateFunctionList.xaml
    /// </summary>
    public partial class UltimateFunctionList : UserControl
    {
        private List<FunctionInfo> _functions;

        public static Dictionary<string, uint> UsageStats;

        public UltimateFunctionList()
        {
            InitializeComponent();
        }

        public event EventHandler<string> FunctionButtonCliked;

        private void RenderButton(string function, Panel sp, RoutedEventHandler click, bool big)
        {
            var btn = new Button();
            btn.Content = function;
            btn.ToolTip = function;
            btn.Click += click;

            if (big)
                btn.Style = FindResource("BigBtn") as Style;

            sp.Children.Add(btn);
        }

        internal void FillFunctionList(List<FunctionInfo> functions)
        {
            _functions = functions;
            RenderCategories();
        }

        private void UpdateStat()
        {
            Stat.Text = string.Format("Showing {0} of {1}", Functions.Children.Count, _functions.Count);
        }

        private void ClearFunctionPanel()
        {
            ErrorText.Visibility = Visibility.Collapsed;
            ErrorText.Text = "";
            foreach (Button b in Functions.Children)
            {
                b.Click -= ActivateFunction;
            }
            Functions.Children.Clear();
        }

        private void RenderCategories()
        {
            var categories = (from i in _functions
                              orderby i.Category ascending
                              select i.Category).Distinct();
            RenderButton("Most used", CategoryView, CategorySwitch, false);
            foreach (var category in categories)
                RenderButton(category, CategoryView, CategorySwitch, false);
        }

        private void CategorySwitch(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            var cat = btn.Content.ToString();

            if (cat == "Most used")
            {
                RenderMostUsed();
                return;
            }

            var items = (from i in _functions
                         where i.Category == cat
                         orderby i.Name ascending
                         select i.Name).Distinct();

            ClearFunctionPanel();

            foreach (var item in items)
                RenderButton(item, Functions, ActivateFunction, true);

            CategoryHeader.Text = cat;
            UpdateStat();
        }

        private void RenderMostUsed()
        {
            CategoryHeader.Text = "Most used";
            var names = _functions.Select(f => f.Name);
            var mostused = (from i in names
                            from j in UsageStats
                            where i == j.Key
                            orderby j.Value descending, i ascending
                            select i);

            ClearFunctionPanel();

            if (!mostused.Any())
            {
                ErrorText.Text = "No usage data is available at the moment.\nHere will be a list of your most used functions.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            foreach (var item in mostused)
                RenderButton(item, Functions, ActivateFunction, true);

            UpdateStat();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var matches = (from i in _functions
                           where i.Name.Contains(SearchBox.Text, StringComparison.InvariantCultureIgnoreCase)
                           orderby i.Name ascending
                           select i.Name).Distinct();

            ClearFunctionPanel();

            if (!matches.Any())
            {
                ErrorText.Text = "No function found for the search criteria";
                ErrorText.Visibility = Visibility.Visible;
                UpdateStat();
                return;
            }

            foreach (var match in matches)
                RenderButton(match, Functions, ActivateFunction, true);

            UpdateStat();
        }

        private void ActivateFunction(object sender, RoutedEventArgs e)
        {
            if (FunctionButtonCliked == null) return;
            var btn = sender as Button;
            if (btn == null) return;
            var key = btn.Content.ToString();
            FunctionButtonCliked(sender, key);
            if (UsageStats.ContainsKey(key)) UsageStats[key] += 1;
            else UsageStats.Add(key, 1);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.IsDesignMode()) return;
                UsageStats = ConfigFileHelpers.DeSerializeFunctionUsageStats();
            }
            catch (Exception)
            {
                UsageStats = new Dictionary<string, uint>();
            }
            RenderMostUsed();
        }
    }
}
