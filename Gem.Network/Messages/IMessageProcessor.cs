using Lidgren.Network;

namespace Gem.Network.Messages
{
    interface IMessageProcessor
    {
        void ProcessMessage(NetIncomingMessage message);
    }
}
