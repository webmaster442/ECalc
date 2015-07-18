using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Classes
{
    public class ConstantList : ObservableCollection<MemoryItem> 
    {
        public ConstantList(): base()
        {
            this.Add(new MemoryItem("&Pi", Math.PI));
            this.Add(new MemoryItem("&E", Math.E));
            this.Add(new MemoryItem("&Phi", 1.6180339887498948482045868343d));
        }
    }
}
