using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{
    public interface IServerMessageRouter
    {

        void ForIncomingConnections(Action<IServer, NetConnection, ConnectionApprovalMessage> action);
       
        void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true);

        IMessageFlowBuilder WhenReceived(MessageType messageType);

    }
}
