using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Configuration;
using Moq;

namespace Gem.Network.Tests.Handlers
{
    [TestClass]
    public class MessageHandlerTests
    {
        [TestMethod]
        public void CreateMessageHandlerAndInvoke()
        {
            var tester = new Mock<HandlerTester>();
            
            //setup dependencies
            Startup.Setup();
            //setup the profile repository
            var repository = new NetworkProfileRepository();
            //setup a builder
            var networkInfoBuilder = new ClientNetworkInfoBuilder(repository);
            //create a new message handler object
            var messageHandler = new MessageHandler(networkInfoBuilder);

            var handler = messageHandler.HandleWith(tester.Object, x => new Action<string>(x.Write));

            handler.Handle("say");
            handler.Handle("say");
            handler.Handle("say");

            tester.Verify(x => x.Write("say"), Times.Exactly(3));
        }

        public interface HandlerTester
        {
            void Write(string message);
        }

        [TestMethod]
        public void CreateComplexMessageHandlerAndInvoke()
        {
            var tester = new Mock<ComplexHandlerTester>();

            //setup dependencies
            Startup.Setup();
            //setup the profile repository
            var repository = new NetworkProfileRepository();
            //setup a builder
            var networkInfoBuilder = new ClientNetworkInfoBuilder(repository);
            //create a new message handler object
            var messageHandler = new MessageHandler(networkInfoBuilder);

            var handler = messageHandler.HandleWith(tester.Object, x => new Action<float, int, long, string, int>(x.Write));

            handler.Handle(0.0f, 1, 2L, "three", 4);
            handler.Handle(0.0f, 1, 2L, "three", 4);
            handler.Handle(0.0f, 1, 2L, "three", 4);

            tester.Verify(x => x.Write(0.0f, 1, 2L, "three", 4), Times.Exactly(3));
        }

        public interface ComplexHandlerTester
        {
            void Write(float value, int number, long longnumber, string message, int anothernumber);
        }
    }
}
