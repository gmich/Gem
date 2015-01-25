using Lidgren.Network;
using System;

namespace Gem.Network
{
    public class ConnectionDetails
    {

        public string ServerName { get; set; }

        public int Port { get; set; }

        public int SequenceChannel { get; set; }

        public NetDeliveryMethod DeliveryMethod { get; set; }

        public string DisconnectMessage { get; set; }
        
    }
}
