using Gem.Network.Builders;
using System;
using System.Collections.Generic;

namespace Gem.Network.Factories
{
    public interface IPocoFactory
    {
        Type Create(List<RuntimePropertyInfo> propertyInfo, string classname);
    }
}
