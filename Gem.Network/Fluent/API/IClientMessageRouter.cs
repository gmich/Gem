using Gem.Network.Client;
using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Protocol;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{

    /// <summary>
    /// The API for configuring GemClients behavior and message flow
    /// </summary>
    public interface IClientMessageRouter
    {
        /// <summary>
        /// Configures a behavior that's invoked when the client receives a notification from the server
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnReceivedServerNotification(Action<Notification> action, bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when an error occurs
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void HandleErrors(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when a warning occurs
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void HandleWarnings(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when the client connects to a server
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnConnected(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when the client is initiating connect to a server
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnConnecting(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when the client disconnects from a server
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnDisconnected(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when a client is initiating disconnect from a server
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnDisconnecting(Action<IClient, NetIncomingMessage> action, bool append = false);

        /// <summary>
        /// The protocol class must be tagged with the NetworkPackage attribute
        /// </summary>
        /// <typeparam name="TNetworkPackage">The object tagged with the NetworkPackage attribute</typeparam>
        /// <returns>The builder</returns>
        IClientProtocolMessageBuilder<TNetworkPackage> CreateNetworkProtocolEvent<TNetworkPackage>()
                where TNetworkPackage : new();

        /// <summary>
        /// Creates a network event that sends and handles messages
        /// </summary>
        IMessageFlowBuilder CreateNetworkEvent { get; }

        /// <summary>
        /// Creates a network event that sends, handles message and includes the client's active time
        /// </summary>
        IMessageFlowBuilder CreateNetworkEventWithRemoteTime { get; }
    }

}
