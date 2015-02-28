using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Messages;
using Lidgren.Network;
using System.Net;
using System.Diagnostics;

namespace Gem.Network.Tests
{
    /// <summary>
    /// Summary description for MessageEncodingTests
    /// </summary>
    [TestClass]
    public class MessageEncodingTests
    {
          
        internal class ClassToSerialize
        {
            public string StringProperty { get; set; }
        }

        [TestMethod]
        public void SuccessfulEncodeTest()
        {
            var server = new NetworkServer(5);
            server.Connect(new ServerConfig { Name = "local", Port = 14241 });

            var outgoingmessage = server.CreateMessage();
            var obj = new ClassToSerialize();

            obj.StringProperty = "SomeString";


            Assert.IsTrue(outgoingmessage.LengthBits == 0);

            MessageSerializer.Encode(obj, ref outgoingmessage);

            Assert.IsTrue(outgoingmessage.LengthBits > 0);
            server.Dispose();
        }

        /// <summary>
        /// This test requires to run the the Gem.Network.Example.MessageEncodingTestHelper() to send the message
        /// </summary>
        [TestMethod]
        public void SuccessfulDecodeTest()
        {
            Process server = new Process();
            try
            {
                server.StartInfo.FileName = @"C:\Users\George\Documents\GitHub\Gem\Gem.Network.Example\bin\Debug\Gem.Network.Example.exe";
                server.StartInfo.Arguments = "SuccessfulDecodeTest";
                server.Start();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to launch the server. Reason:" + ex.Message);
            }

            var client = new Client();
            client.Connect(new ConnectionDetails
            {
                ServerIP = new IPEndPoint(NetUtility.Resolve("127.0.0.1"), 14241),
                ServerName = "local"
            });

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
                            var readableMessage = MessageSerializer.Decode<ClassToSerialize>(msg);
                            Assert.AreEqual(readableMessage.StringProperty, "SomeString");

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
