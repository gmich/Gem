using Gem.Network.Events;
using System;
using System.Reflection;

namespace Gem.Network.Factories
{
    /// <summary>
    /// A generic factory that creates generic types and aligns them to INetworkEvent
    /// </summary>
    public sealed class EventFactory
    {
        /// <summary>
        /// Creates an INetworkEvent that is of the generic type specified
        /// </summary>
        /// <param name="type">The generic type of the TEvent<></param>
        /// <param name="constructorArgs">The constructor args of the TEvent</param>
        /// <returns>The TEvent aligned to IEventFactory</returns>
        public static INetworkEvent Create<TEvent>(Type type,params object[] constructorArgs)
                where TEvent : INetworkEvent
        {
            var dynamicType = typeof(TEvent);
            return Activator.CreateInstance(dynamicType.MakeGenericType(typeof(TEvent)), constructorArgs)
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
