using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{

    public class ServerMessageRouter : IServerMessageRouter
    {
        private readonly string profile;

        public ServerMessageRouter(string profile) 
        {
            this.profile = profile;
        }

        public void ForIncomingConnections(Action<IServer, NetConnection,ConnectionApprovalMessage> action)
        {
            GemNetwork.ServerConfiguration[profile].ConnectionApprove = action;
        }

        public void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true)
        {
            throw new NotImplementedException();
        }

        public IMessageFlowBuilder WhenReceived(ClientMessageType messageType)
        {
            throw new NotImplementedException();
        }
    }

}



