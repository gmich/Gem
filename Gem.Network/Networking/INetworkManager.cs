namespace  Gem.Network
{
    using System;
    using Lidgren.Network;
    using Gem.Network.Messages;


    enum PacketTypes
    {
        LOGIN
    }

    /// <summary>
    /// The network manager.
    /// </summary>
    public interface INetworkManager : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        /// Registers an ip to the server
        /// </summary>
        /// <param name="connection">The incoming connection</param>
        IDisposable RegisterConnection(NetConnection connection,out bool success);
        
        /// <summary>
        /// The connect.
        /// </summary>
        void Start(string serverName, int port);

        /// <summary>
        /// The create message.
        /// </summary>
        /// <returns>
        /// </returns>
        NetOutgoingMessage CreateMessage();

        /// <summary>
        /// The disconnect.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// The read message.
        /// </summary>
        /// <returns>
        /// </returns>
        NetIncomingMessage ReadMessage();

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        void Recycle(NetIncomingMessage im);

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        void SendMessage(IGameMessage gameMessage, NetConnection im);

        #endregion
    }
}