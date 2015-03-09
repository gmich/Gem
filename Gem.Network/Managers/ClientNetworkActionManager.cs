﻿using Gem.Network.Messages;
using Gem.Network.Providers;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;

namespace Gem.Network.Managers
{   
    
    public class ClientNetworkActionManager
    {
        private NetworkActionProvider actionProvider;

        public ClientNetworkActionManager()
        {
            actionProvider = new NetworkActionProvider();
        }

        public Action<NetIncomingMessage> this[string tag,MessageType messagetype,byte configID]
        {
            get
            {
                return actionProvider[tag][messagetype][configID].Invoke;
            }
        }

        public ActionProvider this[string tag, MessageType messagetype]
        {
            get
            {
                return actionProvider[tag][messagetype];
            }
        }

        public ActionMessageTypeProvider this[string tag]
        {
            get
            {
                return actionProvider[tag];
            }
        }
    }

}