using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Utilities
{
    /// <summary>
    /// Handles incoming message according to the NetworkIncomingMessage Type
    /// </summary>
    /// <typeparam name="TMessage">The raw NetIncomingMessage</typeparam>
    /// <typeparam name="TConverted">The converted message</typeparam>
    public interface IMessageConfiguration<TMessage, TConverted>
        where TMessage : NetIncomingMessage
    {
        /// <summary>
        /// The type that gets handled
        /// </summary>
        MessageType ServerIncomingMesssageType { get; set; }

        /// <summary>
        /// The function that takes a NetIncomingMessage and converts it to a type to get handled
        /// </summary>
        Func<TMessage, TConverted> Converter { set; }

        /// <summary>
        /// Adds an action that handles an input message
        /// </summary>
        /// <param name="messageHandler">The action</param>
        /// <returns>The actions' disposable</returns>
        IDisposable AddHandler(Action<TConverted> messageHandler);

        /// <summary>
        /// The method that handles the network's incoming message
        /// </summary>
        /// <param name="message">The raw message</param>
        void Handle(TMessage message);
    }

}
