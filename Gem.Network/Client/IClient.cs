using Gem.Network.Messages;

namespace Gem.Network.Client
{
    interface IClient : INetworkManager
    {
        void SendMessage(IServerMessage gameMessage);
    }
}
