using Gem.Network.Protocol;

namespace Gem.Network.Managers
{
    /// <summary>
    /// Manager class for accessing <see cref=">Gem.Network.Providers.ProtocolProvider"/>
    /// by index [string,byte]
    /// </summary>
    public class ProtocolManager
    {
        private readonly ProtocolProvider protocolProvider;

        public ProtocolManager()
        {
            protocolProvider = new ProtocolProvider();
        }

        public TypeAndAttribute this[string tag, byte id]
        {
            get
            {
                return protocolProvider[tag][id];
            }
        }

        public ProtocolTypeProvider this[string tag]
        {
            get
            {
                return protocolProvider[tag];
            }
        }
    }
}
