using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Other;
using Lidgren.Network;
using System.Diagnostics;
using Gem.Network.Configuration;
using System.Net;
using Gem.Network.Messages;
using Moq;
using Gem.Network.Builders;
using Gem.Network.Factories;
using Gem.Network.Client;

namespace Gem.Network.Tests
{
    [TestClass]
    public class ClientEventTests
    {

        [TestMethod]
        public void SendMessageThroughDynamicPeerEventTest()
        {
            var mockClient = new Mock<INetworkPeer>();
            byte id = 1;
            IDisposable argsDisposable = null;
            //create a new dynamic type
            var propertyList = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            IPocoBuilder PocoBuilder = new CsScriptPocoBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);
            var myNewObject = Activator.CreateInstance(myNewType);

            //create a dynamic event raising class
            var EventFactory = new ClientEventFactory();
            var eventRaisingclass = EventFactory.Create(myNewType, argsDisposable, id);

            // eventRaisingclass.Event += peerEvent;
            eventRaisingclass.SubscribeEvent(mockClient.Object);

            string message = "hello";
            eventRaisingclass.Send(message);

            //verify that the message was sent
            //mockClient.Verify(c => c.SendMessage(message, id), Times.Once);
        }

    }
}
