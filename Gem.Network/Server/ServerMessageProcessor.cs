﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Net;
using System.Collections.Generic;
using Gem.Network.Utilities;
using Gem.Network.Containers;
using Gem.Network.Utilities.Loggers;
using Gem.Network.Extensions;
using Gem.Network.Events;

namespace Gem.Network.Server
{
    public class ServerMessageProcessor : IMessageProcessor
    {

        #region Declarations

        private readonly IServer server;

        private readonly IAppender Write;
       
        #endregion


        #region Constructor

        public ServerMessageProcessor(IServer server)
        {
            this.server = server;

            Write = new ActionAppender(GemNetworkDebugger.Echo);
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
                        var approvalMsg = new ConnectionApprovalMessage(im);
                        Write.Info("Incoming Connection");
                        GemNetwork.ServerConfiguration[GemNetwork.ActiveProfile].OnIncomingConnection(server, im.SenderConnection, approvalMsg);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                //im.SenderConnection.Approve();
                                Write.Info("{0} Connected", im.SenderConnection);
                                break;
                            case NetConnectionStatus.Disconnected:
                                Write.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                Write.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        if (im.ReadByte() == GemNetwork.NotificationByte)
                        {
                            var cmd = new ServerNotification(im);
                            GemNetwork.Commander.ExecuteCommand(im.SenderConnection, cmd.Message);
                        }
                        else
                        {
                            var msg = server.CreateMessage();
                            msg.Write(im);
                            server.SendAndExclude(msg, im.SenderConnection);
                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        Write.Warning(im.ReadString());
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