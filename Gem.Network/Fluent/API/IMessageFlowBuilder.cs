using Gem.Network.Events;
using Gem.Network.Server;
using System;
using System.Linq.Expressions;

namespace Gem.Network.Fluent
{

    /// <summary>
    /// Configures events that send and handle messages
    /// </summary>
    public interface IMessageFlowBuilder
    {
        /// <summary>
        /// Creates an event that raises an event that sends primitive types via a server to the other clients.
        /// When a message is sent and is received the specified Target object's methodToHandle is invoked 
        /// with parameters the message's properties
        /// </summary>
        /// <typeparam name="Target">The type of the object that's being handled when the message is received</typeparam>
        /// <param name="objectToHandle">The object that's being handled when the message is received</param>
        /// <param name="methodToHandle">The method that's being invoked when the message is received. The method's signature must
        /// match the Delegate's signature</param>
        /// <returns>A network event that sends messages with argument's the Delagete's parameters</returns>
        INetworkEvent AndHandleWith<Target>(Target objectToHandle, Expression<Func<Target, Delegate>> methodToHandle);
    }

    /// <summary>
    /// Creates events and handlers that are associated with the NetworkPackage attribute
    /// </summary>
    /// <typeparam name="Target">The object's type that's annotated by the NetworkPackage attribute</typeparam>
    public interface IClientProtocolMessageBuilder<Target>
        where Target : new()
    {
        /// <summary>
        /// Invokes an action when a message that's created by GenerateSendEvent() is received
        /// </summary>
        /// <param name="action">The action to invoke</param>
        /// <returns>The same object for chaining</returns>
        IClientProtocolMessageBuilder<Target> HandleIncoming(Action<Target> action);

        /// <summary>
        /// Creates an event that sends messages of type Target
        /// </summary>
        /// <returns>The object that raises events</returns>
        INetworkEvent GenerateSendEvent();
    }

    /// <summary>
    /// Creates events and handlers that are associated with the NetworkPackage attribute.
    /// </summary>
    /// <typeparam name="Target">The object's type that's annotated by the NetworkPackage attribute</typeparam>
    public interface IServerProtocolMessageBuilder<Target>
        where Target : new()
    {
        /// <summary>
        /// Invokes an action when a message that's created by GenerateSendEvent() is received
        /// </summary>
        /// <param name="action">The action to invoke</param>
        /// <returns>The same object for chaining</returns>
        IServerProtocolMessageBuilder<Target> HandleIncoming(Action<Target> action);

        /// <summary>
        /// Creates an event that sends messages of type Target
        /// </summary>
        /// <returns>The object that raises events</returns>
        IProtocolServerEvent GenerateSendEvent();
    }

}
