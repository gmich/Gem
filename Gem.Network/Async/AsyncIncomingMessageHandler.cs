using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gem.Network.Async
{
    using Extensions;
    
    /// <summary>
    /// Handles incoming messages async
    /// </summary>
    internal class AsyncIncomingMessageHandler
    {
        /// <summary>
        /// The Task
        /// </summary>
        private readonly ActionBlock<NetworkIncomingPackage> actionBlock;

        internal class NetworkIncomingPackage
        {
            /// <summary>
            /// The network package
            /// </summary>
            public NetIncomingMessage Message { get; set; }
            /// <summary>
            /// The arguments to handle the message with
            /// </summary>
            public MessageFlowArguments Arguments { get; set; }
        }

        internal AsyncIncomingMessageHandler()
        {
            //Initialize an actionblock that decodes a message and handles it
            actionBlock = new ActionBlock<NetworkIncomingPackage>(package =>
                {
                    var readableMessage = MessageSerializer.Decode(package.Message, package.Arguments.MessagePoco);
                    package.Arguments.MessageHandler.Handle(readableMessage.ReadAllProperties());
                });
      
        }

        /// <summary>
        /// Post a message and the handling delegate to the asynchronous task
        /// </summary>
        /// <param name="message">The incoming message</param>
        /// <param name="arguments">The delegate to handle the message</param>
        internal void Post(NetIncomingMessage message, MessageFlowArguments arguments)
        {
            actionBlock.Post(new NetworkIncomingPackage
            {
                Message = message,
                Arguments = arguments
            });
        }
    }

}
