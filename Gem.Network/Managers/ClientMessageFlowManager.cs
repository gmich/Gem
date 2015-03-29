using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Managers
{       
    /// <summary>
    /// Manager class for accessing <see cref=">Gem.Network.Providers.ClientConfigurationProvider"/>
    /// by index [string,MessageType,byte]
    /// </summary>
    public class ClientMessageFlowManager
    {
        private readonly ClientConfigurationProvider configurationManager;

        public ClientMessageFlowManager()
        {
            configurationManager = new ClientConfigurationProvider();
        }

        #region Get by Index

        public MessageFlowArguments this[string tag,MessageType messagetype,byte configID]
        {
            get
            {
                return configurationManager[tag][messagetype][configID];
            }
        }

        public MessageFlowInfoProvider this[string tag, MessageType messagetype]
        {
            get
            {
                return configurationManager[tag][messagetype];
            }
        }

        public ClientMessageTypeProvider this[string tag]
        {
            get
            {
                return configurationManager[tag];
            }
        }

        #endregion
    }

}
