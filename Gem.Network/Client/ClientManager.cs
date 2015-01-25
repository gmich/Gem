namespace Gem.Network.Client
{
    using System;
    using Lidgren.Network;
    using Gem.Network.Messages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// The server class
    /// </summary>
    public class ClientManager : INetworkManager
    {

        #region Constructor

        public ClientManager(IPEndPoint serverIP)
        {
            deliveryMethod = NetDeliveryMethod.ReliableUnordered;
            disconnectMessage = "bye";
            sequenceChannel = 1;
            this.serverIP = serverIP;
        }

        #endregion


        #region Constants and Fields

        private NetClient client;

        private readonly IPEndPoint serverIP;

        /// <summary>
        /// The message delivery sequence channel
        /// </summary>
        private readonly int sequenceChannel;

        /// <summary>
        /// How the message is delivered
        /// </summary>
        private readonly NetDeliveryMethod deliveryMethod;

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// This is shown when the server shuts down
        /// </summary>
        private readonly string disconnectMessage;

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Initialize a new connection
        /// </summary>
        public void Connect(string serverName, int port)
        {
            var config = new NetPeerConfiguration(serverName)
            {
                Port = Convert.ToInt32(port)
            };
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            client = new NetClient(config);
            client.Start();

            //Configure a connection message
            NetOutgoingMessage om = this.client.CreateMessage();
            //om.Write((byte)gameMessage.MessageType);
            //gameMessage.Encode(om);
            client.Connect(serverIP, om);
            //TODO: configure wait for approval
            //configure server discovery response
        }


        /// <summary>
        /// The create message.
        /// </summary>
        /// <returns>
        /// </returns>
        public NetOutgoingMessage CreateMessage()
        {
            return this.client.CreateMessage();
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Disconnect()
        {
            this.client.Shutdown(disconnectMessage);
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
            return this.client.ReadMessage();
        }

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public void Recycle(NetIncomingMessage im)
        {
            this.client.Recycle(im);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        public void SendMessage(IServerMessage gameMessage, NetConnection connection)
        {
            NetOutgoingMessage om = this.client.CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.Encode(om);

            this.client.SendMessage(om, deliveryMethod, sequenceChannel);
        }

        #endregion


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




        public IDisposable RegisterConnection(NetConnection connection, out bool success)
        {
            throw new NotImplementedException();
        }

        public void Start(string serverName, int port)
        {
            throw new NotImplementedException();
        }

        public IDisposable RegisterConnection(NetConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}