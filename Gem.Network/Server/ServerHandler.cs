﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Collections.Generic;

namespace Gem.Network.Server
{
    public class ServerHandler : IDisposable
    {
        #region Events

        public event Action<string> WriteMessage;

        #endregion


        #region Declarations

        private readonly IServer networkManager;

        public string Name { get; private set; }

        public int Port { get; private set; }
        
        public bool IsRunning { get; private set; }

        //private Dictionary<string, Registerer<NetConnection>> Groups;

        private Registerer<NetConnection> Clients;

        #endregion
        
        #region Constructor

        public ServerHandler(IServer networkManager, string name, int port,int maxConnections)
        {
            this.Name = name;
            this.Port = port;
            this.networkManager = networkManager;
            Clients = new Registerer<NetConnection>(maxConnections);

            try
            {
                networkManager.Connect(name, port);
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
            networkManager.Disconnect();
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

            while ((im = this.networkManager.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (im.ReadByte() == (byte)IncomingMessageTypes.ConnectionApproval)
                        {
                            var message = new ConnectionApprovalMessage(im);
                            WriteMessage("Incoming Connection");

                            if (Clients.Register(message.Sender,im.SenderConnection))
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
                                //this.networkManager.DeRegisterConnection(im.Name);
                            }
                                break;

                            case NetConnectionStatus.RespondedConnect:
                                //Configure approval
                                NetOutgoingMessage hailMessage = this.networkManager.CreateMessage();
                                im.SenderConnection.Approve(hailMessage);
                                break;
                        }

                        break;
                    case NetIncomingMessageType.Data:
                        if (im.ReadByte() == (byte)IncomingMessageTypes.Data)
                        {
                            //Broadcast to all except sender
                            networkManager.SendMessage(im as IServerMessage, im.SenderConnection);
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
                this.networkManager.Recycle(im);
            }
        }

        #endregion

    }
}