using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Collections.Generic;
using Gem.Network.Utilities;
using System.Net;
using Gem.Network.Utilities.Loggers;
using Gem.Network;
using System.Linq;

namespace Gem.Network.Server
{

    /// <summary>
    /// The server class. Sends and recieves messages
    /// </summary>
    public class NetworkServer : IServer
    {

        #region Fields

        public string Password { get { return serverConfig.Password; } }

        //TODO: provide this somehow
        private readonly string disconnectMessage = "bye";
        
        private ServerConfig serverConfig;

        private bool isDisposed;

        private NetServer netServer;
        
        private readonly IAppender appender;

        public IPAddress IP
        {
            get
            {  return this.netServer.Configuration.LocalAddress;  }
        }

        public int Port
        {
            get
            { return this.netServer.Configuration.Port;   }
        }
        
        #endregion


        #region Construct / Dispose

        public NetworkServer(Action<string> DebugListener)
        {
            this.appender = new ActionAppender(DebugListener);       
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


        #region Connect / Disconnect

        public bool Connect(ServerConfig serverConfig)
        {
            this.serverConfig = serverConfig;
            if (netServer != null)
            {
                appender.Error("Server already started");
                return false;
            }
            var config = new NetPeerConfiguration(serverConfig.Name)
                {
                    Port = serverConfig.Port,
                    MaximumConnections = serverConfig.MaxConnections
                };
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            this.netServer = new NetServer(config);

            try
            {
                this.netServer.Start();
                appender.Info("Server started");
                return true;
            }
            catch (Exception ex)
            {
                appender.Error("Failed to start the server. Reason ", ex.Message);
                return false;
            }

        }

        public void Disconnect()
        {
            netServer.Shutdown(disconnectMessage);
        }

        #endregion


        #region Message Related

        public NetOutgoingMessage CreateMessage()
        {
            return netServer.CreateMessage();
        }
                
        public NetIncomingMessage ReadMessage()
        {
            return netServer.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            netServer.Recycle(im);
        }

        /// <summary>
        /// Sends the message to all 
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(NetOutgoingMessage message)
        {
            netServer.SendToAll(message, serverConfig.DeliveryMethod);
        }

        /// <summary>
        /// Sends the message to all except the sender
        /// </summary>
        /// <param name="gameMessage">The message to send</param>
        /// <param name="connection">The sender</param>
        public void SendMessage(NetOutgoingMessage gameMessage, NetConnection sender)
        {
            netServer.SendToAll(CreateOutgoingMessage(gameMessage),
                                    sender,
                                    serverConfig.DeliveryMethod,
                                    serverConfig.SequenceChannel);
        }

        /// <summary>
        /// Sends the message to the clients in the list
        /// </summary>
        /// <param name="gameMessage">The message to send</param>
        /// <param name="clients">The clients the message is sent to</param>
        public void SendMessage(NetOutgoingMessage gameMessage, List<NetConnection> clients)
        {

            netServer.SendMessage(CreateOutgoingMessage(gameMessage),
                                        clients,
                                        serverConfig.DeliveryMethod,
                                        serverConfig.SequenceChannel);
        }   
        
        private NetOutgoingMessage CreateOutgoingMessage(object gameMessage)
        {
            return netServer.CreateMessage();
        }

        #endregion


        #region Server Management

        public List<IPEndPoint> ClientsIP
        {
            get
            {
                return netServer.Connections.Select(x => x.RemoteEndpoint).ToList();
            }
        }

        public int ClientsCount
        {
            get
            {
                return netServer.ConnectionsCount;
            }
        }

        public void Kick(IPEndPoint clientIp, string reason)
        {
            netServer.GetConnection(clientIp).Disconnect(reason);
        }

        #endregion

    }
}