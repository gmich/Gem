namespace  Gem.Network.Messages
{
    using Lidgren.Network;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RemoveEntityMessage : IGameMessage
    {
        #region Constructors and Destructors

        public RemoveEntityMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public RemoveEntityMessage(int entityID, int infoID, int senderColor)
        {
            this.EntityID = entityID;
            this.InfoID = infoID;
            this.SenderColor = senderColor;
        }

        #endregion

        #region Public Properties

        public int EntityID { get; private set; }

        public int InfoID { get; private set; }

        public int SenderColor { get; private set; }

        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.RemoveEntityState;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Decode(NetIncomingMessage im)
        {
            this.EntityID = im.ReadInt32();
            this.InfoID = im.ReadInt32();
            this.SenderColor = im.ReadInt32();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.EntityID);
            om.Write(this.InfoID);
            om.Write(this.SenderColor);
        }

        #endregion
    }
}