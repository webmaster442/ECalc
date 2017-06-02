using System;

namespace ECalc.Classes
{
    public class CopyPasteData
    {
        public DateTime PasteDate { get; private set; }
        public string Data { get; private set; }

        public CopyPasteData(string o)
        {
            PasteDate = DateTime.Now;
            Data = o;
        }
    }
}
