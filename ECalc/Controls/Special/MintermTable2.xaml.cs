using ECalc.Engineering;
using ECalc.Extensions;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for MintermTable2.xaml
    /// </summary>
    public partial class MintermTable2 : UserControl, IMintermTable
    {
        public MintermTable2()
        {
            InitializeComponent();
        }

        public List<LogicItem> Selected
        {
            get
            {
                List<LogicItem> ret = new List<LogicItem>();
                foreach (var item in MintermTableHelpers.GetMintermTableValues(Minterm2x))
                {
                    ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 2, item.Value));
                }
                return ret;
            }
            set{ MintermTableHelpers.SetMintermTableValues(Minterm2x, value); }
        }

        public void ClearInput()
        {
            MintermTableHelpers.ClearMintermtable(Minterm2x);
        }

        public void SetAll(bool? value)
        {
            List<LogicItem> items = new List<LogicItem>();
            for (int i=0; i<3; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 2, value);
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
                        tb.Text = "B";
                        break;
                    case "B":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
