using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{

    public interface IClientMessageRouter
    {
        void HandleErrors(Action<Lidgren.Network.NetIncomingMessage> action);

        void HandleWarnings(Action<Lidgren.Network.NetIncomingMessage> action);

        ActionDirector OnConnected();

        ActionDirector OnConnecting();

        ActionDirector OnDisconnected();

        ActionDirector OnDisconnecting();

        ActionDirector OnHandshake();

        IMessageFlowBuilder CreateNetworkEvent();
    }

}
