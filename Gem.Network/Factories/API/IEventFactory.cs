using Gem.Network.Builders;
using Gem.Network.Events;
using System;

namespace Gem.Network.Factories
{
    public interface IEventFactory
    {
        INetworkEvent Create(Type type,params object[] constructorArgs);
    }
}
