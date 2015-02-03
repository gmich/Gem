using Gem.Network.Messages;
using Lidgren.Network;
using System;


namespace Gem.Network.Messages
{
    /// <summary>
    /// TODO: make static
    /// </summary>
    public class MessageSerializer
    {

        public static T Decode<T>(NetIncomingMessage im)
            where T: new()
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            im.ReadAllProperties(obj);

            return obj;
        }

        public static object Decode(NetIncomingMessage im,Type messageType)
        {
            var obj = Activator.CreateInstance(messageType);
            im.ReadAllProperties(obj);

            return obj;
        }
        
        public static void Encode<T>(T objectToEncode,ref NetOutgoingMessage om)
        {
            om.WriteAllProperties(objectToEncode);
        }

    }
}
