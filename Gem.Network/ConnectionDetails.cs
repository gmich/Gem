using Lidgren.Network;
using System;
using System.Net;

namespace Gem.Network
{
    public class ConnectionDetails
    {
       
        public string ServerName { get; set; }

        public string IPorHost { get; set; }

        public int Port { get; set;  }

        public int SequenceChannel
        {
            get
            {
                return 1;
            }
        }

        public NetDeliveryMethod DeliveryMethod
        {
            get
            {
                return NetDeliveryMethod.ReliableUnordered;
            }
        }

        public IPEndPoint ServerIP
        {
            get
            {
                return new IPEndPoint(NetUtility.Resolve(IPorHost), Port);
            }
        }

    }
}
