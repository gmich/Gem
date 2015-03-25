using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network
{
    /// <summary>
    /// The base interface for network clients. Handle connection / send messages
    /// </summary>
    public interface IClient : INetworkPeer
    {

        /// <summary>
        /// Shows if there's an active connection to a server
        /// </summary>
        bool IsConnected { get;  }

        void Connect(ConnectionConfig connectionDetails,PackageConfig packageConfig, ConnectionApprovalMessage approvalMessage);

        /// <summary>
        /// Serialize an object to a network outgoing message and send it via socket.
        /// </summary>
        /// <typeparam name="T">The objecto to serialzie and send</typeparam>
        void SendMessage<T>(T message);

        /// <summary>
        /// Serialize an object to a network outgoing message and send it via socket.
        /// Include local time and the package's unique id
        /// </summary>
        /// <typeparam name="T">The object's type to serialzie and send</typeparam>
        /// <param name="message">The actual message</param>
        /// <param name="id">It's id</param>
        void SendMessageWithLocalTime<T>(T message, byte id);

        /// <summary>
        /// Send a notification to the server
        /// </summary>
        /// <param name="notification">The notification</param>
        void SendNotification(Notification notification);

        /// <summary>
        /// Send a message via socket. The class is initialized via reflection
        /// </summary>
        /// <typeparam name="T">The object</typeparam>
        /// <param name="args">The parameter fields</param>
        void SendMessage<T>(object[] args);

        void SendMessage(NetOutgoingMessage msg);

        /// <summary>
        /// Holds the message receiving thread until an event fires 
        /// </summary>
        void Wait();
    }
}
