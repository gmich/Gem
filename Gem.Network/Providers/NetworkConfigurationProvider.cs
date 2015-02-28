using Gem.Network.Containers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Providers
{
    internal class NetworkConfigurationProvider
    : AbstractContainer<MessageTypeProvider, string>
    {
        public NetworkConfigurationProvider()
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
}
