using ECalc.Classes;
using ECalc.Engineering;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for MintermTable5.xaml
    /// </summary>
    public partial class MintermTable5 : UserControl, IMintermTable
    {
        public MintermTable5()
        {
            InitializeComponent();
        }

        public List<LogicItem> Selected
        {
            get
            {
                List<LogicItem> ret = new List<LogicItem>();
                foreach (var item in Functions.GetMintermTableValues(Minterm5x))
                {
                    ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 5, item.Value));
                }
                return ret;
            }
            set { Functions.SetMintermTableValues(Minterm5x, value); }
        }


        public void ClearInput()
        {
            Functions.ClearMintermtable(Minterm5x);
        }

        public void SetAll(bool? value)
        {
            List<LogicItem> items = new List<LogicItem>();
            for (int i = 0; i < 32; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 5, value);
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
                        tb.Text = "E";
                        break;
                    case "B":
                        tb.Text = "D";
                        break;
                    case "C":
                        tb.Text = "C";
                        break;
                    case "D":
                        tb.Text = "B";
                        break;
                    case "E":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
