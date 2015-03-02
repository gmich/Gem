using Lidgren.Network;
using System;
using System.Collections.Generic;
using Gem.Network.Extensions;
using Gem.Network.Messages;

namespace Gem.Network.Extensions
{
    public static class MessageTypeExtensions
    {
        private static readonly Dictionary<NetIncomingMessageType, ClientMessageType> Matcher
        = new Dictionary<NetIncomingMessageType, ClientMessageType>
            {
                 // { NetIncomingMessageType.ConnectionApproval,ClientMessageType.Connecting},
                  { NetIncomingMessageType.Data,              ClientMessageType.Data},
                  { NetIncomingMessageType.Error,             ClientMessageType.Error},
                  //TODO: complete
            };

        public static ClientMessageType Transform(this NetIncomingMessageType messageType)
        {
            return Matcher[messageType];
        }

        public static void Handle(this ClientMessageType messageType, NetIncomingMessage message)
        {
            byte id = message.ReadByte();
            if (GemNetwork.ClientActionManager[GemNetwork.ActiveProfile, messageType].HasKey(id))
            {
                GemNetwork.ClientActionManager[GemNetwork.ActiveProfile, messageType, id](message);
            }
            if (GemNetwork.ClientActionManager[GemNetwork.ActiveProfile, messageType].HasKey(id))
            {
                GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType, id]
                  .SendCachedMessage();
                GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType, id]
                      .HandleIncomingMessage(message);
            }
        }
    }
}
