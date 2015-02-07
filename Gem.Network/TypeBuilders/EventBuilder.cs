using Gem.Network.Networking;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    /// <summary>
    /// Creates a class of type PeerEvent that registers events to the NetworkProvider.Send method
    /// </summary>
    public class EventBuilder
    {
        public static Type Create(Type type)
        {
            var dynamicType = typeof(ClientEvent<>);

            return dynamicType.MakeGenericType(type);
        }
    }
}
