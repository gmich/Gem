using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using Gem.Network.Server;
using System;

namespace Gem.Network.Managers
{

    public class ServerMessageFlowManager
    {
        private ServerConfigurationProvider configurationManager;

        public ServerMessageFlowManager()
        {
            configurationManager = new ServerConfigurationProvider();
        }

        public MessageArguments this[string tag, MessageType messagetype, byte configID]
        {
            get
            {
                return configurationManager[tag][messagetype][configID];
            }
        }

        public InfoProvider this[string tag, MessageType messagetype]
        {
            get
            {
                return configurationManager[tag][messagetype];
            }
        }

        public ServerMessageTypeProvider this[string tag]
        {
            get
            {
                return configurationManager[tag];
            }
        }
    }



}
