using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Network.Extensions;

namespace Gem.Network.Messages
{
    public enum MessageType
    {
        ServerNotification,

        Handshake,

        Connecting,

        Connected,

        Disconnecting,

        Disconnected,

        DiscoveryRequest,

        DiscoveryResponse,

        Data,

        Warning,

        Error
    }
    
}


