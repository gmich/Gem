namespace  Gem.Network.Example.Messages
{
    using Gem.Network.Messages;
    using Gem.Network.Example.Messages;
    using Lidgren.Network;
    
    public class ChatMessage : IServerMessage
    {

        #region Constructors and Destructors

        public ChatMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        #endregion


        #region Public Properties

        public string Message { get; private set; }

        public string Sender { get; private set; }

        public IncomingMessageTypes MessageType
        {
            get
            {
                return IncomingMessageTypes.Data;
            }
        }

        public DataMessageTypes DataMessageType
        {
            get
            {
                return DataMessageTypes.NewChatMessage;
            }
        }

        #endregion


        #region Public Methods and Operators

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

        #endregion
    }
}