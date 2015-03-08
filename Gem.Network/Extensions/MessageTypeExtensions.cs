using Lidgren.Network;
using System;
using System.Collections.Generic;
using Gem.Network.Extensions;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;

namespace Gem.Network.Extensions
{
    public static class MessageTypeExtensions
    {
        public static void ClientHandle(this MessageType messageType, NetIncomingMessage im)
        {
            byte id = im.ReadByte();

            //if (GemNetwork.ClientActionManager[GemNetwork.ActiveProfile, messageType].HasKey(id))
            //{
            //    GemNetwork.ClientActionManager[GemNetwork.ActiveProfile, messageType, id](message);
            //}
            if (GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType].HasKey(id))
            {
                GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType, id]
                      .SendCachedMessage();
                GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType, id]
                      .HandleIncomingMessage(im);
            }
        }

        public static void ServerHandle(this MessageType messageType, NetIncomingMessage message)
        {
            byte id = message.ReadByte();
            //if (GemNetwork.ServerActionManager[GemNetwork.ActiveProfile, messageType].HasKey(id))
            //{
            //    GemNetwork.ClientActionManager[GemNetwork.ActiveProfile, messageType, id](message);
            //}
            //if (GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType].HasKey(id))
            //{
            //    GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType, id]
            //          .SendCachedMessage();
            //    GemNetwork.ClientMessageFlow[GemNetwork.ActiveProfile, messageType, id]
            //          .HandleIncomingMessage(message);
            //}
        }
    }
}
