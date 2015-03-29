using Gem.Network.Builders;
using Gem.Network.Client;
using Gem.Network.Factories;
using Gem.Network.Handlers;
using Gem.Network.Messages;
using Gem.Network.Utilities;
using Gem.Network.Server;
using System;
using System.Linq;
using Autofac;

namespace Gem.Network.Protocol
{
    using Extensions;
    using Gem.Network.Managers;

    /// <summary>
    /// Upon initialization, protocol manager scans the assemblies and finds all the classes that
    /// are annotated with NetworkPackageAttribute, gets their properties and registers their types as 
    /// messageflow POCOs
    /// </summary>
    public static class ProtocolResolver
    {

        #region Fields

        private readonly static ProtocolManager protocolProvider;
        private static int ProtocolObjectsFound = 0;

        #endregion

        #region Ctor

        static ProtocolResolver()
        {
            protocolProvider = new ProtocolManager();

            //find NetworkPackages and cache their types
            AttributeResolver.DoWithAllTypesWithAttribute<NetworkPackageAttribute>(
                              (type, attribute) =>
                              {
                                  byte id = CreateMessageFlowArguments(GemNetwork.GetMesssageId(attribute.Profile), type, attribute.Profile);
                                  attribute.Id = id;
                                  protocolProvider[attribute.Profile].Add(id, new TypeAndAttribute { Type = type, Attribute = attribute } );
                              });
        }

        #endregion

        #region Private Helpers

        private static byte CreateMessageFlowArguments(byte id, Type type, string profile)
        {
            var properties = RuntimePropertyInfo.GetPropertyInfo(Activator.CreateInstance(type).GetPropertyTypes().ToArray());
            var messageFlowArgs = new MessageFlowArguments();
            messageFlowArgs.MessageHandler = new DummyHandler();
            messageFlowArgs.MessagePoco = Dependencies.Container.Resolve<IPocoFactory>().Create(properties, "poco" + id);
            messageFlowArgs.ID = (byte)(GemNetwork.InitialId + ProtocolObjectsFound++);

            GemClient.MessageFlow[profile, MessageType.Data].Add(messageFlowArgs);
            GemServer.MessageFlow[profile, MessageType.Data].Add(new MessageArguments
            {
                ID = messageFlowArgs.ID,
                MessageHandler = new DummyHandler(),
                MessagePoco = messageFlowArgs.MessagePoco
            });

            return messageFlowArgs.ID;
        }

        #endregion

        /// <summary>
        /// Stores the NetworkPackageAttribute annotated objects
        /// </summary>
        public static ProtocolManager Provider
        {
            get
            {
                return protocolProvider;
            }
        }

    }
}