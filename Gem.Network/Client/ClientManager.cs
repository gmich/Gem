﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Net;
using System.Collections.Generic;
using Gem.Network.Utilities;
using Gem.Network.Containers;
using Gem.Network.Utilities.Loggers;

namespace Gem.Network
{
    public class ClientManager : IDisposable
    {

        #region Declarations

        private readonly INetworkManager client;

        private readonly NetworkDirector networkDirector;

        public Action<string> Echo;

        private readonly IAppender Write;

        public bool IsRunning { get; private set; } 
        #endregion


        #region Constructor

        public ClientManager(IClient client, ConnectionDetails connectionDetails)
        {
            Write = new ActionAppender(Echo);

            this.client = client;
            try
            {
                client.Connect(connectionDetails);
                IsRunning = true;
            }
            catch (Exception ex)
            {
                //TODO: log this
                IsRunning = false;
            }
        }

        #endregion
        
        #region Close Connection

        public void Disconnect()
        {
            client.Disconnect();
            IsRunning = false;

        }

        public void Dispose()
        {
            Disconnect();
        }

        #endregion


        #region Messages

        private void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.client.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        //handle 
                        Write.Info("Approved new client");
                        break;
                    case NetIncomingMessageType.Data:
                        var messageType = (MessageType)im.ReadByte();

                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        Write.Debug(im.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Write.Error(im.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                }
                this.client.Recycle(im);
            }
        }

        #endregion


    }
}