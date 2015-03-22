using Gem.Network.Events;
using Gem.Network.Factories;
using System;
using System.Reflection;
using Gem.Network.Server;

namespace Gem.Network.Protocol
{

    public sealed class ServerProtocolEventFactory
    {
        public IProtocolServerEvent Create(Type type, params object[] constructorArgs)
        {
            var dynamicType = typeof(ProtocolServerEvent<>);
            return Activator.CreateInstance(dynamicType.MakeGenericType(type), constructorArgs)
                            .AlignToInterface<IProtocolServerEvent>();
        }

    }
}
