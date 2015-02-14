using Gem.Network.Builders;
using System;
using System.Collections.Generic;

namespace Gem.Network.Factories
{
    public interface IPocoFactory
    {
        Type Create(List<DynamicPropertyInfo> propertyInfo, string classname);
    }
}
