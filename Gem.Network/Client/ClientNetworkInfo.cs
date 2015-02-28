using Gem.Network.Configuration;
using Gem.Network.Events;
using Gem.Network.Handlers;
using Gem.Network.Messages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Repositories
{
    public class ClientNetworkInfo
    {
        [Key]
        public byte ID { get; set; }

        [Required]
        public MessageType MessageType { get; set; }

        [Required]
        public Type MessagePoco { get; set; }      

        [Required]
        public INetworkEvent EventRaisingclass { get; set; }

        [Required]
        public IMessageHandler MessageHandler { get; set; }

    }
}
