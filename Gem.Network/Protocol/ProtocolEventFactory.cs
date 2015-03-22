using Gem.Network.Events;
using Gem.Network.Factories;
using System;
using System.Reflection;

namespace Gem.Network.Protocol
{
    /// <summary>
    /// Creates a class of type PeerEvent that registers events to the NetworkProvider.Send method
    /// </summary>
    public sealed class ProtocolEventFactory : IEventFactory
    {
        public INetworkEvent Create(Type type,params object[] constructorArgs)
        {
            var dynamicType = typeof(ProtocolEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
