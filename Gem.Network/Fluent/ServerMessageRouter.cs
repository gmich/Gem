using Gem.Network.Commands;
using Gem.Network.Messages;
using Gem.Network.Server;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{

    public class ServerMessageRouter : IServerMessageRouter
    {
        private readonly string profile;

        public ServerMessageRouter(string profile)
        {
            this.profile = profile;
        }

        public void OnIncomingConnection(Action<IServer, NetConnection, ConnectionApprovalMessage> action)
        {
            GemNetwork.ServerConfiguration[profile].OnIncomingConnection = action;
        }
        
        public void OnClientDisconnect(Action<IServer, NetConnection> action)
        {
            GemNetwork.ServerConfiguration[profile].OnClientDisconnect = action;
        }

        public void RegisterCommand(string command, string description, CommandExecute callback, bool requiresAuthorization = true)
        {
            GemNetwork.Commander.RegisterCommand(command,requiresAuthorization, description, callback);
        }

    }
}


