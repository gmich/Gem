using Gem.Network.Builders;
using Gem.Network.ClientEvents;
using System;

namespace Gem.Network.Factories
{
    public interface IEventFactory
    {
        INetworkEvent Create(Type type);
    }
}
