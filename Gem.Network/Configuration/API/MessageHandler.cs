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

namespace Gem.Network.Configuration
{
     
    public class MessageHandler : ABuilder
    {

        public MessageHandler(ClientNetworkInfoBuilder builder)
            : base(builder)
        { }

        public IMessageHandler HandleWith<T>(T objectToHandle, Expression<Func<T, Delegate>> methodToHandle)
        {
            var unaryExpression = (UnaryExpression)methodToHandle.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var methodInfoExpression = (ConstantExpression)methodCallExpression.Arguments.Last();
            var methodInfo = (MemberInfo)methodInfoExpression.Value;

            var types = methodCallExpression.Arguments.ToList().Select(x => x.GetType()).ToArray();
            Guard.That(types.All(x => x.IsPrimitive || x == typeof(string)), "All types should be primitive");

            var properties = DynamicPropertyInfo.GetPropertyInfo(types);
            CreatePoco(properties);
            CreateEvent(builder.clientInfo.MessagePoco);

            return GetMessageHandler(properties.Select(x => x.PropertyName).ToList(), objectToHandle, methodInfo.Name);
        }

        private IMessageHandler GetMessageHandler(List<string> propertyNames, object invoker, string functionName)
        {
            var handlerType = Dependencies.Container.Resolve<IMessageHandlerFactory>()
                                         .Create(propertyNames, "handler" + profilesCalled, functionName);

            builder.clientInfo.MessageHandler = Activator.CreateInstance(handlerType, invoker) as IMessageHandler;

            builder.End();

            return builder.clientInfo.MessageHandler;
        }

        private void CreatePoco(List<DynamicPropertyInfo> properties)
        {
            var newType = Dependencies.Container.Resolve<IPocoFactory>().Create(properties, "poco" + profilesCalled);

            builder.clientInfo.MessagePoco = newType;
        }

        private void CreateEvent(Type newType)
        {
            builder.clientInfo.EventRaisingclass = Dependencies.Container.Resolve<IEventFactory>().Create(newType);

        }
    }
}




