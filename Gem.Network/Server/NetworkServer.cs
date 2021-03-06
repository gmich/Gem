﻿using System;
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
    /// The server class. Shows info, starts the server and reads / sends messages
    /// </summary>
    public class NetworkServer : IServer
    {

        #region Private Fields

        private ServerConfig serverConfig;

        private bool isDisposed;

        private NetServer netServer;

        private PackageConfig packageConfig;

        #endregion

        #region Public Readonly Info

        public string Password
        {
            get
            {
                return serverConfig.Password;
            }
        }

        public IPAddress IP
        {
            get
            {
                return this.netServer.Configuration.BroadcastAddress;
            }
        }

        public int Port
        {
            get
            {
                return this.netServer.Configuration.Port;
            }
        }

        public bool IsConnected
        {
            get
            {
                return netServer.Status == NetPeerStatus.Running;
            }
        }

        public PackageConfig PackageConfig
        {
            get
            {
                return packageConfig;
            }
            set
            {
                packageConfig = value;
            }
        }

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

        #endregion
        
        #region Construct / Dispose

        public NetworkServer()
        { }

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

        public bool Connect(ServerConfig serverConfig, PackageConfig packageConfig)
        {
            this.PackageConfig = packageConfig;
            this.serverConfig = serverConfig;
            if (netServer != null)
            {
                GemNetworkDebugger.Append.Error("Server already started");
                return false;
            }
            var config = new NetPeerConfiguration(serverConfig.Name)
                {
                    Port = serverConfig.Port,
                    MaximumConnections = serverConfig.MaxConnections,
                    EnableUPnP = serverConfig.EnableUPnP,
                    ConnectionTimeout = serverConfig.ConnectionTimeout,
                    MaximumHandshakeAttempts = serverConfig.MaxConnectionAttempts
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
                GemNetworkDebugger.Append.Info("Server started");
                return true;
            }
            catch (Exception ex)
            {
                GemNetworkDebugger.Append.Error("Failed to start the server. Reason {0}", ex.Message);
                return false;
            }

        }

        public void Disconnect()
        {
            netServer.Shutdown(serverConfig.DisconnectMessage);
        }

        public void Wait()
        {
            netServer.MessageReceivedEvent.WaitOne();
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

        public void SendMessage<T>(NetConnection sender, T message, byte id)
        {
            var msg = netServer.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            msg.Write(id);

            netServer.SendToAll(msg,
                                sender,
                                packageConfig.DeliveryMethod,
                                packageConfig.SequenceChannel);
        }

        public void SendMessage<T>(T message, byte id)
        {
            var msg = netServer.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            msg.Write(id);
            netServer.SendToAll(msg, packageConfig.DeliveryMethod);
        }

        /// <summary>
        /// Sends the message to all 
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendToAll(NetOutgoingMessage message)
        {
            netServer.SendToAll(message, packageConfig.DeliveryMethod);
        }

        /// <summary>
        /// Sends the message to all except the sender
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="connection">The sender</param>
        public void SendAndExclude(NetOutgoingMessage message, NetConnection sender)
        {
            netServer.SendToAll(message,
                                sender,
                                packageConfig.DeliveryMethod,
                                packageConfig.SequenceChannel);
        }

        /// <summary>
        /// Sends the message to the clients in the list
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="clients">The clients the message is sent to</param>
        public void SendMessage(NetOutgoingMessage message, List<NetConnection> clients)
        {

            netServer.SendMessage(message,
                                  clients,
                                  packageConfig.DeliveryMethod,
                                  packageConfig.SequenceChannel);
        }

        /// <summary>
        /// Sends the message to the clients in the list
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="clients">The clients the message is sent to</param>
        public void SendOnlyTo(NetOutgoingMessage message, NetConnection client)
        {
            if (client != null)
            {
                netServer.SendMessage(message,
                                      new List<NetConnection> { client },
                                      packageConfig.DeliveryMethod,
                                      packageConfig.SequenceChannel);
            }
        }
        #endregion
        
        #region Server Management
        
        public bool Kick(IPAddress clientIp, string reason)
        {
            var netconnection = netServer.Connections.Where(x => x.RemoteEndpoint.Address == clientIp).Select(x => x.RemoteEndpoint).FirstOrDefault();

            if (netconnection != null)
            {
                netServer.GetConnection(netconnection).Disconnect(reason);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Kick(IPEndPoint clientIp, string reason)
        {
            netServer.GetConnection(clientIp).Disconnect(reason);
            return true;
        }

        #endregion


        #region Notifications

        public void SendNotification(Notification notification)
        {
            GemNetworkDebugger.Append.Info(String.Format("Sent to all :  {0}  of type {1}", notification.Message, notification.Type));
            var om = netServer.CreateMessage();
            MessageSerializer.Encode(notification, ref om);
            SendToAll(om);
        }

        public void NotifyAll(string message)
        {
            GemNetworkDebugger.Append.Info(String.Format("Sent to all :  {0}", message));
            var serverNotification = new Notification(message, NotificationType.Message);
            var om = netServer.CreateMessage();
            MessageSerializer.Encode(serverNotification, ref om);
            SendToAll(om);
        }

        public void NotifyAllExcept(string message, NetConnection client, string type = NotificationType.Message)
        {
            GemNetworkDebugger.Append.Info(String.Format("Sent to all :  {0}", message));
            var serverNotification = new Notification(message, type);
            var om = netServer.CreateMessage();
            MessageSerializer.Encode(serverNotification, ref om);
            SendAndExclude(om, client);
        }

        public void NotifyOnly(string message, NetConnection client, string type = NotificationType.Message)
        {
            if (client != null)
            {
                GemNetworkDebugger.Append.Info(String.Format("{0}  :  {1}", client, message));
                var serverNotification = new Notification(message, type);
                var om = netServer.CreateMessage();
                MessageSerializer.Encode(serverNotification, ref om);
                SendOnlyTo(om, client);
            }
            else
            {
                GemNetworkDebugger.Append.Info(String.Format("Server  :  {0}", message));
            }
        }

        #endregion


    }
}