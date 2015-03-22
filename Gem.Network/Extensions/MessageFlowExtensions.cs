using Gem.Network.Messages;
using Gem.Network.Server;
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
        public static void HandleIncomingMessage(this MessageArguments args, NetIncomingMessage message)
        {
            var readableMessage = MessageSerializer.Decode(message, args.MessagePoco);
            args.MessageHandler.Handle(readableMessage);//.ReadAllProperties());
        }    
    }
}
