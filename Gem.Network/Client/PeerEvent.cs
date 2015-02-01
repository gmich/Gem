using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Networking
{
    //TODO: create a wrapper interface for NetOutgoingMessages
    //Perform message encoding
    public class PeerEvent<T>
    {
        public event EventHandler<NetOutgoingMessage> Event;

        public void SubscribeEvent(IClient client)
        {
            Event += (sender, e) => client.SendMessage(e);
        }

        public void OnEvent(NetOutgoingMessage arg)
        {
            EventHandler<NetOutgoingMessage> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(this, arg);
            }
        }
    }
}
