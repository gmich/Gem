using Lidgren.Network;

namespace Gem.Network.Messages
{

    public interface IServerMessage
    {
        MessageType MessageType { get; }
    }

}