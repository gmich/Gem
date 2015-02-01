using Gem.Network.Networking;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public class EventBuilder
    {
        public static object BuildEventRaisingClass(Type type)
        {
            var dynamicType = typeof(PeerEvent<>);
            var dynamicGeneric = dynamicType.MakeGenericType(type);

            return Activator.CreateInstance(dynamicGeneric);

            //MethodInfo method = typeof(PeerEvent<dynamic>).GetMethod("OnEvent", BindingFlags.Public);                       
            //return method.MakeGenericMethod(type);
            //method.Invoke(null, arguments);
        }
    }
}
