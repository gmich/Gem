namespace Gem.Network
{
    using System;
    using Lidgren.Network;
    using Gem.Network.Messages;
    using System.Collections.Generic;
    using Gem.Network.Utilities;

    /// <summary>
    /// The server class. Sends and recieves messages
    /// </summary>
    public class Server : IServer
    {

        #region Construct / Dispose

        public Server(NetDeliveryMethod deliveryMethod, int sequenceChannel, string disconnectMessage = "Bye")
        {
            this.disconnectMessage = disconnectMessage;
            this.deliveryMethod = deliveryMethod;
            this.sequenceChannel = sequenceChannel;
        }

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

        public void Dispose()
        {
            this.Dispose(true);
        }
        
        #endregion


        #region  Fields
        
        private readonly int sequenceChannel;
        
        /// <summary>
        /// This is shown when the server shuts down
        /// </summary>
        private readonly string disconnectMessage;
        
        /// <summary>
        /// How the message is delivered
        /// </summary>
        private readonly NetDeliveryMethod deliveryMethod;

        private bool isDisposed;

        private NetServer netServer;
        
        #endregion


        #region IServer Implementation
        
        public void Connect(string serverName, int port)
        {
            var config = new NetPeerConfiguration(serverName)
                {
                    Port = port
                };
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            this.netServer = new NetServer(config);
            this.netServer.Start();
        }
        
        public NetOutgoingMessage CreateMessage()
        {
            return this.netServer.CreateMessage();
        }

        public void Disconnect()
        {
            this.netServer.Shutdown(disconnectMessage);
        }
        
        public NetIncomingMessage ReadMessage()
        {
            return this.netServer.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            this.netServer.Recycle(im);
        }

        /// <summary>
        /// Sends the message to all except the sender
        /// </summary>
        /// <param name="gameMessage">The message to send</param>
        /// <param name="connection">The sender</param>
        public void SendMessage(IServerMessage gameMessage, NetConnection sender)
        {
            this.netServer.SendToAll(CreateOutgoingMessage(gameMessage),
                                    sender,
                                    deliveryMethod,
                                    sequenceChannel);
        }

        /// <summary>
        /// Sends the message to the clients in the list
        /// </summary>
        /// <param name="gameMessage">The message to send</param>
        /// <param name="clients">The clients the message is sent to</param>
        public void SendMessage(IServerMessage gameMessage, List<NetConnection> clients)
        {

            this.netServer.SendMessage(CreateOutgoingMessage(gameMessage),
                                        clients,
                                        deliveryMethod,
                                        sequenceChannel);
        }   
        
        #endregion


        #region Private Helper Methods
       
        private NetOutgoingMessage CreateOutgoingMessage(IServerMessage gameMessage)
        {
            NetOutgoingMessage om = this.netServer.CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.Encode(om);
            return om;
        }

        #endregion

    }
}