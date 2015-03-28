using Gem.Network.Builders;
using Gem.Network.Events;
using System;

namespace Gem.Network.Factories
{
    /// <summary>
    /// Factory for network events
    /// </summary>
    public interface IEventFactory
    {
        /// <summary>
        /// Creates network events by the provided type.
        /// If the type doesn't have a default constructor, provide it's parameters
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="constructorArgs">The constructor parametters</param>
        /// <returns>An instance of INetworkEvent</returns>
        INetworkEvent Create(Type type,params object[] constructorArgs);
    }
}
