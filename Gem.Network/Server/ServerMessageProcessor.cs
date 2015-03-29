﻿using Lidgren.Network;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;

namespace Gem.Network.Server
{
    /// <summary>
    /// Processes the server incoming messages.
    /// </summary>
    public class ServerMessageProcessor : IMessageProcessor
    {

        #region Declarations

        private readonly IServer server;

        #endregion

        #region Constructor

        public ServerMessageProcessor(IServer server)
        {
            this.server = server;
        }

        #endregion

        #region Messages

        /// <summary>
        /// Processes the server incoming messages by using the GemServer's active configuration
        /// </summary>
        public void ProcessNetworkMessages()
        {
            NetIncomingMessage im;
            server.Wait();

            while ((im = this.server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        var approvalMsg = new ConnectionApprovalMessage(im);
                        GemNetworkDebugger.Append.Info("Incoming Connection");
                        GemServer.ServerConfiguration[GemNetwork.ActiveProfile].OnIncomingConnection(server, im.SenderConnection, approvalMsg);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                //im.SenderConnection.Approve();
                                GemNetworkDebugger.Append.Info("{0} Connected", im.SenderConnection);
                                break;
                            case NetConnectionStatus.Disconnected:
                                GemNetworkDebugger.Append.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                GemServer.ServerConfiguration[GemNetwork.ActiveProfile].OnClientDisconnect(server, im.SenderConnection, im.ReadString());
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                GemNetworkDebugger.Append.Info(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        byte id = im.ReadByte();

                        if (id == GemNetwork.NotificationByte)
                        {
                            GemServer.ServerConfiguration[GemNetwork.ActiveProfile].HandleNotifications(server, im.SenderConnection, new Notification(im));
                        }
                        else if (GemServer.MessageFlow[GemNetwork.ActiveProfile, MessageType.Data].HasKey(id))
                        {
                            GemServer.MessageFlow[GemNetwork.ActiveProfile, MessageType.Data, id]
                                     .HandleIncomingMessage(im);
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
                        GemNetworkDebugger.Append.Warn(im.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        GemNetworkDebugger.Append.Error(im.ReadString());
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
    /// <summary>
    /// Helper class for handling MessageArguments
    /// </summary>
    internal static class MessageArgumentsExtensions
    {
        /// <summary>
        /// Decodes and handles incoming messages
        /// </summary>
        /// <param name="args">The message arguments</param>
        /// <param name="message">The net incoming message to decode and handle</param>
        internal static void HandleIncomingMessage(this MessageArguments args, NetIncomingMessage message)
        {
            var readableMessage = MessageSerializer.Decode(message, args.MessagePoco);
            args.MessageHandler.Handle(message.SenderConnection,readableMessage);
        }
    }

}