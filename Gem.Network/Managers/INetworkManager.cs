namespace Gem.Network
{
    using System;
    using Lidgren.Network;
    using Gem.Network.Messages;


    enum PacketTypes
    {
        LOGIN
    }

    /// <summary>
    /// The base interface for clients and servers
    /// </summary>
    public interface INetworkManager : IDisposable
    {

        void Connect(string serverName, int port);

        NetOutgoingMessage CreateMessage();

        void Disconnect();

        NetIncomingMessage ReadMessage();

        void Recycle(NetIncomingMessage im);

    }
}