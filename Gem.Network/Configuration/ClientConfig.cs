using Gem.Network.Factories;
using Gem.Network.Other;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    using Extensions;

    public class NetworkFacade
    {
        NetworkProfileRepository Profiles;
        ClientConfig clientConfig;

    }

    public class ClientConfig
    {
        private readonly GenericRepository<ClientNetworkInfo, byte> clientInfoRepository;

        public ClientConfig()
        {
            clientInfoRepository = new GenericRepository<ClientNetworkInfo, byte>(x => x.ID);
        }

        public void AddConfig(ClientNetworkInfo clientInfo)
        {
            Guard.That(clientInfoRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            clientInfo.ID = GetUniqueByte();
            clientInfoRepository.Add(clientInfo);
        }

        public void SubscribeEvents(IClient client)
        {
            clientInfoRepository.GetAll()
                                .ForEach(x => x.EventRaisingclass.SubscribeEvent(client));
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
            } while (!clientInfoRepository.HasKey(randomByte));

            return randomByte;
        }
    }

    public class ClientNetworkInfoBuilder
    {
        public readonly ClientNetworkInfo networkInfo;
        public readonly IEventFactory eventFactory;
        public readonly IMessageHandlerFactory handlerFactory;
        public readonly IPocoFactory pocoFactory;
        private readonly ClientConfig clientConfig;

        public string ProfileName { get; set; }

        public ClientNetworkInfoBuilder(ClientConfig clientConfig,
                                        IEventFactory eventFactory,
                                        IMessageHandlerFactory handlerFactory,
                                        IPocoFactory pocoFactory)
        {
            networkInfo = new ClientNetworkInfo();
            this.eventFactory = eventFactory;
            this.handlerFactory = handlerFactory;
            this.pocoFactory = pocoFactory;
            this.clientConfig = clientConfig;
        }

        public void End()
        {
            clientConfig.AddConfig(networkInfo);
        }
    }

}
