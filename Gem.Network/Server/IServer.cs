using Gem.Network.Messages;
using Lidgren.Network;
using System.Collections.Generic;

namespace Gem.Network
{
    public interface IServer : INetworkManager
    {
        void SendMessage(IServerMessage gameMessage, NetConnection sender);

        void SendMessage(IServerMessage gameMessage, List<NetConnection> clients);
    }
}
