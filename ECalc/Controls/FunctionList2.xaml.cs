using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ECalc.Classes;

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
                RenderAlphabeticalView();
            }
        }

        public event StringEventHandler FunctionButtonCliked;

        private void RenderButton(string function, WrapPanel wp)
        {
            Button btn = new Button();
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

        private void BtnExpandColapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RenderCategoryView()
        {
            var categories = (from i in _functions select i.Category).Distinct();
            foreach (var category in categories)
            {
                var functions = from i in _functions where i.Category == category orderby i.Name ascending select i.Name;
                Expander expander = new Expander();
                expander.Header = category;
                expander.IsExpanded = false;
                WrapPanel wp = new WrapPanel();
                expander.Content = wp;
                expander.IsExpanded = true;
                foreach (var function in functions)
                {
                    RenderButton(function, wp);
                }
                CategoryPanel.Children.Add(expander);
            }

        }

        private void RenderAlphabeticalView()
        {
            var abc = (from i in _functions orderby i ascending select i.Name.Substring(0, 1).ToUpper()).Distinct().ToList();
            abc.Add("All");
            abc.Add("Usage");
        }

        private void RenderUsageView()
        {

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
