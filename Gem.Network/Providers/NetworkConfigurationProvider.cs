using Gem.Network.Containers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;

namespace Gem.Network.Providers
{
    internal class ClientConfigurationProvider
    : AbstractContainer<MessageTypeProvider, string>
    {
        public ClientConfigurationProvider()
            : base(new FlyweightRepository<MessageTypeProvider, string>())
        { }
    }
    
    public class MessageTypeProvider
    : AbstractContainer<ClientMessageFlowInfoProvider, MessageType>
    {
        public MessageTypeProvider()
            : base(new FlyweightRepository<ClientMessageFlowInfoProvider, MessageType>())
        { }
    }


    internal class ServerConfigurationManager
    : AbstractContainer<ServerMessageFlowManager, string>
    {
        public ServerConfigurationManager()
            : base(new FlyweightRepository<ServerMessageFlowManager, string>())
        { }
    }
    
}
