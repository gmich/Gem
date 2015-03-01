using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network.Messages
{
    public partial class ConnectionApprovalMessage
    {

        public string Message { get; private set; }

        public string Sender { get; private set; }

        public string Password { get; private set; }

        public byte ID
        {
            get
            {
                return 1;
            }
        }

    }
}