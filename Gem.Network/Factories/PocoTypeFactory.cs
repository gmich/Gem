using Gem.Network.Cache;
using Gem.Network.DynamicBuilders;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Managers
{
    public sealed class PocoTypeFactory
    {
        private static GCache<Type[], Type> typeCache;

        internal class ArrayTypeEquality : EqualityComparer<Type[]>
        {
            public override int GetHashCode(Type[] type)
            {
                return type.GetHashCode();
            }

            public override bool Equals(Type[] type1, Type[] type2)
            {
                return type1.SequenceEqual(type2);
            }
        }

        static PocoTypeFactory()
        {
            typeCache = new GCache<Type[], Type>(GC.GetTotalMemory(true) / 10, new ArrayTypeEquality());
        }

        public static Type Create(List<DynamicPropertyInfo> propertyInfo, string classname)
        {
            Guard.That(propertyInfo.All(x => x.PropertyType.IsPrimitive
                || x.PropertyType == typeof(string)
                || x.PropertyType != typeof(byte)),
              "All types should be primitive and not typeof byte. Bytes are reserved to be the message's unique id");

            Type[] typeArray = propertyInfo.Select(x => x.PropertyType).ToArray();

            Type lookupType = typeCache.Lookup(typeArray);
            if (lookupType != null)
            {
                return lookupType;
            }

            Type newType = PocoBuilder.Create(classname, propertyInfo);
            typeCache.Add(typeArray, newType);

            return newType;
        }
    }
}
