using Gem.Network.Containers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Repositories;

namespace Gem.Network.Providers
{
    /// <summary>
    /// Readonly provider for MessageFlowInfoProvider accessed by string and MessageType
    /// </summary>
    internal class ClientConfigurationProvider
    : AbstractContainer<ClientMessageTypeProvider, string>
    {
        public ClientConfigurationProvider()
            : base(new FlyweightRepository<ClientMessageTypeProvider, string>())
        { }
    }
    
    public class ClientMessageTypeProvider
    : AbstractContainer<MessageFlowInfoProvider, MessageType>
    {
        public ClientMessageTypeProvider()
            : base(new FlyweightRepository<MessageFlowInfoProvider, MessageType>())
        { }
    }


    internal class ServerConfigurationManager
    : AbstractContainer<ServerActionManager, string>
    {
        public ServerConfigurationManager()
            : base(new FlyweightRepository<ServerActionManager, string>())
        { }
    }
    
}
