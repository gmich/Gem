using Gem.Network.Containers;
using Gem.Network.Repositories;
using System;

namespace Gem.Network.Protocol
{
    /// <summary>
    /// Stores object of <see cref=">TypeAndAttribute"/> using a profile tag
    /// </summary>
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
}
