using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Managers
{   
    
    public class PredefinedMessageFlowManager
    {
        private readonly PredefinedMessageTypeProvider configurationManager;

        public PredefinedMessageFlowManager()
        {
            configurationManager = new PredefinedMessageTypeProvider();
        }

        public MessageFlowArguments this[ClientMessageType messagetype,byte configID]
        {
            get
            {
                return configurationManager[messagetype][configID];
            }
        }

        public PredefinedMessageFlow this[ClientMessageType messagetype]
        {
            get
            {
                return configurationManager[messagetype];
            }
        }
    }

}
