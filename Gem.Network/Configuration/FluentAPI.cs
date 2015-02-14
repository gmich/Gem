using Gem.Network.Builders;
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
            builder.ProfileName = profileName;
            return new MessageType(this.builder);
        }
    }

    public class MessageType : ABuilder
    {
        public MessageType(ClientNetworkInfoBuilder builder) : base(builder) { }

        public MessageType ForProfile(IncomingMessageTypes messageType)
        {
            builder.NetworkInfo.MessageType = messageType;
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


        public MessageType CreateEvent(params Type[] args)
        {
            Guard.That(args.All(x => x.IsPrimitive || x == typeof(string)), "All types should be primitive");

            foreach (var arg in args)
            {
                propertyInfo.Add(new DynamicPropertyInfo
                {
                    PropertyName = (Tag + (++PropertyCount)),
                    PropertyType = arg
                });
            }
        }
    }
}



