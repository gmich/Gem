using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gem.Network
{

    public class Peer : IClient
    {

        #region Declarations

        private NetClient client;

        private bool isDisposed;

        private ConnectionDetails connectionDetails;

        //TODO: provide this somehow
        private readonly string disconnectMessage = "bye";

        public bool IsConnected
        {
            get
            {
                return client.ConnectionStatus == NetConnectionStatus.Connected;
            }
        }

        #endregion


        #region Construct / Dispose

        public Peer() { }
        
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    Disconnect();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
        
        
        #region IClient Implementation

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect(ConnectionDetails connectionDetails, ConnectionApprovalMessage approvalMessage = null)
        {
            this.connectionDetails = connectionDetails;

            var config = new NetPeerConfiguration(connectionDetails.ServerName)
            {
                //Port = serverIP.Port
            };
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();

            if (approvalMessage == null)
            {
                approvalMessage = new ConnectionApprovalMessage();
            }

            var hailMessage = CreateMessage();
            MessageSerializer.Encode(approvalMessage, ref hailMessage);
            client.Connect(connectionDetails.ServerIP, hailMessage);
        }

        public NetOutgoingMessage CreateMessage()
        {
            return client.CreateMessage();
        }

        public void Disconnect()
        {
            if (client != null)
            {
                client.Shutdown(disconnectMessage);
            }
        }

        public NetIncomingMessage ReadMessage()
        {
            return client.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            client.Recycle(im);
        }

        //virtual is set for mocking purposes
        public virtual void SendMessage(NetOutgoingMessage msg)
        {
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }
        
        public void SendMessage<T>(T message)
        {
            var msg = client.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            client.SendMessage(msg, connectionDetails.DeliveryMethod);
        }

        public void SendMessage<T>(T message, byte id)
        {
            var msg = client.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            msg.Write(id);

            client.SendMessage(msg, connectionDetails.DeliveryMethod);
        }

        #endregion

    }
}