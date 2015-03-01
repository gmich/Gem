using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Other;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;

namespace Gem.Network.Managers
{
    public class ClientMessageFlowInfoProvider
             : AbstractContainer<MessageFlowArguments, byte>
    {
        public ClientMessageFlowInfoProvider()
            : base(new FlyweightRepository<MessageFlowArguments, byte>())
        { }

        public IDisposable Add(MessageFlowArguments clientInfo)
        {
            Guard.That(dataRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            clientInfo.ID = GetUniqueByte();

            return dataRepository.Add(clientInfo.ID, clientInfo);
        }

        public void SubscribeEvent(byte id)
        {
            this[id].EventRaisingclass.SubscribeEvent(GemNetwork.Client);
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

        private byte GetUniqueByte()
        {
            byte randomByte;
            do
            {
                randomByte = RandomGenerator.GetRandomByte();

            } while (dataRepository.HasKey(randomByte));

            return randomByte;
        }
    }
}
