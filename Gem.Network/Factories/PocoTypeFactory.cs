using Gem.Network.Builders;
using Gem.Network.Cache;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Factories
{
    public sealed class PocoTypeFactory : IPocoFactory
    {

        #region Private Properties

        private readonly IPocoBuilder pocoBuilder;
        private readonly GCache<Type[], Type> cache;
           
        #endregion

        #region Constructor

        public PocoTypeFactory(IPocoBuilder pocoBuilder)
        {
            this.pocoBuilder = pocoBuilder;   
            cache = new GCache<Type[], Type>(GC.GetTotalMemory(true) / 10, new ArrayTypeEquality());
        }

        #endregion

        #region IPocoFactory Implementation

        public Type Create(List<RuntimePropertyInfo> propertyInfo, string classname)
        {
            Guard.That(propertyInfo.All(x => x.PropertyType.IsPrimitive
                || x.PropertyType == typeof(string)
                || x.PropertyType != typeof(byte)),
              "All types should be primitive and not type of byte. Bytes are reserved to be the message's unique id");

            Type[] typeArray = propertyInfo.Select(x => x.PropertyType).ToArray();

            Type lookupType = cache.Lookup(typeArray);
            if (lookupType != null)
            {
                return lookupType;
            }

            Type newType = pocoBuilder.Build(classname, propertyInfo);
            cache.Add(typeArray, newType);

            return newType;
        }

        #endregion

        #region Equality Comparer for Type[]

        internal class ArrayTypeEquality : EqualityComparer<Type[]>
        {
            public override int GetHashCode(Type[] types)
            {
                int hash = 17;
                foreach (var type in types)
                {
                    hash = hash * 31 + type.GetHashCode();
                }
                return hash;
            }

            public override bool Equals(Type[] type1, Type[] type2)
            {
                return type1.SequenceEqual(type2);
            }
        }

        #endregion


    }
}
