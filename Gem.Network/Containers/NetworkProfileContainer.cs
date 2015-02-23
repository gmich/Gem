using Gem.Network.Repositories;
using System;

namespace Gem.Network.Containers
{
    public class NetworkProfileContainer
    {

        private readonly IDataProvider<ClientConfigurationContainer, string> profiles;

        public NetworkProfileContainer(IDataProvider<ClientConfigurationContainer, string> dataProvider)
        {
            profiles = dataProvider;
        }

        public void Add(string profileId, ClientConfigurationContainer config)
        {
            profiles.Add(profileId,config);
        }

        public void Update(string profileId, ClientConfigurationContainer config)
        {
            profiles.Update(profileId,config);
        }

        /// <summary>
        /// Returns the client configuration by tag.
        /// If none is found, a new config is created and added to the repository
        /// </summary>
        /// <param name="profileId">The profile's unique id</param>
        /// <returns></returns>
        public ClientConfigurationContainer Get(string profileId)
        {
            var profile = profiles.GetById(profileId);

            if (profile != null)
            {
                return profile;
            }
            else
            {
                var config = new ClientConfigurationContainer();
                Add(profileId, config);
                return config;
            }
        }
    }
}
