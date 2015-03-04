using Gem.Network.Events;
using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network.Messages
{
    public partial class ServerNotification
    {
        public ServerNotification(NetIncomingMessage im)
        {
            Message = im.ReadString();
        }

        public ServerNotification()
        {}

        public string Message { get; set; }

        private readonly byte _id = 0;
        public byte Id
        {
            get
            {
                return _id;
            }
            set
            { }
        }
        
        public void Encode(NetOutgoingMessage om)
        {
            om.Write(Message);
            om.Write(Id);
        }
    }
}