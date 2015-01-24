// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerNetworkManager.cs" company="">
//   
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace  Gem.Network
{
    using System;
    using Lidgren.Network;
    using  Gem.Network.Messages;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServerNetworkManager : INetworkManager
    {

        #region Constants and Fields

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The net server.
        /// </summary>
        private NetServer netServer;

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// The connect.
        /// </summary>
        public void Connect(string serverName,string IP)
        {
            var config = new NetPeerConfiguration(serverName)
                {
                    Port = Convert.ToInt32("14242"), 
                    // SimulatedMinimumLatency = 0.2f, 
                    // SimulatedLoss = 0.1f 
                };
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this.netServer = new NetServer(config);
            this.netServer.Start();
        }

        /// <summary>
        /// The create message.
        /// </summary>
        /// <returns>
        /// </returns>
        public NetOutgoingMessage CreateMessage()
        {
            return this.netServer.CreateMessage();
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Disconnect()
        {
            this.netServer.Shutdown("Bye");
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
            return this.netServer.ReadMessage();
        }

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public void Recycle(NetIncomingMessage im)
        {
            this.netServer.Recycle(im);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        public void SendMessage(IGameMessage gameMessage)
        {
            NetOutgoingMessage om = this.netServer.CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.Encode(om);
            //TODO update SendToAll to SendToAll + exception overload
            this.netServer.SendToAll(om, NetDeliveryMethod.ReliableUnordered);
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