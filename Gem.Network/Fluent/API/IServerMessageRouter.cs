using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{
    /// <summary>
    /// The API for configuring GemServer's behavior and message flow
    /// </summary>
    public interface IServerMessageRouter
    {
        /// <summary>
        /// Configures a behavior that's invoked when there's an incoming connection
        /// </summary>
        /// <param name="action">The action with the server, the incoming connection and the approval message</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnIncomingConnection(Action<IServer, NetConnection, ConnectionApprovalMessage> action,
                                  bool append = false);

        /// <summary>
        /// Configures a behavior that's invoked when a client disconnects
        /// </summary>
        /// <param name="action">The action with the server, the incoming connection and the disconnect message as a string</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void OnClientDisconnect(Action<IServer, NetConnection, string> action,
                                bool append = false);

        /// <summary>
        /// Configures how the incoming notifications are handled
        /// </summary>
        /// <param name="action">The action with the server, the incoming connection and the notification</param>
        /// <param name="append">If the delegate is appends or overrides the others</param>
        void HandleNotifications(Action<IServer, NetConnection, Notification> action,
                                 bool append = false);

        /// <summary>
        /// Registers a command to the servers' CommandExecutioner
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="description">The command's descriptiopn </param>
        /// <param name="callback">The <see cref="Gem.Network.Commands>"/> callback that's invoked when someone executes a command</param>
        /// <param name="requiresAuthorization">If a password is required to execute the command</param>
        void RegisterCommand(string command, string description, CommandExecute callback,
                             bool requiresAuthorization = true);

        /// <summary>
        /// Creates events and handlers that are associated with the NetworkPackage attribute.
        /// </summary>
        /// <typeparam name="TNetworkPackage">The object's type that's annotated by the NetworkPackage attribute</typeparam>
        IServerProtocolMessageBuilder<TNetworkPackage> CreateNetworkProtocolEvent<TNetworkPackage>()
                             where TNetworkPackage : new();

    }

}
