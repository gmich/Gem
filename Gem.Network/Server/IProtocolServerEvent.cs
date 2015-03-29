using Lidgren.Network;
using System;

namespace Gem.Network.Server
{
    /// <summary>
    /// Raises events that use the NetworkPackageAttribute
    /// by serializing and sending packages via server
    /// </summary>
    public interface IProtocolServerEvent : IDisposable
    {
        /// <summary>
        /// Raise the event that sends a message and excludes a connection
        /// </summary>
        /// <param name="excluded">The excluded connection</param>
        /// <param name="netpackage">The message to serialize and send</param>
        void Send(NetConnection excluded, object netpackage);

        /// <summary>
        /// Subscribe the networkPeer that sends the message
        /// </summary>
        /// <param name="server"></param>
        void SubscribeEvent(INetworkPeer server);
    }
}
