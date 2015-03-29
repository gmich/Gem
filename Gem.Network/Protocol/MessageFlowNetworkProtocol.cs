using Gem.Network.Handlers;
using System;
using System.Linq;
using Gem.Network.Events;
using Gem.Network.Messages;
using Gem.Network.Client;
using Gem.Network.Fluent;
using Gem.Network.Factories;
using Lidgren.Network;

namespace Gem.Network.Protocol
{
    /// <summary>
    /// Creates events and handlers that are associated with the NetworkPackage attribute
    /// </summary>
    /// <typeparam name="Target">The object's type that's annotated by the NetworkPackage attribute</typeparam>
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
            id = ProtocolResolver.Provider[profile].GetAll()
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

        public IClientProtocolMessageBuilder<Target> HandleIncoming(Action<NetConnection,Target> action)
        {
            var networkArgs = GemClient.MessageFlow[profile, MessageType.Data, id];

            var handlerType = new ProtocolHandlerBuilder().Build<Target>(new Target().GetType().Assembly.GetName().Name,"handler" + id, typeof(Target).FullName);

            networkArgs.MessageHandler = Activator.CreateInstance(handlerType, action) as IMessageHandler;

            return this;
        }

        #endregion

    }
}




