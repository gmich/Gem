using Gem.Engine.MessageBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gem.Engine.Tests
{
    [TestClass]
    public class MessageBusTests
    {
        private IMessageBus Bus { get; } = new LightMessageBus();

        private class ConsumeFirstTestMessage
        {  }

        private class ConsumeSecondTestMessage
        { }

        [TestMethod]
        public void MessageBus_ConsumesMessage_Successfuly()
        {
            int handlerInvokations = 0;

            Bus.Consume<ConsumeFirstTestMessage>(msg => handlerInvokations++);
            Bus.Publish(new ConsumeFirstTestMessage()).RunSynchronously();

            Assert.AreEqual(1, handlerInvokations);
        }

        [TestMethod]
        public void MessageBus_MultipleConsumers_ConsumeSuccessfuly()
        {
            int firstHandler = 0;
            int secondHandler = 0;
            int thirdHandler = 0;

            Bus.Consume<ConsumeFirstTestMessage>(msg => firstHandler++);
            Bus.Consume<ConsumeFirstTestMessage>(msg => secondHandler++);
            Bus.Consume<ConsumeFirstTestMessage>(msg => thirdHandler++);

            Bus.Publish(new ConsumeFirstTestMessage()).RunSynchronously();

            Assert.AreEqual(1, firstHandler);
            Assert.AreEqual(1, secondHandler);
            Assert.AreEqual(1, thirdHandler);
        }

        [TestMethod]
        public void MessageBus_Consumer_IsDisposed_Successfuly()
        {
            int firstHandler = 0;
            int secondHandler = 0;
            int thirdHandler = 0;
            Bus.Consume<ConsumeFirstTestMessage>(msg => firstHandler++);
            Bus.Consume<ConsumeFirstTestMessage>(msg => secondHandler++).Dispose();
            Bus.Consume<ConsumeFirstTestMessage>(msg => thirdHandler++);

            Bus.Publish(new ConsumeFirstTestMessage()).RunSynchronously();

            Assert.AreEqual(1, firstHandler);
            Assert.AreEqual(0, secondHandler);
            Assert.AreEqual(1, thirdHandler);
        }

        [TestMethod]
        public void MessageBus_Correct_Consumer_IsInvoked_Successfuly()
        {
            int firstHandler = 0;
            int secondHandler = 0;
            Bus.Consume<ConsumeFirstTestMessage>(msg => firstHandler++);
            Bus.Consume<ConsumeSecondTestMessage>(msg => secondHandler++);

            Bus.Publish(new ConsumeFirstTestMessage()).RunSynchronously();
            Bus.Publish(new ConsumeSecondTestMessage()).RunSynchronously();
            Bus.Publish(new ConsumeSecondTestMessage()).RunSynchronously();

            Assert.AreEqual(1, firstHandler);
            Assert.AreEqual(2, secondHandler);
        }
    }
}
