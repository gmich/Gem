using Gem.Network.Messages;
using Lidgren.Network;
using System.Collections.Generic;
using System.Net;

namespace Gem.Network
{
    public interface IServer : INetworkManager
    {

        List<IPEndPoint> ConnectedUsers { get; }

        void Kick(IPEndPoint clientIp);

        void SendMessage(NetOutgoingMessage message);

        void SendMessage(NetOutgoingMessage message, NetConnection sender);

        void SendMessage(NetOutgoingMessage message, List<NetConnection> clients);
    }
}
