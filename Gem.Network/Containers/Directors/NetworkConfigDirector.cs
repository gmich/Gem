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

    public class MessageTypeManager
    : AbstractContainer<ClientNetworkInfoManager, MessageType>
    {
                public MessageTypeManager()
            : base(new FlyweightRepository<ClientNetworkInfoManager, MessageType>())
        { }
    }

    public class ClientNetworkInfoManager
           : AbstractContainer<ClientNetworkInfo, byte>
    {
        public ClientNetworkInfoManager()
            : base(new FlyweightRepository<ClientNetworkInfo, byte>())
        { }
    }
    
    public class NetworkDirector
    {
        private readonly NetworkConfigurationManager configurationManager;

        public NetworkDirector()
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

        //Readonly
        public ClientNetworkInfoManager this[string tag, MessageType messagetype]
        {
            get
            {
                return configurationManager[tag][messagetype];
            }
        }

        //Readonly
        public MessageTypeManager this[string tag]
        {
            get
            {
                return configurationManager[tag];
            }
        }
    }
}
