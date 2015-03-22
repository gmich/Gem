using System;
using Lidgren.Network;
using Gem.Network.Messages;

namespace Gem.Network
{
    /// <summary>
    /// The base interface for clients and servers
    /// </summary>
    public interface INetworkPeer : IDisposable
    {
        PackageConfig PackageConfig { get; set; }

        NetOutgoingMessage CreateMessage();

        NetIncomingMessage ReadMessage();

        void Recycle(NetIncomingMessage im);

        void SendMessage<T>(T message, byte id);

        void Disconnect();

    }

}