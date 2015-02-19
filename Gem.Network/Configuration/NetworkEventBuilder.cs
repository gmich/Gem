using Gem.Network.Builders;
using Gem.Network.Handlers;
using Gem.Network.Messages;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Gem.Network.Factories;
using System.Reflection;
using System.Linq.Expressions;

namespace Gem.Network.Configuration
{

    public class NetworkEventBuilder
    {
        private NetworkProfileRepository profileRepository;

        public MessageType this[string index]
        {
            get
            {
                profileRepository.Add(index,null);
                return new MessageType(new ClientNetworkInfoBuilder(profileRepository));
            }
        }
    }

    public abstract class ABuilder
    {
        protected static int profilesCalled = 0;
        protected readonly ClientNetworkInfoBuilder builder;

        public ABuilder(ClientNetworkInfoBuilder builder)
        {
            this.builder = builder;
        }
    }

    public class MessageType : ABuilder
    {
        public MessageType(ClientNetworkInfoBuilder builder) : base(builder) { }

        public MessageType Send(IncomingMessageTypes messageType)
        {
            profilesCalled++;
            builder.clientInfo.MessageType = messageType;
            return new MessageType(this.builder);
        }
    }

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
            MessageHandler a = null;

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




