using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;
using Gem.Network.Server;

namespace Gem.Network.Managers
{
    public class ServerActionManager
    {

        public Action<IServer, NetConnection, ConnectionApprovalMessage> OnIncomingConnection { get; set; }

        public Action<IServer, NetConnection, Notification> HandleNotifications { get; set; }

        public Action<IServer, NetConnection,string> OnClientDisconnect { get; set; }

        public ServerActionManager()
        {
            HandleNotifications = (server, connection, msg) => { };
            OnIncomingConnection = (server, connection, msg) => { };
            OnClientDisconnect = (server, connection, msg) => { };
        }
    }
}
