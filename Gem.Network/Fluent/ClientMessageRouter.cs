using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{
    public class ClientMessageRouter : IClientMessageRouter
    {
        private readonly string profile;

        public ClientMessageRouter(string profile) 
        {
            this.profile = profile;
        }

        public IMessageFlowBuilder Send(MessageType messageType)
        {
            return new MessageFlowBuilder(profile, messageType);
        }
        
        public IMessageFlowBuilder WhenReceived(MessageType messageType)
        {
            throw new NotImplementedException();
        }
    }

}



