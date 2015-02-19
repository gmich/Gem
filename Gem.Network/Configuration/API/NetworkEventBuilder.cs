namespace Gem.Network.Configuration
{
    public class NetworkEventBuilder
    {
        private NetworkProfileRepository profileRepository;

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




