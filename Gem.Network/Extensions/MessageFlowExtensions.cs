using Gem.Network.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Extensions
{
    public static class MessageFlowArgumentsExtensions
    {

        public static void HandleIncomingMessage(this MessageFlowArguments args, NetIncomingMessage message)
        {
            var readableMessage = MessageSerializer.Decode(message, args.MessagePoco);
            args.MessageHandler.Handle(readableMessage.ReadAllProperties());
        }

        public static void SendCachedMessage(this MessageFlowArguments args)
        {
            args.EventRaisingclass.Send(args.CachedMessage);

        }
        
    }
}
