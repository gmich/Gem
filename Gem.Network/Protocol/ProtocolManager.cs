using Gem.Network.Builders;
using Gem.Network.Client;
using Gem.Network.Factories;
using Gem.Network.Handlers;
using Gem.Network.Messages;
using Gem.Network.Utilities;
using System;
using System.Linq;
using Autofac;

namespace Gem.Network.Protocol
{
    using Extensions;
    using System.Collections.Generic;

    public static class ProtocolManager
    {

        private readonly static ProtocolProviderManager protocolProvider;

        internal static void Init() { }

        static ProtocolManager()
        {
            protocolProvider = new ProtocolProviderManager();

            AttributeResolver.DoWithAllTypesWithAttribute<NetworkPackageAttribute>(
                              (type, attribute) =>
                              {
                                  byte id = CreateMessageFlowArguments(GemNetwork.GetMesssageId(attribute.Profile), type, attribute.Profile);
                                  attribute.Id = id;
                                  protocolProvider[attribute.Profile].Add(id, new TypeAndAttribute { Type = type, Attribute = attribute } );
                              });
        }

        private static byte CreateMessageFlowArguments(byte id, Type type, string profile)
        {
            var properties = DynamicPropertyInfo.GetPropertyInfo(Activator.CreateInstance(type).GetPropertyTypes().ToArray());
            var messageFlowArgs = new MessageFlowArguments();
            messageFlowArgs.MessageHandler = new DummyHandler();
            messageFlowArgs.MessagePoco = Dependencies.Container.Resolve<IPocoFactory>().Create(properties, "poco" + id);

            GemClient.MessageFlow[profile, MessageType.Data].Add(messageFlowArgs);

            return messageFlowArgs.ID;
        }
                     
        public static ProtocolProviderManager Provider
        {
            get
            {
                return protocolProvider;
            }
        }

    }
}