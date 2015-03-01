using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Server
{
    public class ServerConfig
    {

        public string Name { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public int MaxConnections { get; set; }

        public int SequenceChannel { get { return 1; } }

        public NetDeliveryMethod DeliveryMethod { get { return NetDeliveryMethod.ReliableUnordered; } }

    }
}
