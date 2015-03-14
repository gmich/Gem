using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;

namespace Gem.Network.Providers
{
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
                    return new ActionProviderArguments();
                }
            }
        }

        public Action<Notification> OnReceivedNotification { get; set; }

    }

    public class ActionProviderArguments
    {
        public ActionProviderArguments()
        {
            Action = x => { };
        }

        public Action<IClient> Action { get; set; }
    }
}
