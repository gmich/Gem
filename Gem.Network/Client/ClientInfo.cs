using Gem.Network.Configuration;
using Gem.Network.Handlers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Repositories
{
    public class ClientInfo
    {
        [Key]
        public byte ID { get; set; }

        [Required]
        public Type MessagePoco { get; set; }

        [Required]
        public object EventRaisingclass { get; private set; }

        [Required]
        public IMessageHandler MessageHandler { get; private set; }
    }
}
