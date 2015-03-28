using Gem.Network.Builders;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Factories
{
    /// <summary>
    /// Creates types that are aligned to IMessageHandler, to handle incoming network messages
    /// </summary>
    public sealed class MessageHandlerFactory : IMessageHandlerFactory
    {

        #region Private Properties

        private readonly IMessageHandlerBuilder messageBuilder;
           
        #endregion

        #region Constructor

        public MessageHandlerFactory(IMessageHandlerBuilder messageBuilder)
        {
            this.messageBuilder = messageBuilder;
        }

        #endregion

        #region IMessageHandlerFactorys Implementation

        /// <summary>
        /// Creates the type that handles incoming message
        /// </summary>
        /// <param name="propertyTypeNames">The property names that are being handled </param>
        /// <param name="classname">The handler's class name</param>
        /// <param name="functionName">The function that's being handled</param>
        /// <returns>A type to be used by activator and create the IMessageHandler instance</returns>
        public Type Create(List<string> propertyTypeNames, string classname, string functionName)
        {
            Guard.That(propertyTypeNames.All(x => x != null), "The propertyTypeNames should not be null");
            Guard.That(classname).IsNotNull();
            Guard.That(functionName).IsNotNull();

            var newHandler = messageBuilder.Build(propertyTypeNames, classname, functionName);

            return newHandler;
        }
       
        #endregion        
    }
}
