using Gem.Network.Networking;
using System;
using System.Reflection;

namespace Gem.Network.Configuration
{
    /// <summary>
    /// Creates a class of type PeerEvent that registers events to the NetworkProvider.Send method
    /// </summary>
    public sealed class EventFactory
    {
        public static object Create(Type type)
        {
            var dynamicType = typeof(ClientEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type));
        }
    }
}
