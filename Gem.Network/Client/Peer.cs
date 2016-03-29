using System;
using Lidgren.Network;
using Gem.Network.Messages;
using Gem.Network.Server;

namespace Gem.Network.Client
{

    public class Peer : IClient
    {

        #region Declarations

        /// <summary>
        /// The lidgren's netclient
        /// </summary>
        private NetClient client;

        /// <summary>
        /// Shows if the object's members have been garbage collected 
        /// </summary>
        private bool isDisposed;

        private ConnectionConfig connectionDetails;

        public bool IsConnected
        {
            get
            {
                return client.ConnectionStatus == NetConnectionStatus.Connected;
            }
        }

        private PackageConfig packageConfig;
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

        #endregion


        #region Construct / Dispose

        
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
        public void Connect(ConnectionConfig connectionDetails,PackageConfig packageConfig, ConnectionApprovalMessage approvalMessage)
        {
            this.connectionDetails = connectionDetails;
            this.PackageConfig = packageConfig;

            var config = new NetPeerConfiguration(connectionDetails.ServerName);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
  
            client = new NetClient(config);
            client.Start();
            //encrypt passwords ? 
            //INetEncryption algo = new NetTripleDESEncryption(encryptionKey);
            //handshake.Encrypt(algo);

            //Create the approval message
            var handshake = CreateMessage();
            approvalMessage.Encode(handshake);

            client.Connect(connectionDetails.ServerIP, handshake);
        }

        public void Wait()
        {
            this.client.MessageReceivedEvent.WaitOne();
        }
        
        public void Disconnect()
        {
            if (client != null)
            {
                client.Shutdown(connectionDetails.DisconnectMessage);
            }
        }

        public NetOutgoingMessage CreateMessage()
        {
            return client.CreateMessage();
        }

        public NetIncomingMessage ReadMessage()
        {
            return client.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            client.Recycle(im);
        }

        public void SendMessage(NetOutgoingMessage msg)
        {
            client.SendMessage(msg, PackageConfig.DeliveryMethod, PackageConfig.SequenceChannel);
        }
        
        public void SendMessage<T>(T message)
        {
            var msg = client.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            client.SendMessage(msg, PackageConfig.DeliveryMethod,PackageConfig.SequenceChannel);
        }

        public void SendMessage<T>(object[] args)
        {
            var om = client.CreateMessage();
            var msg = (T)Activator.CreateInstance(typeof(T), args);
            MessageSerializer.Encode(msg, ref om);
            client.SendMessage(om, PackageConfig.DeliveryMethod, PackageConfig.SequenceChannel);
        }

        public void SendMessage<T>(T message, byte id)
        {
            var msg = client.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            msg.Write(id);
            client.SendMessage(msg, PackageConfig.DeliveryMethod, PackageConfig.SequenceChannel);
        }

        public void SendMessageWithLocalTime<T>(T message, byte id)
        {
            var msg = client.CreateMessage();
            MessageSerializer.Encode(message, ref msg);
            msg.Write(id);
            msg.WriteTime(NetTime.Now, true);
            client.SendMessage(msg, PackageConfig.DeliveryMethod, PackageConfig.SequenceChannel);
        }

        public void SendNotification(Notification notification)
        {
            var msg = client.CreateMessage();
            MessageSerializer.Encode(notification, ref msg);
            client.SendMessage(msg, PackageConfig.DeliveryMethod, PackageConfig.SequenceChannel);
        }

        #endregion

    }
}