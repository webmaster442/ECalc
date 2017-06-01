using System;

namespace ECalc.Classes
{
    public class CopyPasteData
    {
        public DateTime PasteDate { get; private set; }
        public object Data { get; private set; }

        public CopyPasteData(object o)
        {
            PasteDate = DateTime.Now;
            Data = o;
        }
    }
}
