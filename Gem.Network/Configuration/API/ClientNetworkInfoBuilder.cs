using Gem.Network.Factories;
using Gem.Network.Repositories;
using Autofac;
using Gem.Network.Events;

namespace Gem.Network.Configuration
{

    public class ClientNetworkInfoBuilder
    {
        private readonly NetworkProfileRepository profiles;

        public ClientNetworkInfo clientInfo { get; set; }
        public string ProfileId { get; set; }

        public ClientNetworkInfoBuilder(NetworkProfileRepository profiles)
        {
            clientInfo = new ClientNetworkInfo();
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
