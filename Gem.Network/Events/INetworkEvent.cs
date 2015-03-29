using System;

namespace Gem.Network.Events
{
    /// <summary>
    /// Raises events by serializing and sending packages via socket
    /// </summary>
    public interface INetworkEvent : IDisposable
    {
        /// <summary>
        /// Subscribes the send event to the connected client.
        /// When Send(params object[] networkargs) is invoked, an event is raised,
        /// sending a message from the connected client to the server 
        /// </summary>
        /// <param name="client">The client that sends the message</param>
        void SubscribeEvent(INetworkPeer client);

        /// <summary>
        /// Subscribes the send event to the connected client.
        /// When Send(params object[] networkargs) is invoked, an event is raised,
        /// sending a message from the connected client to the server 
        /// </summary>
        /// <param name="client">The client that sends the message</param>
        void Send(params object[] networkargs);
    }
}
