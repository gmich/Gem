using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network
{
    public class ServerConfig
    {

        public string Name { get; set; }

        public int Port { get; set; }

        public int SequenceChannel { get; set; }

        public NetDeliveryMethod DeliveryMethod { get; set; }

    }
}
