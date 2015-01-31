using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Messages;
using Lidgren.Network;

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
        private MessageSerializer serializer;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            serializer = new MessageSerializer();
            server = new Server(NetDeliveryMethod.ReliableUnordered, 0);
            server.Connect("localhost", 80);
        }
        
        [TestCleanup()]
        public void MyTestCleanup()
        {
            server.Dispose();
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

            //TODO:
            //Server.SendMessage
            //Client.GetMessage
            //Client.ReadMessage using the serializer
        }

    }
}
