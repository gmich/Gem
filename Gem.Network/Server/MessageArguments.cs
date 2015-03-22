using Gem.Network.Configuration;
using Gem.Network.Events;
using Gem.Network.Handlers;
using Gem.Network.Messages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Server
{
    public class MessageArguments
    {
        [Key]
        public byte ID { get; set; }

        [Required]
        public Type MessagePoco { get; set; }      

        [Required]
        public IProtocolServerEvent EventRaisingclass { get; set; }

        [Required]
        public IMessageHandler MessageHandler { get; set; }

    }
    
}
