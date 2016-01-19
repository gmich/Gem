using Gem.Infrastructure.Assertions;
using System;
using System.Collections.Generic;

namespace Gem.Engine.MessageBus
{
    /// <summary>
    /// A lightweight simple Publish - Consume message bus
    /// </summary>
    public class LightMessageBus : IMessageBus
    {
        //Stores consumers with their consuming message type
        private readonly Dictionary<Type, Action<object>> consumers = new Dictionary<Type, Action<object>>();

        ///<inheritdoc />
        public IDisposable Consume<TMessage>(Action<TMessage> handler)
            where TMessage : class
        {
            DebugArgument.Require.NotNull(() => handler);

            var messageType = typeof(TMessage);
            Action<object> consumer = msg => handler(msg as TMessage);

            if (consumers.ContainsKey(messageType))
            {
                consumers[messageType] += consumer;
            }
            else
            {
                consumers.Add(messageType, consumer);
            }
            return new ConsumerDisposable(() => consumers[messageType] -= consumer);
        }

        ///<inheritdoc />
        public bool Publish<TMessage>(TMessage message)
            where TMessage : class
        {
            DebugArgument.Require.NotNull(() => message);

            var messageType = typeof(TMessage);
            if (consumers.ContainsKey(messageType))
            {
                consumers[messageType].Invoke(message);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Used to remove consumer handlers from the consumer dictionary
        /// </summary>
        private class ConsumerDisposable : IDisposable
        {
            private Action disposableAction;

            public ConsumerDisposable(Action disposableAction)
            {
                this.disposableAction = disposableAction;
            }
            public void Dispose()
            {
                disposableAction?.Invoke();
                disposableAction = null;
            }
        }
    }
}
