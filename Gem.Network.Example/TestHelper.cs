using Gem.Network.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Example
{
    /// <summary>
    /// This class has predefined servers for Gem.Network.Tests
    /// </summary>
    class TestHelper
    {
  
        internal class ClassToSerialize
        {
            public string StringProperty { get; set; }
        }

        static void ChooseTestHelper(string arg)
        {
            switch (arg)
            {
                case "SuccessfulDecodeTest":
                    MessageDecodingTestHelper();
                    break;
                case "DecodeIncomingDynamicMessageTest":
                    DynamicMessageDecodingTestHelper();
                    break;
                default:
                    Console.WriteLine("no such method found");
                    break;
            }
        }

        static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("arguments are null");
            }
            else if (args.Length == 1)
            {
                ChooseTestHelper(args[0]);
            }
            else
            {
                Console.WriteLine("insufficient numbber of arguments");
            }
        }

        /// <summary>
        /// Helper method for Gem.Network.Tests.MessageEncodingTests.SuccessfulDecodeTest()
        /// Set's up a server and sends a message for decoding
        /// This is done in a seperate project because a socket exception is thrown
        /// because client / server use the same port
        /// </summary>
        public static void MessageDecodingTestHelper()
        {
            Server server;
            MessageSerializer serializer;
            serializer = new MessageSerializer();
            server = new Server(NetDeliveryMethod.ReliableUnordered, 0);
            server.Connect("local", 14242);
            
            NetIncomingMessage inc;

            while (true)
            {

                if ((inc = server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine("Incoming LOGIN");
                            inc.SenderConnection.Approve();
                            break;

                        case NetIncomingMessageType.Data:                       
                            var outgoingmessage = server.CreateMessage();
                            var obj = new ClassToSerialize();
                            obj.StringProperty = "SomeString";
                            serializer.Encode(obj, ref outgoingmessage);
                            server.SendMessage(outgoingmessage);

                            Console.WriteLine("Sended a package with some string");
                            break;
                    }

                }
            }
        }

        public static void DynamicMessageDecodingTestHelper()
        {
            #region Dynamic Type

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

            #endregion

            Server server;
            MessageSerializer serializer;
            serializer = new MessageSerializer();
            server = new Server(NetDeliveryMethod.ReliableUnordered, 0);
            server.Connect("local", 14241);

            NetIncomingMessage inc;

            while (true)
            {

                if ((inc = server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine("Incoming LOGIN");
                            inc.SenderConnection.Approve();
                            break;

                        case NetIncomingMessageType.Data:
                            var outgoingmessage = server.CreateMessage();
                            serializer.Encode(myObject, ref outgoingmessage);
                            server.SendMessage(outgoingmessage);

                            Console.WriteLine("Sended a package with some string");
                            break;
                    }

                }
            }
                    
        }
    }
}
