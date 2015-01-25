using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Networking
{
    public class PeerEvent<T> where T : IServerMessage
    {
        public event EventHandler<T> Event;

        public PeerEvent(IServer server,NetConnection connection)
        {
            Event += (sender, e) => server.SendMessage(e, connection);
        }

        public void OnEvent(T e)
        {
            EventHandler<T> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(this, e);
            }
        }       
    }
}
