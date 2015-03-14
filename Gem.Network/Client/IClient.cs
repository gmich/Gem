using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network
{
    public interface IClient : INetworkPeer
    {

        bool IsConnected { get;  }
                
        void Connect(ConnectionConfig connectionDetails,PackageConfig packageConfig, ConnectionApprovalMessage approvalMessage);

        void SendMessage<T>(T message);

        void SendMessage<T>(T message,byte id);

        /// <summary>
        /// Send a message via socket. The class is initialized via activator
        /// </summary>
        /// <typeparam name="T">The object</typeparam>
        /// <param name="args">The parameter fields</param>
        void SendMessage<T>(object[] args);

        void SendMessage(NetOutgoingMessage msg);

        void Wait();
    }
}
