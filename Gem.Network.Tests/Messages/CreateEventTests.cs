using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Other;
using Gem.Network.Configuration;
using Gem.Network.Messages;
using Moq;

namespace Gem.Network.Tests
{
    [TestClass]
    public class CreateEventTests
    {

        [TestMethod]
        public void DynamicEventInvocationTest()
        {
            var tester = new Mock<EventTester>();

            var messageHandler = NetworkConfig.ForTag("tag")
                .CreateEvent(typeof(string))
                .HandleWith(tester.Object, "AppendSomething");

            messageHandler.HandleMessage("say");

            tester.Verify(x => x.AppendSomething("say"), Times.Once);

            messageHandler.HandleMessage("say twice");
            messageHandler.HandleMessage("say twice");
   
            tester.Verify(x => x.AppendSomething("say twice"), Times.Exactly(2));
        }

        [TestMethod]
        public void ComplexDynamicEventInvocationTest()
        {
            var tester = new Mock<EventTester>();
            var messageHandler = NetworkConfig.ForTag("tag")
                .CreateEvent(typeof(string), typeof(int), typeof(int), typeof(string))
                .HandleWith(tester.Object, "DoSomethingComplicated");

            messageHandler.HandleMessage("say", 1, 2, "error");

            //TODO: handle floats
            tester.Verify(x => x.DoSomethingComplicated("say", 1, 2, "error"), Times.Once);

        }

        public interface EventTester
        {
            void AppendSomething(string message);
            void DoSomethingComplicated(string msg, int count, int value, string error);
        }
    }
}
