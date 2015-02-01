using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Messages;
using Lidgren.Network;
using System.Net;

namespace Gem.Network.Tests
{
    /// <summary>
    /// Summary description for MessageEncodingTests
    /// </summary>
    [TestClass]
    public class MessageEncodingTests
    {

        #region Initialization

        private Server server;
        private Client client;
        private MessageSerializer serializer;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            serializer = new MessageSerializer();
            server = new Server(NetDeliveryMethod.ReliableUnordered, 0);
            server.Connect("localhost", 14242);

            //client = new Client(new IPEndPoint(NetUtility.Resolve("127.0.0.1"), 14242));
            //client.Connect("localhost", 14242);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            server.Dispose();
            //client.Dispose();
        }

        #endregion

        internal class ClassToSerialize
        {
            public string StringProperty { get; set; }
        }

        [TestMethod]
        public void SuccessfulSerializationTest()
        {
            var outgoingmessage = server.CreateMessage();
            var obj = new ClassToSerialize();

            obj.StringProperty = "SomeString";
            serializer.Encode(obj, ref outgoingmessage);

        }

    }
}
