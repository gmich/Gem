using Gem.Network.Events;
using System;
using Gem.Network.Server;

namespace Gem.Network.Protocol
{
    /// <summary>
    /// Creates an event that's used to raise events that send a message using  the NetworkPackage protocol by the server.
    /// This factory is bounded to ProtocolServerEvent<>
    /// </summary>
    public sealed class ServerProtocolEventFactory
    {
        /// <summary>
        /// Creates an ProtocolServerEvent<> and aligns it to IProtocolServerEvent 
        /// </summary>
        /// <param name="type">The type that's passed to ProtocolServerEvent's generic </param>
        /// <param name="constructorArgs">The ProtocolServerEvent<> constructor arguments </param>
        /// <returns>An instance of IProtocolServerEvent</returns>
        public IProtocolServerEvent Create(Type type, params object[] constructorArgs)
        {
            var dynamicType = typeof(ProtocolServerEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<IProtocolServerEvent>();
        }

    }
}
