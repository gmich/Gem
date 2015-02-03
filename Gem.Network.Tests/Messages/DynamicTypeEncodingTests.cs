using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lidgren.Network;
using Gem.Network.Messages;
using System.Diagnostics;
using System.Net;

namespace Gem.Network.Tests.Messages
{
    [TestClass]
    public class DynamicTypeEncodingTests
    {

        [TestMethod]
        public void EncodeDynamicMessageTest()
        {
            var propertyList = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };

            Type myNewType = ClassBuilder.CreateNewObject("POCO", propertyList);

            dynamic myObject = Activator.CreateInstance(myNewType);

            myObject.Name = "DynamicType";

            var server = new Server(NetDeliveryMethod.ReliableUnordered, 0);
            server.Connect("local", 14242);

            var outgoingmessage = server.CreateMessage();

            Assert.IsTrue(outgoingmessage.LengthBits == 0);

            MessageSerializer.Encode(myObject, ref outgoingmessage);

            Assert.IsTrue(outgoingmessage.LengthBits > 0);

            server.Dispose();
        }

        [TestMethod]
        public void DecodeIncomingDynamicMessageTest()
        {
            Process server = new Process();
            try
            {
                server.StartInfo.FileName = @"C:\Users\George\Documents\GitHub\Gem\Gem.Network.Example\bin\Debug\Gem.Network.Example.exe";
                server.StartInfo.Arguments = "DecodeIncomingDynamicMessageTest";
                server.Start();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to launch the server. Reason:" + ex.Message);
            }

            //the dynamic type
            var propertyList = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            Type myNewType = ClassBuilder.CreateNewObject("POCO", propertyList);

            var client = new Client(new IPEndPoint(NetUtility.Resolve("127.0.0.1"), 14241), "local");

            client.Connect("local", 14241);

            NetIncomingMessage msg;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //wait 7 sec
            while (stopwatch.ElapsedMilliseconds < 7000)
            {
                if ((msg = client.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            var om = client.CreateMessage();
                            client.SendMessage(om);
                            break;
                        default:
                            dynamic readableMessageWithType = MessageSerializer.Decode(msg, myNewType);
                            Assert.AreEqual(readableMessageWithType.Name, "DynamicType");

                            //Decoding dynamic types with generics doesn't work
                            //dynamic readableMessageWithGeneric = serializer.Decode<dynamic>(msg);       
                            //Assert.AreEqual(readableMessageWithGeneric.Name, "DynamicType");

                            server.CloseMainWindow();
                            server.Close();
                            return;
                    }
                }
            }

            Assert.Fail("Failed to get the incoming message");
            server.CloseMainWindow();
            server.Close();
        }

    }
}

