using Gem.Network.Messages;
using Lidgren.Network;
using System.Collections.Generic;
using System.Net;

namespace Gem.Network.Server
{
    public interface IServer : INetworkPeer
    {
        bool IsConnected { get;
        }
        bool Connect(ServerConfig config);

        List<IPEndPoint> ClientsIP { get; }

        int ClientsCount { get; }

        string Password { get; }

        void Kick(IPEndPoint clientIp,string reason);

        void SendMessage(NetOutgoingMessage message);

        void SendMessage(NetOutgoingMessage message, NetConnection sender);

        void SendMessage(NetOutgoingMessage message, List<NetConnection> clients);

    }
}
