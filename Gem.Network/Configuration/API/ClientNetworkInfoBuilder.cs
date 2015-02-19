using Gem.Network.Repositories;

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

        public void End()
        {
            profiles.Get(ProfileId).AddConfig(clientInfo);
        }
    }
}
