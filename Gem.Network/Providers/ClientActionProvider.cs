﻿using Gem.Network.Client;
using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using System;

namespace Gem.Network.Providers
{
    /// <summary>
    /// Readonly provider for ActionProviderArguments that are accessed first by string
    /// and by messagetype through ActionMessageTypeProvider
    /// </summary>
    internal class NetworkActionProvider
           : AbstractContainer<ActionMessageTypeProvider, string>
    {
        public NetworkActionProvider()
            : base(new FlyweightRepository<ActionMessageTypeProvider, string>())
        { }
    }

    public class ActionMessageTypeProvider
             : AbstractContainer<ActionProviderArguments, MessageType>
    {
        public ActionMessageTypeProvider()
            : base(new FlyweightRepository<ActionProviderArguments, MessageType>())
        {
            OnReceivedNotification = (x) => { };
        }

        public override ActionProviderArguments this[MessageType id]
        {
            get
            {
                if (dataRepository.HasKey(id))
                {
                    return dataRepository.GetById(id);
                }
                else
                {
                    var providerArgs = new ActionProviderArguments();
                    dataRepository.Add(id, providerArgs);
                    return providerArgs;
                }
            }
        }

        public Action<Notification> OnReceivedNotification { get; set; }

    }
    
    public class ActionProviderArguments
    {
        public ActionProviderArguments()
        {
            Action = (client,message) => { };
        }

        public Action<IClient,NetIncomingMessage> Action { get; set; }
    }
}
