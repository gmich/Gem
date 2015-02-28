using Lidgren.Network;
using System;
using System.Net;

namespace Gem.Network
{
    public class ConnectionDetails
    {

        public string ServerName { get; set; }

        public IPEndPoint ServerIP { get; set; }

        public int SequenceChannel { get; set; }

        public NetDeliveryMethod DeliveryMethod { get; set; }

    }
}
