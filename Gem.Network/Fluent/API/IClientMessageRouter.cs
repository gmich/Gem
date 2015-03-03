using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{

    public interface IClientMessageRouter
    {
        void HandleErrors(Action<Lidgren.Network.NetIncomingMessage> action);

        void HandleWarnings(Action<Lidgren.Network.NetIncomingMessage> action);

        ActionDirector WhenConnected { get; }

        ActionDirector OnConnecting { get; }

        ActionDirector OnDisconnected { get; }

        ActionDirector OnDisconnecting { get; }

        ActionDirector OnHandshake { get; }

        IMessageFlowBuilder CreateNetworkEvent { get; }
    }

}
