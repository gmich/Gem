using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Protocol;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{

    public interface IClientMessageRouter
    {
        void OnReceivedServerNotification(Action<Notification> action, bool append = false);

        void HandleErrors(Action<IClient, NetIncomingMessage> action, bool append = false);

        void HandleWarnings(Action<IClient, NetIncomingMessage> action, bool append = false);

        void WhenConnected(Action<IClient, NetIncomingMessage> action, bool append = false);

        void OnConnecting(Action<IClient, NetIncomingMessage> action, bool append = false);

        void OnDisconnected(Action<IClient, NetIncomingMessage> action, bool append = false);

        void OnDisconnecting(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// The protocol class must be tagged with the NetworkPackage attribute
        /// </summary>
        /// <typeparam name="TNetworkPackage">The object tagged with the NetworkPackage attribute</typeparam>
        /// <returns>The builder</returns>
        IProtocolMessageBuilder<TNetworkPackage> CreateNetworkProtocolEvent<TNetworkPackage>()
                where TNetworkPackage : new();

        IMessageFlowBuilder CreateNetworkEvent { get; }

        IMessageFlowBuilder CreateNetworkEventWithRemoteTime { get; }
    }

}
