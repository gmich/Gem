using Gem.Network.Events;
using System;
using System.Reflection;

namespace Gem.Network.Factories
{
    /// <summary>
    /// A factory that's bounded to align INetworkEvents to ClientEvent<>
    /// </summary>
    public sealed class ClientEventFactory : IEventFactory
    {
        /// <summary>
        /// Creates an INetwork event that is of the generic type ClientEvent<>
        /// </summary>
        /// <param name="type">The generic type of the ClientEvent<></param>
        /// <param name="constructorArgs">The constructor args of the ClientEvent<></param>
        /// <returns>The ClientEvent<> aligned to INetworkEvent</returns>
        public INetworkEvent Create(Type type,params object[] constructorArgs)
        {
            var dynamicType = typeof(ClientEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
