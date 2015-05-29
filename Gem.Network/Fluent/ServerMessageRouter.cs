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
    public class ServerMessageRouter : IServerMessageRouter
    {
        private readonly string profile;

        public ServerMessageRouter(string profile)
        {
            this.profile = profile;
        }

        public IServerProtocolMessageBuilder<T> CreateNetworkProtocolEvent<T>()
              where T : new()
        {
            return new ServerMessageFlowNetworkProtocol<T>(profile, MessageType.Data);
        }

        public void OnIncomingConnection(Action<IServer, NetConnection, ConnectionApprovalMessage> action, bool append = false)
        {
            if (append)
            {
                GemServer.ServerConfiguration[profile].OnIncomingConnection += action;
            }
            else
            {
                GemServer.ServerConfiguration[profile].OnIncomingConnection = action;
            }
        }

        public void OnClientDisconnect(Action<IServer, NetConnection,string> action, bool append = false)
        {
            if (append)
            {
                GemServer.ServerConfiguration[profile].OnClientDisconnect += action;
            }
            else
            {
                GemServer.ServerConfiguration[profile].OnClientDisconnect = action;
            }
        }

        public void RegisterCommand(string command, string description, ExecuteCommand callback, bool requiresAuthorization = true)
        {
            GemNetwork.Commander.RegisterCommand(command,requiresAuthorization, description, callback);
        }

        public void HandleNotifications(Action<IServer, NetConnection, Notification> action, bool append = false)
        {
            if (append)
            {
                GemServer.ServerConfiguration[profile].HandleNotifications += action;
            }
            else
            {
                GemServer.ServerConfiguration[profile].HandleNotifications = action;
            }
        }

    }
}


