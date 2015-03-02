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
             : AbstractContainer<ActionProvider, ClientMessageType>
    {
        public ActionMessageTypeProvider()
            : base(new FlyweightRepository<ActionProvider, ClientMessageType>())
        { }
    }

    public class ActionProviderArguments
    {
        public Action<NetIncomingMessage> Invoke { get; set; }
    }

    public class ActionProvider
    : AbstractContainer<ActionProviderArguments, byte>
    {
        public ActionProvider()
            : base(new FlyweightRepository<ActionProviderArguments, byte>()) { }

        public IDisposable Add(Action<NetIncomingMessage> action)
        {
            Guard.That(dataRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            byte id = GetUniqueByte();

            return dataRepository.Add(id, new ActionProviderArguments { Invoke = action });
        }

        public override ActionProviderArguments this[byte id]
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

        private byte GetUniqueByte()
        {
            byte uniqueByte = (byte)dataRepository.TotalElements;
            do
            { } while (dataRepository.HasKey(++uniqueByte));

            return uniqueByte;
        }
    }
}
