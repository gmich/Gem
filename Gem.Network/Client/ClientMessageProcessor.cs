﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Net;
using System.Collections.Generic;
using Gem.Network.Utilities;
using Gem.Network.Containers;
using Gem.Network.Utilities.Loggers;
using Gem.Network.Extensions;

namespace Gem.Network
{
    public class ClientMessageProcessor : IMessageProcessor
    {

        #region Declarations

        private readonly INetworkPeer client;

        public Action<string> Echo;

        private readonly IAppender Write;
       

        #endregion


        #region Constructor

        public ClientMessageProcessor(IClient client)
        {
            Write = new ActionAppender(Echo);
        }

        #endregion


        #region Messages

        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.client.ReadMessage()) != null)
            {
                try
                {
                    GemNetwork.ClientMessageFlowManager[GemNetwork.ActiveProfile,im.MessageType.Transform(), im.ReadByte()]
                              .HandleIncomingMessage(im);
                }
                catch (Exception ex)
                {
                    Write.Error("Unable to handle incoming message. Reason: " + ex.Message);
                }

                client.Recycle(im);
            }
        }

        #endregion


    }
}