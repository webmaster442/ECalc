using ECalc.Classes;
using ECalc.Engineering;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for MintermTable.xaml
    /// </summary>
    public partial class MintermTable4 : UserControl, IMintermTable
    {
        public MintermTable4()
        {
            InitializeComponent();
        }

        public List<LogicItem> Selected
        {
            get
            {
                List<LogicItem> ret = new List<LogicItem>();
                foreach (var item in Functions.GetMintermTableValues(Minterm4x))
                {
                    ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 4, item.Value));
                }
                return ret;
            }
            set { Functions.SetMintermTableValues(Minterm4x, value); }
        }

        public void ClearInput()
        {
            Functions.ClearMintermtable(Minterm4x);
        }

        public void SetAll(bool? value)
        {
            List<LogicItem> items = new List<LogicItem>();
            for (int i = 0; i < 16; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 4, value);
                items.Add(lo);
            }
            Selected = items;
        }

        public void SwapVarnames()
        {
            foreach (var tb in Helpers.FindChildren<TextBlock>(this))
            {
                switch (tb.Text)
                {
                    case "A":
                        tb.Text = "D";
                        break;
                    case "B":
                        tb.Text = "C";
                        break;
                    case "C":
                        tb.Text = "B";
                        break;
                    case "D":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
