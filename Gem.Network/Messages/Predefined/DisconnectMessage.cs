using Gem.Network.Events;
using Gem.Network.Messages;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Messages
{
    public partial class DisconnectMessage
    {
        public DisconnectMessage(NetIncomingMessage im)
        {
            Message = im.ReadString();
            Sender = im.ReadString();
        }

        public DisconnectMessage()
        {}

        [Required]
        public string Message { get; set; }

        [Required]
        public string Sender { get; set; }

        private readonly byte _id = GemNetwork.DisconnectByte;
        public byte Id
        {
            get
            {
                return _id;
            }
            set
            { }
        }

        public virtual void Encode(NetOutgoingMessage om)
        {
            Guard.That(Message).IsNotNull();
            Guard.That(Sender).IsNotNull();

            om.Write(Message);
            om.Write(Sender);
            om.Write(Id);
        }
    }
}