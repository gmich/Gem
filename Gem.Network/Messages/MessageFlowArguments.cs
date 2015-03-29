using Gem.Network.Events;
using Gem.Network.Handlers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Messages
{
    /// <summary>
    /// The arguments that are needed for the message flow.
    /// </summary>
    public class MessageFlowArguments
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
        public INetworkEvent EventRaisingclass { get; set; }

        /// <summary>
        /// The class that handles the incoming MessagePoco
        /// </summary>
        [Required]
        public IMessageHandler MessageHandler { get; set; }

        /// <summary>
        /// If the outgoing message includes local time
        /// </summary>
        public bool IncludesLocalTime { get; set; }
    }
    
}
