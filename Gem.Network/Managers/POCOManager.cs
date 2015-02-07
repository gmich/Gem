using Gem.Network.DynamicBuilders;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Managers
{
    class PocoManager
    {

        private Dictionary<byte, Type> pocoLookup;
        
        public PocoManager()
        {
            pocoLookup = new Dictionary<byte, Type>();
        }

        public IDisposable RegisterPoco(string classname, params Type[] types)
        {
            Guard.That(types.All(x => x.IsPrimitive || x == typeof(string) || x!=typeof(byte)), 
              "All types should be primitive and not typeof byte. Bytes are reserved to be the message's unique id");

            Guard.That(pocoLookup).IsTrue(x => x.Count < (int)byte.MaxValue,
                "You have reached the maximum capacity of Pocos. Consider deregistering");

           var propInfo = DynamicPropertyInfo.GetPropertyInfo(types);

            //get random byte
            //create the class
            //register to dictionary

            throw new NotImplementedException();
           
        }
    }
}
