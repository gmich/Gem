using Gem.Network.Containers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;

namespace Gem.Network.Protocol
{
    internal class ProtocolProvider
    : AbstractContainer<ProtocolTypeProvider, string>
    {
        public ProtocolProvider()
            : base(new FlyweightRepository<ProtocolTypeProvider, string>())
        { }
    }


    public class TypeAndAttribute
    {
        public Type Type { get; set; }
        public NetworkPackageAttribute Attribute { get; set; }
    }

    public class ProtocolTypeProvider
    : AbstractContainer<TypeAndAttribute, byte>
    {
        public ProtocolTypeProvider()
            : base(new FlyweightRepository<TypeAndAttribute, byte>())
        { }


        public void Add(byte id, TypeAndAttribute item)
        {
            this.dataRepository.Add(id, item);
        }

    }

    public class ProtocolProviderManager
    {
        private readonly ProtocolProvider protocolProvider;

        public ProtocolProviderManager()
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
