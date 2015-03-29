using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Server;

namespace Gem.Network.Managers
{
    /// <summary>
    /// Manager class for accessing <see cref=">Gem.Network.Providers.ServerConfigurationProvider"/>
    /// by index [string,MessageType,byte]
    /// </summary>
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

        public MessageArgumentProvider this[string tag, MessageType messagetype]
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
