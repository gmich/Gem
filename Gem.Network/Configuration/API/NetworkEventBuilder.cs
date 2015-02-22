namespace Gem.Network.Configuration
{
    public class NetworkProfile
    {
        private readonly NetworkProfileRepository profileRepository;

        public NetworkProfile()
        {
            profileRepository = new NetworkProfileRepository();
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




