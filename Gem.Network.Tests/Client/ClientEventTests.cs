using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Other;
using Lidgren.Network;
using Gem.Network.Networking;
using System.Diagnostics;
using Gem.Network.Configuration;
using System.Net;
using Gem.Network.Messages;
using Moq;

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
            var propertyList = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            Type myNewType = ClassBuilder.CreateNewObject("POCO", propertyList);
            var myNewObject = Activator.CreateInstance(myNewType);

            //create a dynamic event raising class
            dynamic eventRaisingclass = EventBuilder.BuildEventRaisingClass(myNewType);

            var serializer = new MessageSerializer();
            var msg = client.CreateMessage();
            serializer.Encode(myNewObject, ref msg);
            // eventRaisingclass.Event += peerEvent;
            eventRaisingclass.SubscribeEvent(mockClient.Object);
            eventRaisingclass.OnEvent(msg);

            //verify that the message was sent
            mockClient.Verify(c => c.SendMessage(msg), Times.Once);
        }

      
             
    }
}
