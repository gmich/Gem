using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network
{
    public interface IClient : INetworkManager
    {
        void SendMessage<T>(T message);

        void SendMessage(NetOutgoingMessage gameMessage);
    }
}
