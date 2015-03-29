using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;

namespace Gem.Network.Managers
{
    /// <summary>
    /// Manager class for accessing <see cref=">Gem.Network.Providers.NetworkActionProvider"/>
    /// by index [string,MessageType]
    /// </summary>
    public class ClientNetworkActionManager
    {
        private readonly NetworkActionProvider actionProvider;

        public ClientNetworkActionManager()
        {
            actionProvider = new NetworkActionProvider();
        }

        #region Get by Index

        public ActionProviderArguments this[string tag, MessageType messagetype]
        {
            get
            {
                return actionProvider[tag][messagetype];
            }
        }

        public ActionMessageTypeProvider this[string tag]
        {
            get
            {
                return actionProvider[tag];
            }
        }

        #endregion

    }

}
