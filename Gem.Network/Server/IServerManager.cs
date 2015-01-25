using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Server
{
    public interface IServerManager : INetworkManager
    {
        bool SendToAll { set; }
    }
}
