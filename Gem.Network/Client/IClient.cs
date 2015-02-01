using Gem.Network.Messages;

namespace Gem.Network
{
    interface IClient : INetworkManager
    {
        void SendMessage(IServerMessage gameMessage);
    }
}
