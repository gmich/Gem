//using System;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Lidgren.Network;
//using Gem.Network.Messages;
//using System.Diagnostics;
//using System.Net;
//using Gem.Network.Builders;

//namespace Gem.Network.Tests.Messages
//{
//    [TestClass]
//    public class DynamicTypeEncodingTests
//    {

//        [TestMethod]
//        public void EncodeDynamicMessageTest()
//        {
//            var propertyList = new List<DynamicPropertyInfo>
//            {
//                new DynamicPropertyInfo{
//                        PropertyName = "Name",
//                        PropertyType = typeof(string)
//                }
//            };
//            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
//            Type myNewType = PocoBuilder.Build("POCO", propertyList);

//            dynamic myObject = Activator.CreateInstance(myNewType);

//            myObject.Name = "DynamicType";

//            var server = new NetworkServer(5);
//            server.Connect(new ServerConfig { Name = "local", Port = 14241 });

//            var outgoingmessage = server.CreateMessage();

//            Assert.IsTrue(outgoingmessage.LengthBits == 0);

//            MessageSerializer.Encode(myObject, ref outgoingmessage);

//            Assert.IsTrue(outgoingmessage.LengthBits > 0);

//            server.Dispose();
//        }

//        [TestMethod]
//        public void DecodeIncomingDynamicMessageTest()
//        {
//            Process server = new Process();
//            try
//            {
//                server.StartInfo.FileName = @"C:\Users\George\Documents\GitHub\Gem\Gem.Network.Example\bin\Debug\Gem.Network.Example.exe";
//                server.StartInfo.Arguments = "DecodeIncomingDynamicMessageTest";
//                server.Start();
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail("Failed to launch the server. Reason:" + ex.Message);
//            }

//            //the dynamic type
//            var propertyList = new List<DynamicPropertyInfo>
//            {
//                new DynamicPropertyInfo{
//                        PropertyName = "Name",
//                        PropertyType = typeof(string)
//                }
//            };
//            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
//            Type myNewType = PocoBuilder.Build("POCO", propertyList);

//            var client = new Client();
//            client.Connect(new ConnectionDetails
//            {
//                ServerIP = new IPEndPoint(NetUtility.Resolve("127.0.0.1"), 14241),
//                ServerName = "local"
//            });

//            NetIncomingMessage msg;
//            Stopwatch stopwatch = new Stopwatch();
//            stopwatch.Start();

//            //wait 7 sec
//            while (stopwatch.ElapsedMilliseconds < 7000)
//            {
//                if ((msg = client.ReadMessage()) != null)
//                {
//                    switch (msg.MessageType)
//                    {
//                        case NetIncomingMessageType.StatusChanged:
//                            var om = client.CreateMessage();
//                            client.SendMessage(om);
//                            break;
//                        default:
//                            dynamic readableMessageWithType = MessageSerializer.Decode(msg, myNewType);
//                            Assert.AreEqual(readableMessageWithType.Name, "DynamicType");

//                            //Decoding dynamic types with generics doesn't work
//                            //dynamic readableMessageWithGeneric = serializer.Decode<dynamic>(msg);       
//                            //Assert.AreEqual(readableMessageWithGeneric.Name, "DynamicType");

//                            server.CloseMainWindow();
//                            server.Close();
//                            return;
//                    }
//                }
//            }

//            Assert.Fail("Failed to get the incoming message");
//            server.CloseMainWindow();
//            server.Close();
//        }

//    }
//}

