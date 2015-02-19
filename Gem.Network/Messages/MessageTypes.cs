using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Messages
{
    public enum MessageType
    {

        Hail,
    
        ConnectionApproval,

        NewClient,

        Command,
                
        Disconnect,

        DiscoveryResponse,

        Data

    }
}

