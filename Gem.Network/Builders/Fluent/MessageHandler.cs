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

namespace Gem.Network.Configuration
{
     
    public class ClientInfoBuilder : ABuilder
    {

        public ClientInfoBuilder(ClientNetworkInfoBuilder builder)
            : base(builder)
        { }

        public INetworkEvent HandleWith<T>(T objectToHandle, Expression<Func<T, Delegate>> methodToHandle)
        {
            var lambdaExpression = (LambdaExpression)methodToHandle;
            var unaryExpression = (UnaryExpression)lambdaExpression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var methodInfoExpression = (ConstantExpression)methodCallExpression.Object;
            var methodInfo = (MethodInfo)methodInfoExpression.Value;

            var types = methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();

            Guard.That(types.All(x => x.IsPrimitive || x == typeof(string)), "All types should be primitive");

            var properties = DynamicPropertyInfo.GetPropertyInfo(types);
            CreatePoco(properties);

            builder.clientInfo.MessageHandler= GetMessageHandler(properties.Select(x => DynamicPropertyInfo.GetPrimitiveTypeAlias(x.PropertyType)).ToList(), objectToHandle, methodInfo.Name);

            return builder.End();
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
        
    }
}




