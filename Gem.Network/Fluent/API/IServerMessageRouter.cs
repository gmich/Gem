using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{
    public interface IServerMessageRouter
    {

        void OnIncomingConnection(Action<IServer, NetConnection, ConnectionApprovalMessage> action);

        void OnClientDisconnect(Action<IServer, NetConnection> action);

        void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true);

    }
    
}
