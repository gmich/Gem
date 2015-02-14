using Gem.Network.Builders;
using System;

namespace Gem.Network.Factories
{
    public interface IEventFactory
    {
        object Create(Type type);
    }
}
