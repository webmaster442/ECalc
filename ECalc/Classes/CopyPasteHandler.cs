using AppLib.Common;
using AppLib.Common.MessageHandler;
using System.Collections.ObjectModel;

namespace ECalc.Classes
{
    public class CopyPasteHandler: ObservableCollection<CopyPasteData>, IMessageTarget<CopyPasteData>
    {
        public CopyPasteHandler(): base()
        {
            MessageSender.Instance.SubScribe(this);
            MessageReciverID = UId.Create();
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
