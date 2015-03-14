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
using Gem.Network.Client;

namespace Gem.Network
{
    public class ClientMessageProcessor : IMessageProcessor
    {

        #region Declarations

        private readonly IClient client;

        private readonly IAppender Write;

        #endregion


        #region Constructor

        public ClientMessageProcessor(IClient client)
        {
            this.client = client;
            Write = new ActionAppender(GemNetworkDebugger.Echo);
        }

        #endregion


        #region Messages

        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;
            client.Wait();

            while ((im = this.client.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.InitiatedConnect:
                                Write.Info("Connected to {0}", im.SenderConnection);
                                MessageType.Connecting.Handle(im,client);
                                break;
                            case NetConnectionStatus.Connected:
                                Write.Info("Connected to {0}", im.SenderConnection);
                                MessageType.Connected.Handle(im, client);
                                break;
                            case NetConnectionStatus.Disconnecting:
                                Write.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                MessageType.Disconnecting.Handle(im, client);
                                break;
                            case NetConnectionStatus.Disconnected:
                                Write.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                MessageType.Disconnected.Handle(im, client);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        MessageType.Data.Handle(im, client);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        Write.Warning(im.ReadString());
                        //MessageType.Warning.ClientHandle(im); 
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Write.Error(im.ReadString());
                        //MessageType.Error.ClientHandle(im); 
                        break;
                }

                client.Recycle(im);
            }

        }

        #endregion

    }

    internal static class MessageTypeExtensions
    {
        public static void Handle(this MessageType messageType, NetIncomingMessage im, IClient client)
        {
            byte id = im.ReadByte();

            if (id == GemNetwork.NotificationByte)
            {
                GemClient.ActionManager[GemNetwork.ActiveProfile].OnReceivedNotification(new Notification(im));
                return;
            }

            GemClient.ActionManager[GemNetwork.ActiveProfile, messageType].Action(client);

            if (GemClient.MessageFlow[GemNetwork.ActiveProfile, messageType].HasKey(id))
            {
                GemClient.MessageFlow[GemNetwork.ActiveProfile, messageType, id]
                      .HandleIncomingMessage(im);
            }
        }
    }
}