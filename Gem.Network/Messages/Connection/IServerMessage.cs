using Lidgren.Network;

namespace Gem.Network.Messages
{

    public interface IServerMessage
    {
        ClientMessageType MessageType { get; }
    }

}