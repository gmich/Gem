using Gem.Network.Events;
using System;
using System.Reflection;

namespace Gem.Network.Factories
{
    /// <summary>
    /// Creates a class of type PeerEvent that registers events to the NetworkProvider.Send method
    /// </summary>
    public sealed class EventFactory
    {
        public static INetworkEvent Create<T>(Type type,params object[] constructorArgs)
        {
            var dynamicType = typeof(T);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
