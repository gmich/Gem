namespace  Gem.Network.Messages
{
    using Lidgren.Network;

    /// <summary>
    /// The game message
    /// </summary>
    public interface IServerMessage
    {

        #region Public Properties

        MessageType MessageType { get; }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// The decode.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        void Decode(NetIncomingMessage im);

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="om">
        /// The om.
        /// </param>
        void Encode(NetOutgoingMessage om);

        #endregion
    }
}