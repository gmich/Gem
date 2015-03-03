using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;

namespace Gem.Network.Providers
{

    public class PredefinedMessageTypeProvider
             : AbstractContainer<PredefinedMessageFlow, ClientMessageType>
    {
        public PredefinedMessageTypeProvider()
            : base(new FlyweightRepository<PredefinedMessageFlow, ClientMessageType>())
        { }
    }

    public class PredefinedMessageFlow
             : AbstractContainer<MessageFlowArguments, byte>
    {
        public PredefinedMessageFlow()
            : base(new FlyweightRepository<MessageFlowArguments, byte>())
        { }

        public IDisposable Add(MessageFlowArguments clientInfo,byte id)
        {
            Guard.That(dataRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            Guard.That(dataRepository).IsTrue(x => !x.HasKey(id),
            "This key has already been registered");

            clientInfo.ID = id;

            return dataRepository.Add(clientInfo.ID, clientInfo);
        }

    
        public override MessageFlowArguments this[byte id]
        {
            get
            {
                if (dataRepository.HasKey(id))
                {
                    return dataRepository.GetById(id);
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
