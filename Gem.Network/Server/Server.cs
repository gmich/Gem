namespace Gem.Network
{
    using System;
    using Lidgren.Network;
    using Gem.Network.Messages;
    using System.Collections.Generic;
    using Gem.Network.Utilities;
    using System.Net;

    /// <summary>
    /// The server class. Sends and recieves messages
    /// </summary>
    public class Server : IServer
    {

        #region Construct / Dispose

        public Server(NetDeliveryMethod deliveryMethod, int sequenceChannel, int maxConnections = 4 , string disconnectMessage = "Bye")
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

        //TODO: refactor
        public IPAddress IP
        {
            get
            {
                return this.netServer.Configuration.LocalAddress;
            }
        }

        //TODO: refactor
        public int Port
        {
            get
            {
                return this.netServer.Configuration.Port;
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
                    //MaximumConnections = maxConnections
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
        /// Sends the message to all 
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(NetOutgoingMessage message)
        {
            this.netServer.SendToAll(message, deliveryMethod);
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