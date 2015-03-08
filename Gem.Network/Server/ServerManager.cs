﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Collections.Generic;
using Gem.Network.Configuration;
using Gem.Network.Containers;

namespace Gem.Network.Server
{

    public class ServerManager : IDisposable
    {

        #region Events

        public Action<string> WriteMessage;

        private readonly IServer server;

        public bool IsRunning { get; private set; }

        #endregion
        

        #region Constructor

        public ServerManager(IServer server, ServerConfig serverConfig)
        {
            this.server = server;
 
            try
            {
                server.Connect(serverConfig);
                IsRunning = true;
                WriteMessage("Server session started");
            }
            catch (Exception ex)
            {
                WriteMessage("Server failed to connect : " + ex);
                IsRunning = false;
            }
        }

        #endregion


        #region Close Connection

        public void Disconnect()
        {
            server.Disconnect();
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

            while ((im = this.server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (im.ReadByte() == (byte)MessageType.Handshake)
                        {
                            WriteMessage("Incoming Connection");
                            var message = MessageSerializer.Decode<ConnectionApprovalMessage>(im);
                            GemNetwork.ServerConfiguration[GemNetwork.ActiveProfile].OnIncomingConnection(server, im.SenderConnection, message);
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                WriteMessage("Connected to {0}");
                                break;
                            case NetConnectionStatus.Disconnected:
                                WriteMessage(im.SenderConnection + " status changed. " + (NetConnectionStatus)im.SenderConnection.Status);
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
                        server.SendAndExclude(msg, im.SenderConnection);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        WriteMessage(im.ReadString());
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