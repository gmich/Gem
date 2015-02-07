using Gem.Network.DynamicBuilders;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Managers
{
    public sealed class PocoTypeFactory
    {
        
        public static Type Create(List<DynamicPropertyInfo> propertyInfo, string classname)
        {
            Guard.That(propertyInfo.All(x => x.PropertyType.IsPrimitive
                || x.PropertyType == typeof(string)
                || x.PropertyType != typeof(byte)),
              "All types should be primitive and not typeof byte. Bytes are reserved to be the message's unique id");

            //TODO: use the ICache
           // typeCache.Add(propertyInfo.Select(x => x.PropertyType);
            return PocoBuilder.Create(classname, propertyInfo);
        }
    }
}
