using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network.Messages
{
    public partial class ConnectionApprovalMessage
    {

        public string Message { get; set; }

        public string Sender { get; set; }

        public string Password { get; set; }

        public MessageType ID
        {
            get
            {
                return MessageType.ConnectionApproval;
            }
        }

    }
}