using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Networking
{
    public class PeerEvent
    {
        public event EventHandler<IServerMessage> Event;

        public void SubscribeEvent(IServer server,NetConnection connection)
        {
            Event += (sender, e) => server.SendMessage(e, connection);
        }

        public void OnEvent(Func<IServerMessage> serverMessage)
        {
            EventHandler<IServerMessage> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(this, serverMessage());
            }
        }       
    }
}
