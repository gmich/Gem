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

        public ServerNotification(byte id, string message)
        {
            this.id = id;
            this.Message = message;
        }

        private readonly byte id;
        public byte Id
        {
            get
            {
                return id;
            }
        }

        public string Message { get; set; }

    }
}