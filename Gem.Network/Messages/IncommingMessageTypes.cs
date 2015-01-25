using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Messages
{
    /// <summary>
    /// The game message types.
    /// </summary>
    public enum IncomingMessageTypes
    {
        ConnectionApproval,
                
        Disconnect,

        Discovery,

        Data

    }
}

