using Gem.Network.ClientEvents;
using System;
using System.Reflection;

namespace Gem.Network.Factories
{
    /// <summary>
    /// Creates a class of type PeerEvent that registers events to the NetworkProvider.Send method
    /// </summary>
    public sealed class ClientEventFactory : IEventFactory
    {
        public INetworkEvent Create(Type type)
        {
            var dynamicType = typeof(ClientEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type))
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
