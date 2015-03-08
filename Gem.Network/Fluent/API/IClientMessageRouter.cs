using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{

    public interface IClientMessageRouter
    {
        IActionDirector HandleErrors { get; }

        IActionDirector HandleWarnings { get; }

        IActionDirector WhenConnected { get; }

        IActionDirector OnConnecting { get; }

        IActionDirector OnDisconnected { get; }

        IActionDirector OnDisconnecting { get; }

        IActionDirector OnHandshake { get; }

        IMessageFlowBuilder CreateNetworkEvent { get; }
    }

}
