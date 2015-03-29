using Gem.Network.Containers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Repositories;

namespace Gem.Network.Providers
{
    /// <summary>
    /// Readonly provider for MessageArgumentProvider that are accessed by index[string,MessageType]
    /// </summary>
    internal class ServerConfigurationProvider
    : AbstractContainer<ServerMessageTypeProvider, string>
    {
        public ServerConfigurationProvider()
            : base(new FlyweightRepository<ServerMessageTypeProvider, string>())
        { }
    }

    public class ServerMessageTypeProvider
    : AbstractContainer<MessageArgumentProvider, MessageType>
    {
        public ServerMessageTypeProvider()
            : base(new FlyweightRepository<MessageArgumentProvider, MessageType>())
        { }
    }
    
}
