using AppLib.Common;
using AppLib.Common.MessageHandler;
using System.Collections.ObjectModel;

namespace ECalc.Classes
{
    public class CopyPasteHandler: ObservableCollection<CopyPasteData>, IMessageClient<CopyPasteData>
    {
        public CopyPasteHandler(): base()
        {
            Messager.Instance.SubScribe(this);
            MessageReciverID = new UId();
        }

        public UId MessageReciverID
        {
            get;
            private set;
        }

        public void HandleMessage(CopyPasteData message)
        {
            Add(message);
        }
    }
}
