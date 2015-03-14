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

        public bool RequireAuthentication { get; set; }

        public float ConnectionTimeout { get; set; }

        public bool EnableUPnP { get; set; }

        public string DisconnectMessage { get; set; }

    }
}
