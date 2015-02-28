﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Collections.Generic;
using Gem.Network.Configuration;
using Gem.Network.Containers;

namespace Gem.Network
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
                        if (im.ReadByte() == (byte)MessageType.ConnectionApproval)
                        {
                            var message = new ConnectionApproval(im);
                            WriteMessage("Incoming Connection");
                            //TODO: change
                            if(server.ClientsCount<5)
                            {
                                im.SenderConnection.Approve();     
                                //Send a message to notify the others
                                //networkManager.SendMessage(IGameMessage, im.SenderConnection);
                                WriteMessage("Approved new connection " + im.SenderConnection);
                            }
                            else
                            {
                                im.SenderConnection.Deny();
                                WriteMessage("Denied new connection" + im.SenderConnection);
                            }                   
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
                            {
                                //Deregister peer
                                //Clients.Deregister(im.Name);
                                //this.networkManager.DeRegisterConnection(im.Name);
                            }
                                break;

                            case NetConnectionStatus.RespondedConnect:
                                //Configure approval
                                NetOutgoingMessage hailMessage = this.server.CreateMessage();
                                im.SenderConnection.Approve(hailMessage);
                                break;
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        if (im.ReadByte() == (byte)MessageType.Data)
                        {
                            //Broadcast to all except sender
                           // networkManager.SendMessage(im as IServerMessage, im.SenderConnection);
                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        //Append to listener
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