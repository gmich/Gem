using Gem.Network.Messages;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Containers
{

    internal class NetworkConfigurationManager
    : AbstractContainer<MessageTypeManager, string>
    {
        public NetworkConfigurationManager()
        :base(new FlyweightRepository<MessageTypeManager, string>())
        { }
    }

    internal class MessageTypeManager
    : AbstractContainer<ClientNetworkInfoManager, MessageType>
    {
                public MessageTypeManager()
            : base(new FlyweightRepository<ClientNetworkInfoManager, MessageType>())
        { }
    }

    internal class ClientNetworkInfoManager
           : AbstractContainer<ClientNetworkInfo, byte>
    {
        public ClientNetworkInfoManager()
            : base(new FlyweightRepository<ClientNetworkInfo, byte>())
        { }
    }
    
    public class NetworkConfigDirector
    {
        private readonly NetworkConfigurationManager configurationManager;

        public NetworkConfigDirector()
        {
            configurationManager = new NetworkConfigurationManager();
        }

        private object InvokationTest()
        {
            return this["tag", MessageType.Data, (byte)10];
        }

        //Readonly
        public ClientNetworkInfo this[string tag,MessageType messagetype,byte configID]
        {
            get
            {
                return configurationManager[tag][messagetype][configID];
            }
        }
    }
}
