using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network
{
    public interface IClient : INetworkPeer
    {

        bool IsConnected { get;  }

        void Connect(ConnectionDetails connectionDetails, ConnectionApprovalMessage approvalMessage = null);

        void SendMessage<T>(T message);

        void SendMessage<T>(T message,byte id);

        void SendMessage(NetOutgoingMessage gameMessage);

    }
}
