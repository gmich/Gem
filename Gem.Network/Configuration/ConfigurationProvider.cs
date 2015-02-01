using Gem.Network.Messages;
using Gem.Network.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public class ConfigurationProvider<TMessage>
    {
        Dictionary<IncomingMessageTypes, Action<TMessage>> MessageHandlers;
        
        public void AddConfiguration(IncomingMessageTypes type, Action<TMessage>action)
        {
            //PeerEvent<TMessage> peerEvent = new PeerEvent<TMessage>()
            MessageHandlers.Add(type,action);
        }
    }
}
