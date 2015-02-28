using Gem.Network.Factories;
using Gem.Network.Other;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Containers
{
    using Extensions;

    public class ClientConfigurationContainer
    {
        private readonly GenericRepository<MessageFlowArguments, byte> clientInfoRepository;

        public ClientConfigurationContainer()
        {
            clientInfoRepository = new GenericRepository<MessageFlowArguments, byte>(x => x.ID);
        }

        public IDisposable AddConfig(MessageFlowArguments clientInfo)
        {
            Guard.That(clientInfoRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            clientInfo.ID = GetUniqueByte();
            return clientInfoRepository.Add(clientInfo.ID,clientInfo);            
        }

        public void SubscribeEvents(IClient client)
        {
            clientInfoRepository.GetAll()
                                .ForEach(x => x.EventRaisingclass.SubscribeEvent(client));
        }

        public IEnumerable<MessageFlowArguments> Query(Func<MessageFlowArguments,bool> whereClause)
        {
            return clientInfoRepository.GetAll().Where(whereClause);
        }

        public void GetById(byte id)
        {
            clientInfoRepository.GetById(id);
        }

        private byte GetUniqueByte()
        {
            byte randomByte;
            do
            {
                randomByte = RandomGenerator.GetRandomByte();

            } while (clientInfoRepository.HasKey(randomByte));

            return randomByte;
        }
    }
       
}
