using Gem.Network.Events;
using System;
using System.Reflection;

namespace Gem.Network.Factories
{
    /// <summary>
    /// Creates a class of type PeerEvent that registers events to the NetworkProvider.Send method
    /// </summary>
    public sealed class TimeDeltaEventFactory : IEventFactory
    {
        public INetworkEvent Create(Type type,params object[] constructorArgs)
        {
            var dynamicType = typeof(RemoteTimeEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
