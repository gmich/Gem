using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gem.Network.Async
{
    using Extensions;
    
    public class AsyncIncomingMessageHandler
    {
        private readonly ActionBlock<NetworkIncomingPackage> actionBlock;

        internal class NetworkIncomingPackage
        {
            public NetIncomingMessage Message { get; set; }
            public MessageFlowArguments Arguments { get; set; }
        }
        public AsyncIncomingMessageHandler()
        {
            actionBlock = new ActionBlock<NetworkIncomingPackage>(package =>
                {
                    var readableMessage = MessageSerializer.Decode(package.Message, package.Arguments.MessagePoco);
                    package.Arguments.MessageHandler.Handle(readableMessage.ReadAllProperties());
                });
      
        }

        public void Post(NetIncomingMessage message, MessageFlowArguments arguments)
        {
            actionBlock.Post(new NetworkIncomingPackage
            {
                Message = message,
                Arguments = arguments
            });
        }
    }

}
