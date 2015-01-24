using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Gem.Network
{
    public class ClientNetworkManager
    {
        // Client Object
        NetClient Client;
        bool IsRunning = false;

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
            NetIncomingMessage inc;

            while ((inc = Client.ReadMessage()) != null)
            {
                if (inc.MessageType == NetIncomingMessageType.Data)
                {

                }
            }
        }



        private void GetInputAndSendItToServer()
        {
            
            if (false)
            {

                Client.Disconnect("bye bye");
            }

            // Create new message
            NetOutgoingMessage outmsg = Client.CreateMessage();

            // Write byte = move direction
            outmsg.Write("something");

            // Send it to server
            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);


        }

    }

}
