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

namespace Gem.Network.Fluent
{
     
    public class MessageFlowBuilder : IMessageFlowBuilder
    {

        #region Fields

        private readonly string profile;
        
        private readonly ClientMessageType messageType;
        
        private MessageFlowArguments messageFlowArgs;

        #endregion


        #region Constructor
        
        public MessageFlowBuilder(string profile,ClientMessageType messageType)
        {
            this.profile = profile;
            this.messageType = messageType;
            this.messageFlowArgs = new MessageFlowArguments();
        }

        public MessageFlowBuilder(string profile, ClientMessageType messageType,object[] cachedMessage):this(profile,messageType)
        {
            this.messageFlowArgs.CachedMessage = cachedMessage;
        }

        #endregion


        #region IMessageFlowBuilder Implementation

        public INetworkEvent AndHandleWith<T>(T objectToHandle, Expression<Func<T, Delegate>> methodToHandle)
        {
            var lambdaExpression = (LambdaExpression)methodToHandle;
            var unaryExpression = (UnaryExpression)lambdaExpression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var methodInfoExpression = (ConstantExpression)methodCallExpression.Object;
            var methodInfo = (MethodInfo)methodInfoExpression.Value;

            var types = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();

            Guard.That(types.All(x => x.IsPrimitive || x == typeof(string)), "All types should be primitive");

            var properties = DynamicPropertyInfo.GetPropertyInfo(types.ToArray());
            
            SetDynamicPoco(properties);
            SetMessageHandler(properties.Select(x => DynamicPropertyInfo.GetPrimitiveTypeAlias(x.PropertyType)).ToList(), objectToHandle, methodInfo.Name);
            var argumentsDisposable = GemNetwork.ClientMessageFlow[profile,messageType].Add(messageFlowArgs);
            SetDynamicEvent(argumentsDisposable);

            GemNetwork.ClientMessageFlow[profile, messageType].SubscribeEvent(messageFlowArgs.ID);

            return messageFlowArgs.EventRaisingclass;
        }

        #endregion


        #region MessageFlow Setup

        private void SetMessageHandler(List<string> propertyNames, object invoker, string functionName)
        {
            var handlerType = Dependencies.Container.Resolve<IMessageHandlerFactory>()
                                          .Create(propertyNames, "handler" + GemNetwork.profilesInvoked, functionName);

            messageFlowArgs.MessageHandler = Activator.CreateInstance(handlerType, invoker) as IMessageHandler;
        }

        private void SetDynamicPoco(List<DynamicPropertyInfo> properties)
        {
            var newType = Dependencies.Container.Resolve<IPocoFactory>().Create(properties, "poco" + GemNetwork.profilesInvoked);

            messageFlowArgs.MessagePoco = newType;
        }

        private void SetDynamicEvent(IDisposable argumentDisposable)
        {
            messageFlowArgs.EventRaisingclass = Dependencies.Container.Resolve<IEventFactory>().Create(messageFlowArgs.MessagePoco, argumentDisposable,messageFlowArgs.ID);
        }

        #endregion

    }
}




