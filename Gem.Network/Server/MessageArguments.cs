using Gem.Network.Handlers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Server
{
    /// <summary>
    /// The arguments that are needed for the server side message flow.
    /// </summary>
    public class MessageArguments
    {
        [Key]
        public byte ID { get; set; }

        /// <summary>
        /// The outgoing message's type
        /// </summary>
        [Required]
        public Type MessagePoco { get; set; }

        /// <summary>
        /// The class that raises the event that sends MessagePoco
        /// </summary>
        [Required]
        public IProtocolServerEvent EventRaisingclass { get; set; }

        /// <summary>
        /// The class that handles the incoming MessagePoco
        /// </summary>
        [Required]
        public IMessageHandler MessageHandler { get; set; }

    }
    
}
