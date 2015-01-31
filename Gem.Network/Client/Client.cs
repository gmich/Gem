namespace Gem.Network.Client
{
    using System;
    using Lidgren.Network;
    using Gem.Network.Messages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class Client : IClient
    {

        #region Construct / Dispose

        public Client(IPEndPoint serverIP,ConnectionDetails connectionDetails)
        {
            this.serverIP = serverIP;
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


        #region Declarations

        private NetClient client;

        /// <summary>
        /// The server's ip
        /// </summary>
        private readonly IPEndPoint serverIP;
    
        private readonly int sequenceChannel;

        private readonly NetDeliveryMethod deliveryMethod;

        private bool isDisposed;

        private readonly string disconnectMessage;

        #endregion
        
        #region IClient Implementation

        /// <summary>
        /// Connect to the server
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
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

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

        public NetOutgoingMessage CreateMessage()
        {
            return client.CreateMessage();
        }
             
        public void Disconnect()
        {
            client.Shutdown(disconnectMessage);
        }

        public NetIncomingMessage ReadMessage()
        {
            return client.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            client.Recycle(im);
        }
        
        public void SendMessage(IServerMessage gameMessage)
        {
            NetOutgoingMessage om = this.client.CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.Encode(om);

            client.SendMessage(om, deliveryMethod, sequenceChannel);
        }

        #endregion
        
    }
}