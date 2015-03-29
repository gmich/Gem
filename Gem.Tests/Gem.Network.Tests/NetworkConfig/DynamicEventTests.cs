using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Gem.Network.Client;

namespace Gem.Network.Tests
{
    [TestClass]
    public class DynamicEventTests
    {

        [TestMethod]
        public void DynamicEventInvocationTest()
        {
            var tester = new Mock<EventTester>();

            var messageHandler = GemClient.Profile("tag")
                .CreateNetworkEvent
                .AndHandleWith(tester.Object, x => new Action<string>(x.AppendSomething));
            
            //Start a server to verify this
            //messageHandler.Send("say");
            //tester.Verify(x => x.AppendSomething("say"), Times.Once);
            
            //Start a server to verify this
            //messageHandler.Send("say twice");
            //messageHandler.Send("say twice");
            // tester.Verify(x => x.AppendSomething("say twice"), Times.Exactly(2));
        }

        [TestMethod]
        public void ComplexDynamicEventInvocationTest()
        {
            var client = new GemClient("tag", new ConnectionConfig(),PackageConfig.TCP);
            var tester = new Mock<EventTester>();
            var messageHandler = GemClient.Profile("tag")
                 .CreateNetworkEvent
                 .AndHandleWith(tester.Object, x => new Action<string, int, int, string>(x.DoSomethingComplicated));
            
            //Start a server to verify this
            //messageHandler.Send("say", 1, 2, "error");
            // tester.Verify(x => x.DoSomethingComplicated("say", 1, 2, "error"), Times.Once);
        }

        public interface EventTester
        {
            void AppendSomething(string message);
            void DoSomethingComplicated(string msg, int count, int value, string error);
        }
    }
}
