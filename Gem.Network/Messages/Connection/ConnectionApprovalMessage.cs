using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network.Messages
{
    public partial class ConnectionApprovalMessage
    {

        public string Message { get; set; }

        public string Sender { get; set; }

        public string Password { get; set; }

        public byte ID
        {
            get
            {
                return 1;
            }
        }

    }
}