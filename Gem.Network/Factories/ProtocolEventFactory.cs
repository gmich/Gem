using Gem.Network.Events;
using System;

namespace Gem.Network.Factories
{
    /// <summary>
    /// Creates an event that's used to raise events that send a message using the NetworkPackage protocol by the client
    /// This factory is bounded to ProtocolEvent<>
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
