using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Managers
{   
    
    public class MessageFlowManager
    {
        private ClientConfigurationProvider configurationManager;

        public MessageFlowManager()
        {
            configurationManager = new ClientConfigurationProvider();
        }

        public MessageFlowArguments this[string tag,MessageType messagetype,byte configID]
        {
            get
            {
                return configurationManager[tag][messagetype][configID];
            }
        }

        public ClientMessageFlowInfoProvider this[string tag, MessageType messagetype]
        {
            get
            {
                return configurationManager[tag][messagetype];
            }
        }

        public MessageTypeProvider this[string tag]
        {
            get
            {
                return configurationManager[tag];
            }
        }
    }

}
