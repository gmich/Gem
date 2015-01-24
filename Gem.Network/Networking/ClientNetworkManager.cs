namespace  Gem.Network
{
    using System;
    using System.Net;

    using Lidgren.Network;

    using  Gem.Network.Messages;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClientNetworkManager : INetworkManager
    { 

        #region Constants and Fields

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The net client.
        /// </summary>
        private NetClient netClient;

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// The connect.
        /// </summary>
        public void Connect(string serverName, string IP)
        {
            var config = new NetPeerConfiguration(serverName)
                {
                    //SimulatedMinimumLatency = 0.2f, 
                    // SimulatedLoss = 0.1f
                };

            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this.netClient = new NetClient(config);
            this.netClient.Start();

            this.netClient.Connect(new IPEndPoint(NetUtility.Resolve(IP), Convert.ToInt32("14242")));

        }

        /// <summary>
        /// The create message.
        /// </summary>
        /// <returns>
        /// </returns>
        public NetOutgoingMessage CreateMessage()
        {
            return this.netClient.CreateMessage();
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Disconnect()
        {
            this.netClient.Disconnect("Bye");
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// The read message.
        /// </summary>
        /// <returns>
        /// </returns>
        public NetIncomingMessage ReadMessage()
        {
            return this.netClient.ReadMessage();
        }

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public void Recycle(NetIncomingMessage im)
        {
            this.netClient.Recycle(im);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        public void SendMessage(IGameMessage gameMessage)
        {
            NetOutgoingMessage om = this.netClient.CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.Encode(om);

            this.netClient.SendMessage(om, NetDeliveryMethod.ReliableUnordered);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.Disconnect();
                }

                this.isDisposed = true;
            }

        }

        #endregion

    }
}