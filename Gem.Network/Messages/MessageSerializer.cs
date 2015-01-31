using Gem.Network.Messages;
using Lidgren.Network;
using System;


namespace Gem.Network.Messages
{
    public class MessageSerializer
    {

        public T Decode<T>(NetIncomingMessage im)
            where T: new()
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            im.ReadAllProperties(obj);

            return obj;
        }

        public void Encode<T>(T objectToEncode,ref NetOutgoingMessage om)
        {
            om.WriteAllProperties(objectToEncode);
        }

    }
}
