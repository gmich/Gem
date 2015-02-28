using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Messages
{
    public enum MessageType
    {

        Hail,
    
        ConnectionApproval,

        NewClient,

        Command,
                
        Disconnect,

        DiscoveryResponse,

        Data,

        Warning,

        Error
    }

    public static class NetIncomingMessageTypeExtensions
    {
        private static readonly Dictionary<NetIncomingMessageType, MessageType> Matcher
        = new Dictionary<NetIncomingMessageType, MessageType>
            {
                  { NetIncomingMessageType.ConnectionApproval,MessageType.ConnectionApproval},
                  { NetIncomingMessageType.Data,              MessageType.Data},
                  { NetIncomingMessageType.Error,             MessageType.Error},
                  //TODO: complete
            };

        public static MessageType Transform(this NetIncomingMessageType messageType)
        {
            return Matcher[messageType];
        }
    }
}

