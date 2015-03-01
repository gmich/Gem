﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Net;
using System.Collections.Generic;
using Gem.Network.Utilities;
using Gem.Network.Containers;
using Gem.Network.Utilities.Loggers;
using Gem.Network.Extensions;

namespace Gem.Network.Server
{
    public class ServerMessageProcessor : IMessageProcessor
    {

        #region Declarations

        private readonly IServer server;

        public Action<string> Echo;

        private readonly IAppender Write;
       
        #endregion


        #region Constructor

        public ServerMessageProcessor(IServer server)
        {
            this.server = server;
            Write = new ActionAppender(Echo);
        }

        #endregion


        #region Messages

        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (im.ReadByte() == (byte)MessageType.ConnectionApproval)
                        {
                            Write.Info("Incoming Connection");
                            var message = MessageSerializer.Decode<ConnectionApprovalMessage>(im);
                            GemNetwork.ServerConfiguration[GemNetwork.ActiveProfile].ConnectionApprove(server, im.SenderConnection, message);
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                Write.Info("Connected to {0}");
                                break;
                            case NetConnectionStatus.Disconnected:
                                Write.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                if (im.SenderConnection.Status == NetConnectionStatus.Disconnected
                                 || im.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                                { }
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        //broadcast to all exception sender
                        var msg = server.CreateMessage();
                        msg.Write(im);
                        server.SendMessage(msg, im.SenderConnection);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        Write.Warn(im.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Write.Error(im.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        //notify the client 
                        break;
                }
                this.server.Recycle(im);
            }
        }

        #endregion


    }
}