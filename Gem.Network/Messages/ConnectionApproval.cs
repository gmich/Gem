using Gem.Network.Messages;
using Lidgren.Network;


namespace Gem.Network.Messages
{
    public class ConnectionApproval : IServerMessage
    {

        public ConnectionApproval(NetIncomingMessage im)
        {
            this.Decode(im);
        }


        #region Public Properties
        
        public string Message { get; private set; }

        public string Sender { get; private set; }

        public MessageType MessageType
        {
            get
            {
                return MessageType.Data;
            }
        }
        
        #endregion


        public void Decode(NetIncomingMessage im)
        {
            this.Message = im.ReadString();
            this.Sender = im.ReadString();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Message);
            om.Write(this.Sender);
        }

    }
}
