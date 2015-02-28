using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{
    public interface IMessageRouter
    {
        IMessageFlowBuilder Send(MessageType messageType);    

        void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true);

        IMessageFlowBuilder WhenReceived(MessageType messageType);
    }
}
