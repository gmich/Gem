using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Networking
{
    //TODO: create a wrapper interface for NetOutgoingMessages
    //Perform message encoding
    public class ClientEvent<T>
    {
        private event EventHandler<T> Event;

        public void SubscribeEvent(IClient client)
        {
            Event += (sender, e) => client.SendMessage<T>(e);
        }

        public void OnEvent(object message)
        {
            EventHandler<T> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(this, (T)message);
            }
        }
    }
}
