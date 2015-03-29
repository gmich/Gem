using Gem.Network.Messages;
using Lidgren.Network;
using System;
using Gem.Network.Server;

namespace Gem.Network.Managers
{
    /// <summary>
    /// Manager class for server's behaviors
    /// </summary>
    public class ServerActionManager
    {

        #region Ctor

        public ServerActionManager()
        {
            HandleNotifications = (server, connection, msg) => { };
            OnIncomingConnection = (server, connection, msg) => { };
            OnClientDisconnect = (server, connection, msg) => { };
        }

        #endregion

        #region Behaviors


        /// <summary>
        /// Configures a behavior that's invoked when there's an incoming connection.
        /// Uses the server, the sender's connection and the approval message
        /// </summary>
        public Action<IServer, NetConnection, ConnectionApprovalMessage> OnIncomingConnection { get; set; }

        /// <summary>
        /// Configures how the incoming notifications are handled.
        /// Uses the server, the sender's connection and the notification
        /// </summary>
        public Action<IServer, NetConnection, Notification> HandleNotifications { get; set; }

        /// <summary>
        /// Configures a behavior that's invoked when a client disconnects.
        /// Uses the server, the sender's connection and the disconnect message
        /// </summary>
        public Action<IServer, NetConnection,string> OnClientDisconnect { get; set; }

        #endregion

    }
}
