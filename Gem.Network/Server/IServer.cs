using Gem.Network.Messages;
using Lidgren.Network;
using System.Collections.Generic;
using System.Net;

namespace Gem.Network.Server
{
    /// <summary>
    /// The base interface for servers
    /// Shows info, starts server and sends messages
    /// </summary>
    public interface IServer : INetworkPeer
    {
        #region Info

        bool IsConnected { get; }

        IPAddress IP { get; }

        int Port { get; }

        List<IPEndPoint> ClientsIP { get; }

        int ClientsCount { get; }

        string Password { get; }

        #endregion

        #region Start

        bool Connect(ServerConfig config, PackageConfig packageConfig);

        #endregion

        #region Stall Thread

        /// <summary>
        /// Stalls the running thread until an event is raised
        /// </summary>
        void Wait();

        #endregion

        #region Management

        bool Kick(IPAddress clientIp, string reason);

        bool Kick(IPEndPoint clientIp, string reason);

        #endregion

        #region Send Messages

        void NotifyAll(string message);

        void SendNotification(Notification notification);

        void NotifyAllExcept(string message, NetConnection client, string type = NotificationType.Message);

        void NotifyOnly(string message, NetConnection client, string type = NotificationType.Message);

        void SendToAll(NetOutgoingMessage message);

        void SendAndExclude(NetOutgoingMessage message, NetConnection excludeSender);

        void SendOnlyTo(NetOutgoingMessage message, NetConnection sender);

        void SendMessage(NetOutgoingMessage message, List<NetConnection> clients);

        void SendMessage<T>(NetConnection sender, T message, byte id);

        #endregion
    }
}
