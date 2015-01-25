using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Gem.Network.Messages;

namespace Gem.Network
{
    public class ClientNetworkManager
    {
        NetClient Client;
        bool IsRunning = false;
        Dictionary<IncomingMessageTypes, Action<NetIncomingMessage>> ServerEventHandler;
        NetConnection ServerConnection;

        void Connect(string hostName, string hostIP, int port)
        {
            NetPeerConfiguration Config = new NetPeerConfiguration(hostName);

            // Create new client, with previously created configs
            Client = new NetClient(Config);

            // Create new outgoing message
            NetOutgoingMessage outmsg = Client.CreateMessage();

            Client.Start();

            outmsg.Write((byte)PacketTypes.LOGIN);
            outmsg.Write("MyName");

            Client.Connect(hostIP, port, outmsg);

            WaitForStartingInfo();
        }


        private void WaitForStartingInfo()
        {

            bool CanStart = false;

            NetIncomingMessage inc;

            while (!CanStart)
            {

                if ((inc = Client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:


                            CanStart = true;

                            break;
                        default:
                            Console.WriteLine(inc.ReadString() + " Strange message");
                            break;
                    }
                }
            }
        }
        
        private void CheckServerMessages()
        {
            NetIncomingMessage im;

            while ((im = Client.ReadMessage()) != null)
            {
                if (im.MessageType == NetIncomingMessageType.Data)
                {
                    var messageType = (IncomingMessageTypes)im.ReadByte();
                    if (ServerEventHandler.ContainsKey(messageType))
                    {
                    ServerEventHandler[messageType](im);
                    }
                    else
                    {
                        //append bad incoming message
                    }
                }
            }
        }


    }

}
