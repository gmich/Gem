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
 
        bool Connect(ServerConfig config);

        List<IPEndPoint> ClientsIP { get; }

        int ClientsCount { get; }

        string Password { get; }

        void Kick(IPEndPoint clientIp,string reason);

        void NotifyAll(string message);

        void SendToAll(NetOutgoingMessage message);

        void SendAndExclude(NetOutgoingMessage message, NetConnection excludeSender);

        void SendOnlyTo(NetOutgoingMessage message, NetConnection sender);

        void SendMessage(NetOutgoingMessage message, List<NetConnection> clients);

    }
}
