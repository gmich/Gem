using Gem.Network.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public class NetworkProfileRepository
    {
        private readonly Dictionary<string, ClientConfig> profiles;

        public NetworkProfileRepository()
        {
            profiles = new Dictionary<string, ClientConfig>();
        }

        public void Add(string profileName, ClientConfig config)
        {            
            profiles.Add(profileName, config);
        }

        public ClientConfig Get(string profileName)
        {
            if (profiles.ContainsKey(profileName))
            {
                return profiles[profileName];
            }
            return null;
        }
    }
}
