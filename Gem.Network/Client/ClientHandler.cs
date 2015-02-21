﻿using System;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Net;
using System.Collections.Generic;
using Gem.Network.Utilities;

namespace Gem.Network
{
    public class ClientHandler : IDisposable
    {

        #region Message Listeners
        
        public IDisposable AddListener(Action<string> listener)
        {
            this.WriteMessage += listener;
            throw new NotImplementedException();
        }
        
        private event Action<string> WriteMessage;

        #endregion


        #region Configurations

        //INetworkConfiguration<IServerMessage> connectionApproval;

        #endregion


        #region Declarations

        private readonly INetworkManager client;

        private readonly IPEndPoint server;

        public string Name { get; private set; }

        public bool IsRunning { get; private set; }

        private readonly int maxConnections;

        private Dictionary<MessageType, Action<NetIncomingMessage>> ServerEventHandler;

        public Action<NetIncomingMessage> OnConnected;

        #endregion


        #region Constructor

        public ClientHandler(IPEndPoint server, INetworkManager client, string name, int port, int maxConnections)
        {
            this.ServerEventHandler = new Dictionary<MessageType, Action<NetIncomingMessage>>();
            this.maxConnections = maxConnections;
            this.Name = name;
            this.client = client;
            try
            {
                client.Connect(name, port);
                IsRunning = true;
            }
            catch (Exception ex)
            {
                //TODO: log this
                IsRunning = false;
            }
        }

        #endregion


        #region Register Actions

        public IDisposable RegisterAction(MessageType type, Action<NetIncomingMessage> action)
        {
            if (ServerEventHandler.ContainsKey(type))
            {
                ServerEventHandler[type] += action;
            }
            else
            {
                ServerEventHandler.Add(type, action);
            }
            return new DeregisterDictionary<MessageType, Action<NetIncomingMessage>>(ServerEventHandler, action);
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
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        //handle 
                        WriteMessage("Approved new client");
                        break;
                    case NetIncomingMessageType.Data:
                        var messageType = (MessageType)im.ReadByte();
                        if (ServerEventHandler.ContainsKey(messageType))
                        {
                            ServerEventHandler[messageType](im);
                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        WriteMessage(im.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        //show the response 
                        break;
                }
                this.client.Recycle(im);
            }
        }

        #endregion


    }
}