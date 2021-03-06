﻿using Gem.Network.Containers;
using Gem.Network.Messages;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;

namespace Gem.Network.Managers
{
    /// <summary>
    /// Readonly provider for MessageFlowArguments that are accessed by index[byte id]
    /// </summary>
    public class MessageFlowInfoProvider
             : AbstractContainer<MessageFlowArguments, byte>
    {
        public MessageFlowInfoProvider()
            : base(new FlyweightRepository<MessageFlowArguments, byte>())
        { }

        public IDisposable Add(MessageFlowArguments clientInfo)
        {
            Guard.That(dataRepository).IsTrue(x => x.TotalElements < (int)byte.MaxValue,
            "You have reached the maximum capacity. Consider deregistering");

            if (clientInfo.ID == (byte)0)
            {
                clientInfo.ID = GetUniqueByte();
            }
            //if a repository already has that key, replace
            else if (dataRepository.HasKey(clientInfo.ID))
            {
                dataRepository.GetById(clientInfo.ID).ID = GetUniqueByte();
            }

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
            byte uniqueByte = (byte)(GemNetwork.InitialId + dataRepository.TotalElements);
            do
            { } while (dataRepository.HasKey(++uniqueByte));

            return uniqueByte;
        }
    }
}
