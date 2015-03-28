using Gem.Network.Events;
using System;
using System.Reflection;

namespace Gem.Network.Factories
{
    /// <summary>
    /// A factory that's bounded to align INetworkEvents to RemoteTimeEvent<>
    /// </summary>
    public sealed class TimeDeltaEventFactory : IEventFactory
    {
        /// <summary>
        /// Creates an INetwork event that is of the generic type RemoteTimeEvent<>
        /// </summary>
        /// <param name="type">The generic type of the RemoteTimeEvent<></param>
        /// <param name="constructorArgs">The constructor args of the RemoteTimeEvent<></param>
        /// <returns>The RemoteTimeEvent<> aligned to INetworkEvent</returns>
        public INetworkEvent Create(Type type,params object[] constructorArgs)
        {
            var dynamicType = typeof(RemoteTimeEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<INetworkEvent>();
        }

    }
}
