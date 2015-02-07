using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Cache
{
    public class TypeRepository : IDynamicTypeRepository
    {
        private Dictionary<string, Type> typeCache;

        public TypeRepository()
        {
            typeCache = new Dictionary<string, Type>();
        }

        public void RegisterType(string typeID, Type type)
        {
            Guard.That(typeCache).IsTrue(x => !x.ContainsKey(typeID), "The typeID is not found");

            typeCache.Add(typeID, type);
            //TODO: raise new type added event
        }

        public void DeregisterType(string typeID)
        {
            throw new NotImplementedException();
            //TODO: raise type removed event
        }

        public object CreateObject(string typeID)
        {
            Guard.That(typeCache).IsTrue(x => x.ContainsKey(typeID), "The typeID is not found");

            return Activator.CreateInstance(typeCache[typeID]);
        }

        public object CreateObject(string typeID, params object[] constructorParams)
        {
            Guard.That(typeCache).IsTrue(x => x.ContainsKey(typeID), "The typeID is not found");

            return Activator.CreateInstance(typeCache[typeID], constructorParams);
        }
    }
}
