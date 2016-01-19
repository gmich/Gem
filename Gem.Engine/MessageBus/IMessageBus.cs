using System;

namespace Gem.Engine.MessageBus
{
    /// <summary>
    /// A Publish - Consume message bus
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Subscribes a consumer for a message type to the message bus
        /// </summary>
        /// <typeparam name="TMessage">The type of the consumed message</typeparam>
        /// <param name="handler">The consumer</param>
        /// <returns>A disposable object that is used to remove the consumer from the bus</returns>
        IDisposable Consume<TMessage>(Action<TMessage> handler)
            where TMessage : class;

        /// <summary>
        /// Publishes a message to the message bus
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to publish</typeparam>
        /// <param name="message">The message object</param>
        /// <returns>True if the message was consumed</returns>
        bool Publish<TMessage>(TMessage message)
            where TMessage : class;
  
    }
}
