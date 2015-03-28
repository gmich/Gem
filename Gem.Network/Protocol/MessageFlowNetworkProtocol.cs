using Gem.Network.Handlers;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Seterlund.CodeGuard;
using Gem.Network.Builders;
using Autofac;
using System.Collections.Generic;
using Gem.Network.Factories;
using Gem.Network.Events;
using Gem.Network.Messages;
using Gem.Network.Client;

namespace Gem.Network.Fluent
{
    using Extensions;
    using Gem.Network.Protocol;

    public class MessageFlowNetworkProtocol<Target> : IClientProtocolMessageBuilder<Target>
        where Target: new()
    {

        #region Fields

        private readonly string profile;

        private readonly MessageType messageType;

        private readonly byte id;
        #endregion


        #region Constructor

        public MessageFlowNetworkProtocol(string profile, MessageType messageType)
        {
            this.profile = profile;
            this.messageType = messageType;
            id = ProtocolManager.Provider[profile].GetAll()
                                    .Where(x => x.Type == typeof(Target))
                                    .Select(x => x.Attribute.Id).Single();
        }

        #endregion


        #region IMessageFlowBuilder Implementation

        public INetworkEvent GenerateSendEvent()
        {
            var networkArgs = GemClient.MessageFlow[profile, MessageType.Data, id];

            networkArgs.EventRaisingclass = new ProtocolEventFactory().Create(networkArgs.MessagePoco, null, networkArgs.ID);
            //Subscribe the on send event
            GemClient.MessageFlow[profile, messageType].SubscribeEvent(networkArgs.ID);

            return networkArgs.EventRaisingclass;
        }

        public IClientProtocolMessageBuilder<Target> HandleIncoming(Action<Target> action)
        {
            var networkArgs = GemClient.MessageFlow[profile, MessageType.Data, id];

            var handlerType = new ProtocolHandlerBuilder().Build<Target>(new Target().GetType().Assembly.GetName().Name,"handler" + id, typeof(Target).FullName);

            networkArgs.MessageHandler = Activator.CreateInstance(handlerType, action) as IMessageHandler;

            return this;
        }

        #endregion


    }
}




