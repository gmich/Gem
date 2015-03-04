using Gem.Network.Events;
using Gem.Network.Messages;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Messages
{
    public partial class ConnectionApprovalMessage
    {
        public ConnectionApprovalMessage(NetIncomingMessage im)
        {
            Message = im.ReadString();
            Sender = im.ReadString();
            Password = im.ReadString();
        }

        public ConnectionApprovalMessage()
        {}

        [Required]
        public string Message { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string Password { get; set; }

        private readonly byte _id = 1;
        public byte Id
        {
            get
            {
                return _id;
            }
            set
            { }
        }

        public void Encode(NetOutgoingMessage om)
        {
            Guard.That(Message).IsNotNull();
            Guard.That(Sender).IsNotNull();
            Guard.That(Password).IsNotNull();

            om.Write(Message);
            om.Write(Sender);
            om.Write(Password);
            om.Write(Id);
        }
    }
}