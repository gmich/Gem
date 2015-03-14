using Gem.Network.Messages;
using Lidgren.Network;
using System.Collections.Generic;
using System.Net;

namespace Gem.Network.Server
{
    public interface IServer : INetworkPeer
    {
        bool IsConnected { get; }

        IPAddress IP { get; }

        int Port { get; }
 
        bool Connect(ServerConfig config,PackageConfig packageConfig);

        List<IPEndPoint> ClientsIP { get; }

        int ClientsCount { get; }

        string Password { get; }

        void Wait();

        bool Kick(IPAddress clientIp,string reason);

        bool Kick(IPEndPoint clientIp, string reason);

        void NotifyAll(string message);

        void NotifyOnly(string message,NetConnection client);

        void SendToAll(NetOutgoingMessage message);

        void SendAndExclude(NetOutgoingMessage message, NetConnection excludeSender);

        void SendOnlyTo(NetOutgoingMessage message, NetConnection sender);

        void SendMessage(NetOutgoingMessage message, List<NetConnection> clients);

    }
}
