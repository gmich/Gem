using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;
using Gem.Network.Server;

namespace Gem.Network.Managers
{
    public class ServerMessageFlowManager
    {

        public Action<IServer, NetConnection, ConnectionApprovalMessage> OnIncomingConnection { get; set; }

        public Action<IServer, NetConnection> OnClientDisconnect { get; set; }


        //public Action DiscoveryResponse { get; set; }

        public ServerMessageFlowManager()
        {
            OnIncomingConnection = null;
            OnClientDisconnect = null;
        }

    }
}
