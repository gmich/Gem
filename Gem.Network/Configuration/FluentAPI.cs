using Gem.Network.Builders;
using Gem.Network.Handlers;
using Gem.Network.Messages;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public abstract class ABuilder
    {
        protected static int profilesCalled = 0;
        protected readonly ClientNetworkInfoBuilder builder;

        public ABuilder(ClientNetworkInfoBuilder builder)
        {
            this.builder = builder;
        }
    }

    public class Profile : ABuilder
    {

        public Profile(ClientNetworkInfoBuilder builder) : base(builder) { }

        public MessageType ForProfile(string profileName)
        {
            profilesCalled++;
            builder.ProfileName = profileName;
            return new MessageType(this.builder);
        }
    }

    public class MessageType : ABuilder
    {
        public MessageType(ClientNetworkInfoBuilder builder) : base(builder) { }

        public MessageType ForProfile(IncomingMessageTypes messageType)
        {
            builder.networkInfo.MessageType = messageType;
            return new MessageType(this.builder);
        }
    }

    public class EventCreator : ABuilder
    {
        private readonly List<DynamicPropertyInfo> propertyInfo;

        public EventCreator(ClientNetworkInfoBuilder builder)
            : base(builder)
        {
            this.propertyInfo = new List<DynamicPropertyInfo>();
        }


        public MessageHandler CreateEvent(params Type[] args)
        {
            Guard.That(args.All(x => x.IsPrimitive || x == typeof(string)), "All types should be primitive");

            var properties = DynamicPropertyInfo.GetPropertyInfo(args);
            var newType = builder.pocoFactory.Create(properties, "poco" + profilesCalled);

            builder.networkInfo.MessagePoco = newType;
            builder.networkInfo.EventRaisingclass = builder.eventFactory.Create(newType);

            return new MessageHandler(builder, properties.Select(x => x.PropertyName).ToList());
        }
    }

    public class MessageHandler : ABuilder
    {
        private readonly List<string> propertyNames;

        public MessageHandler(ClientNetworkInfoBuilder builder, List<string> propertyNames)
            : base(builder)
        {
            this.propertyNames = propertyNames;
        }

        public IMessageHandler HandleWidth(object invoker, string functionName)
        {
            var handlerType = builder.handlerFactory.Create(propertyNames, "handler" + profilesCalled, functionName);

            builder.networkInfo.MessageHandler = Activator.CreateInstance(handlerType, invoker) as IMessageHandler;

            builder.End();

            return builder.networkInfo.MessageHandler;
        }
    }
}



