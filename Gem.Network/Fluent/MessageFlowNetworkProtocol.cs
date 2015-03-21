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

     /// <summary>
     /// Creates a network event that calculates the delta time from sending to receiving
     /// </summary>
    public class MessageFlowNetworkProtocol<T> : IMessageFlowBuilder
        where T : INetworkProtocol
    {
        
        #region Fields

        private readonly string profile;
        
        private readonly MessageType messageType;

        private MessageFlowArguments messageFlowArgs;

        private readonly byte Id; 
        #endregion


        #region Constructor

        public MessageFlowNetworkProtocol(string profile, MessageType messageType)
        {
            this.profile = profile;
            this.Id = GemNetwork.GetMesssageId(profile);
            this.messageType = messageType;
            this.messageFlowArgs = new MessageFlowArguments();
        }


        #endregion


        #region IMessageFlowBuilder Implementation

        public INetworkEvent AndHandleWith<Target>(Target objectToHandle, Expression<Func<Target, Delegate>> methodToHandle)
        {
            var methodInfo = methodToHandle.GetMethodInfo();
            var protocolType = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();

            var types = protocolType.GetPropertyTypes();

            Guard.That(types.All(x => x.IsPrimitive || x == typeof(string)), "All types should be primitive");
            
            var properties = DynamicPropertyInfo.GetPropertyInfo(types.ToArray());
            
            SetDynamicPoco(properties);

            SetMessageHandler(properties.Select(x => DynamicPropertyInfo.GetPrimitiveTypeAlias(x.PropertyType)).ToList(), objectToHandle, methodInfo.Name);
            var argumentsDisposable = GemClient.MessageFlow[profile,messageType].Add(messageFlowArgs);
            SetDynamicEvent(argumentsDisposable);

            messageFlowArgs.IncludesLocalTime = true;
            GemClient.MessageFlow[profile, messageType].SubscribeEvent(messageFlowArgs.ID);

            return messageFlowArgs.EventRaisingclass;
        }

        #endregion


        #region MessageFlow Setup

        private void SetMessageHandler(List<string> propertyNames, object invoker, string functionName)
        {
            var handlerType = Dependencies.Container.Resolve<IMessageHandlerFactory>()
                                          .Create(propertyNames, "handler" + Id, functionName);

            messageFlowArgs.MessageHandler = Activator.CreateInstance(handlerType, invoker) as IMessageHandler;
        }

        private void SetDynamicPoco(List<DynamicPropertyInfo> properties)
        {
            var newType = Dependencies.Container.Resolve<IPocoFactory>().Create(properties, "poco" + Id);

            messageFlowArgs.MessagePoco = newType;
        }

        private void SetDynamicEvent(IDisposable argumentDisposable)
        {
            messageFlowArgs.EventRaisingclass = new TimeDeltaEventFactory().Create(messageFlowArgs.MessagePoco, argumentDisposable, messageFlowArgs.ID);
        }

        #endregion

    }
}




