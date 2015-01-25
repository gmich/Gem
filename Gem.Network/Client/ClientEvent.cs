using Gem.Network.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Networking
{
    public class PeerEvent<T> where T : IServerMessage
    {
        public event EventHandler<T> Event;

        public PeerEvent(INetworkManager networkManager,NetConnection connection)
        {
            Event += (sender, e) => networkManager.SendMessage(e, connection);
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
