using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{
    public interface IServerMessageRouter
    {

        void OnIncomingConnection(Action<IServer, NetConnection, ConnectionApprovalMessage> action, bool append = false);

        void OnClientDisconnect(Action<IServer, NetConnection,string> action, bool append = false);

        void HandleNotifications(Action<IServer, NetConnection, Notification> action, bool append = false);

        void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true);

        IServerProtocolMessageBuilder<T> CreateNetworkProtocolEvent<T>() where T : new();
    }
    
}
