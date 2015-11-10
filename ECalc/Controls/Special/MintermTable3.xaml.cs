﻿using ECalc.Engineering;
using ECalc.Extensions;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for MintermTable3.xaml
    /// </summary>
    public partial class MintermTable3 : UserControl, IMintermTable
    {
        public MintermTable3()
        {
            InitializeComponent();
        }

        public List<LogicItem> Selected
        {
            get
            {
                List<LogicItem> ret = new List<LogicItem>();
                foreach (var item in MintermTableHelpers.GetMintermTableValues(Minterm3x))
                {
                    ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 3, item.Value));
                }
                return ret;
            }
            set { MintermTableHelpers.SetMintermTableValues(Minterm3x, value); }
        }


        public void ClearInput()
        {
            MintermTableHelpers.ClearMintermtable(Minterm3x);
        }

        public void SetAll(bool? value)
        {
            List<LogicItem> items = new List<LogicItem>();
            for (int i = 0; i < 8; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 3, value);
                items.Add(lo);
            }
            Selected = items;
        }

        public void SwapVarnames()
        {
            foreach (var tb in this.FindChildren<TextBlock>())
            {
                switch (tb.Text)
                {
                    case "A":
                        tb.Text = "C";
                        break;
                    case "B":
                        tb.Text = "B";
                        break;
                    case "C":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
