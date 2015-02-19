using Gem.Network.Repositories;
using System;

namespace Gem.Network.Configuration
{
    public class NetworkProfileRepository
    {
        internal class ClientConfiguration
        {
            public string Tag { get; set; }
            public ClientConfig Config { get; set; }
        }

        private readonly GenericRepository<ClientConfiguration, string> profiles;

        public NetworkProfileRepository()
        {
            profiles = new GenericRepository<ClientConfiguration, string>(x => x.Tag);
        }

        public void Add(string profileId, ClientConfig config)
        {
            profiles.Add(new ClientConfiguration { Config = config, Tag = profileId });
        }

        public void Update(string profileId, ClientConfig config)
        {
            profiles.Update(new ClientConfiguration { Config = config, Tag = profileId });
        }

        /// <summary>
        /// Returns the client configuration by tag.
        /// If none is found, a new config is created and added to the repository
        /// </summary>
        /// <param name="profileId">The profile's unique id</param>
        /// <returns></returns>
        public ClientConfig Get(string profileId)
        {
            var profile = profiles.GetById(profileId);

            if (profile != null)
            {
                return profile.Config;
            }
            else
            {
                var config = new ClientConfig();
                Add(profileId, config);
                return config;
            }
        }
    }
}
