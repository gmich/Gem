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

        #region Declarations

        private readonly INetworkManager client;

        private readonly IPEndPoint server;

        public string Name { get; private set; }

        public bool IsRunning { get; private set; }

        private readonly int maxConnections;

        private Dictionary<IncomingMessageTypes, Action<NetIncomingMessage>> ServerEventHandler;

        #endregion


        #region Constructor

        public ClientHandler(IPEndPoint server, INetworkManager client, string name, int port, int maxConnections)
        {
            this.ServerEventHandler = new Dictionary<IncomingMessageTypes, Action<NetIncomingMessage>>();
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

        public IDisposable RegisterAction(IncomingMessageTypes type, Action<NetIncomingMessage> action)
        {
            if (ServerEventHandler.ContainsKey(type))
            {
                ServerEventHandler[type] += action;
            }
            else
            {
                ServerEventHandler.Add(type, action);
            }
            return new DeregisterDictionary<IncomingMessageTypes, Action<NetIncomingMessage>>(ServerEventHandler, action);
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
                    case NetIncomingMessageType.Data:
                        //if (im.ReadByte() == (byte)IncomingMessageTypes.NewClient)
                        //{
                        //    Console.WriteLine("Approved new client");
                        //}
                        var messageType = (IncomingMessageTypes)im.ReadByte();
                        if (ServerEventHandler.ContainsKey(messageType))
                        {
                            ServerEventHandler[messageType](im);
                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        //Append to listener
                        Console.WriteLine(im.ReadString());
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