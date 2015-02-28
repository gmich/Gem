using Gem.Network.Factories;
using Gem.Network.Repositories;
using Autofac;
using Gem.Network.Events;
using Gem.Network.Containers;

namespace Gem.Network.Configuration
{

    public class ClientNetworkInfoBuilder
    {
        private readonly NetworkProfileContainer profiles;

        public MessageFlowArguments clientInfo { get; set; }
        public string ProfileId { get; set; }

        public ClientNetworkInfoBuilder(NetworkProfileContainer profiles)
        {
            clientInfo = new MessageFlowArguments();
            this.profiles = profiles;
        }

        public INetworkEvent End()
        {
            var configDisposable = profiles.Get(ProfileId).AddConfig(clientInfo);
            clientInfo.EventRaisingclass = Dependencies.Container.Resolve<IEventFactory>().Create(clientInfo.MessagePoco, configDisposable);

            return clientInfo.EventRaisingclass;
        }
    }
}
