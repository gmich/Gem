using Gem.Network.Containers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;

namespace Gem.Network.Providers
{
    internal class ServerConfigurationProvider
    : AbstractContainer<ServerMessageTypeProvider, string>
    {
        public ServerConfigurationProvider()
            : base(new FlyweightRepository<ServerMessageTypeProvider, string>())
        { }
    }

    public class ServerMessageTypeProvider
    : AbstractContainer<InfoProvider, MessageType>
    {
        public ServerMessageTypeProvider()
            : base(new FlyweightRepository<InfoProvider, MessageType>())
        { }
    }
    
}
