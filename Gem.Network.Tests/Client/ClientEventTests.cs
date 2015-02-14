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

namespace Gem.Network.Tests
{
    [TestClass]
    public class ClientEventTests
    {

        [TestMethod]
        public void SendMessageThroughDynamicPeerEventTest()
        {
            Mock<Client> mockClient = new Mock<Client>();
            var client = new Client(new IPEndPoint(NetUtility.Resolve("127.0.0.1"), 14241), "local");
            client.Connect("local", 14241);

            //create a new dynamic type
            var propertyList = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);
            var myNewObject = Activator.CreateInstance(myNewType);

            //create a dynamic event raising class
            var EventFactory = new ClientEventFactory();
            dynamic eventRaisingclass = EventFactory.Create(myNewType);

            // eventRaisingclass.Event += peerEvent;
            eventRaisingclass.SubscribeEvent(mockClient.Object);
            eventRaisingclass.OnEvent(myNewObject);

            //verify that the message was sent
             mockClient.Verify(c => c.SendMessage(myNewObject), Times.Once);
        }     
             
    }
}
