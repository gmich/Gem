using Lidgren.Network;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Gem.Network
{
    public class ConnectionConfig
    {
        [Required]
        public string ServerName { get; set; }

        [Required]
        public string IPorHost { get; set; }

        [Required]
        public int Port { get; set;  }

        [Required]
        public string DisconnectMessage { get; set; }

        public IPEndPoint ServerIP
        {
            get
            {
                return new IPEndPoint(NetUtility.Resolve(IPorHost), Port);
            }
        }

    }
}
