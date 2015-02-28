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

        private string Profile;

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
                try
                {
                    var config = networkDirector[Profile, im.MessageType.Transform(), im.ReadByte()];

                }
                catch (Exception ex)
                {
                    Write.Error("Unable to read incoming message. Reason: " + ex.Message);
                }
                client.Recycle(im);
            }
        }

        #endregion


    }
}