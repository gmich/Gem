using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network
{
    public interface IClient : INetworkManager
    {
        void SendMessage(IServerMessage gameMessage);

        void SendMessage(NetOutgoingMessage gameMessage);
    }
}
