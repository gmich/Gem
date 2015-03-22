using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network.Extensions
{
    public static class MessageFlowArgumentsExtensions
    {

        public static void HandleIncomingMessage(this MessageFlowArguments args, NetIncomingMessage message)
        {
            var readableMessage = MessageSerializer.Decode(message, args.MessagePoco);
            args.MessageHandler.Handle(readableMessage);//.ReadAllProperties());
        }
        
    }
}
