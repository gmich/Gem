using Gem.Network.Containers;
namespace Gem.Network.Configuration
{
    public class NetworkProfile
    {
        private readonly NetworkProfileContainer profileRepository;

        public NetworkProfile(IDataProvider<ClientConfigurationContainer,string> dataprovider)
        {
            profileRepository = new NetworkProfileContainer(dataprovider);
        }

        public MessageRouter this[string index]
        {
            get
            {
                profileRepository.Add(index, null);
                return new MessageRouter(new ClientNetworkInfoBuilder(profileRepository));
            }
        }
    }        
}




