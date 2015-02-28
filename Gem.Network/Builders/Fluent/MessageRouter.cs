using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Configuration
{
    public class MessageRouter : ABuilder
    {
        public MessageRouter(ClientNetworkInfoBuilder builder) : base(builder) { }

        public ClientInfoBuilder Send(MessageType messageType)
        {
            profilesCalled++;
            //builder.clientInfo = messageType;
            return new ClientInfoBuilder(this.builder);
        }

        public void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true)
        {
            throw new NotImplementedException();
        }

        public Executor WhenReceived(MessageType messageType)
        {
            throw new NotImplementedException();
        }
    }

}



