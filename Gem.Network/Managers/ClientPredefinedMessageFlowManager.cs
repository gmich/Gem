using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Managers
{   
    
    public class ClientPredefinedMessageFlowManager
    {
        private readonly PredefinedMessageTypeProvider configurationManager;

        public ClientPredefinedMessageFlowManager()
        {
            configurationManager = new PredefinedMessageTypeProvider();
        }

        public MessageFlowArguments this[MessageType messagetype,byte configID]
        {
            get
            {
                return configurationManager[messagetype][configID];
            }
        }

        public PredefinedMessageFlow this[MessageType messagetype]
        {
            get
            {
                return configurationManager[messagetype];
            }
        }
    }

}
