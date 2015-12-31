﻿using ECalc.Api.Extensions;
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
            var expanders = CategoryPanel.FindChildren<Expander>();
            bool expand = false;
            if (BtnExpandColapse.Content.ToString() == "Colapse all")
            {
                BtnExpandColapse.Content = "Expand all";
            }
            else
            {
                BtnExpandColapse.Content = "Colapse all";
                expand = true;
            }
            foreach (var expander in expanders) expander.IsExpanded = expand;
        }

        private void RenderCategoryView()
        {
            var categories = (from i in _functions orderby i.Category ascending select i.Category).Distinct();
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

        private void RenderAlphabeticalViewHeader()
        {
            var abc = (from i in _functions orderby i.Name ascending select i.Name.Substring(0, 1).ToUpper()).Distinct().ToList();
            abc.Add("All");
            abc.Add("Usage");
            LetterSelector.ItemsSource = abc;
        }

        private void RenderAlphabeticalView(string letter)
        {
            string[] items = null;
            if (letter == "All") items = (from i in _functions orderby i.Name select i.Name).ToArray();
            else items = (from i in _functions where i.Name.StartsWith(letter) orderby i.Name ascending select i.Name).ToArray();
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
            var usageinfo = (from i in _functions from j in UsageStats
                             where i.Name == j.Key
                             orderby j.Value descending, i.Name ascending
                             select i.Name).ToArray();
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
