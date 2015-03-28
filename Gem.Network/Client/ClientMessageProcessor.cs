﻿using Lidgren.Network;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using Gem.Network.Extensions;
using Gem.Network.Client;

namespace Gem.Network
{
    /// <summary>
    /// Processes the client's network incoming messages
    /// </summary>
    internal class ClientMessageProcessor : IMessageProcessor
    {

        #region Declarations

        /// <summary>
        /// The client
        /// </summary>
        private readonly IClient client;

        /// <summary>
        /// The information appending
        /// </summary>
        private readonly IAppender Write;

        #endregion
        
        #region Constructor

        public ClientMessageProcessor(IClient client)
        {
            this.client = client;
            Write = new ActionAppender(GemNetworkDebugger.Echo);
        }

        #endregion
        
        #region Message Processing

        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            //Holds the message receiving thread until an event fires
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
                        Write.Warn(im.ReadString());
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

    /// <summary>
    /// Helper class for handling the incoming messages
    /// </summary>
    internal static class MessageTypeExtensions
    {
        /// <summary>
        /// A messagetype extension for the GemClient. Invokes all the client related actions
        /// </summary>
        /// <param name="messageType">The incoming message's type</param>
        /// <param name="im">The incoming message</param>
        /// <param name="client">The client</param>
        internal static void Handle(this MessageType messageType, NetIncomingMessage im, IClient client)
        {
            byte id = im.ReadByte();

            if (id == GemNetwork.NotificationByte)
            {
                GemClient.ActionManager[GemNetwork.ActiveProfile].OnReceivedNotification(new Notification(im));
                return;
            }

            GemClient.ActionManager[GemNetwork.ActiveProfile, messageType].Action(client, im);

            if (GemClient.MessageFlow[GemNetwork.ActiveProfile, messageType].HasKey(id))
            {
                GemClient.MessageFlow[GemNetwork.ActiveProfile, messageType, id]
                      .HandleIncomingMessage(im);
            }
        }

        /// <summary>
        /// Decodes and handles incoming messages
        /// </summary>
        /// <param name="args">The message arguments</param>
        /// <param name="message">The net incoming message to decode and handle</param>
        internal static void HandleIncomingMessage(this MessageFlowArguments args, NetIncomingMessage message)
        {
            var readableMessage = MessageSerializer.Decode(message, args.MessagePoco);
            args.MessageHandler.Handle(readableMessage);//.ReadAllProperties());
        }

    }
}