using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{
    public class MessageRouter : IMessageRouter
    {
        private readonly string profile;

        public MessageRouter(string profile) 
        {
            this.profile = profile;
        }

        public IMessageFlowBuilder Send(MessageType messageType)
        {
            return new MessageFlowBuilder(profile, messageType);
        }

        public void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true)
        {
            throw new NotImplementedException();
        }

        public IMessageFlowBuilder WhenReceived(MessageType messageType)
        {
            throw new NotImplementedException();
        }
    }

}



