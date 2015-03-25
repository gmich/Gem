using Gem.Network.Handlers;
using System;
using System.Collections.Generic;

namespace Gem.Network.Builders
{
    /// <summary>
    /// Builds an runtime object of IMessageHandler and returns its type.
    /// Instantiate it with reference of the object to handle.
    /// </summary>
    public interface IMessageHandlerBuilder
    {
        /// <summary>
        /// Builds an runtime object of IMessageHandler and returns its type.
        /// When a network message is received and the parsed to an object, use its properties 
        /// to invoke the Handle(params object[] args) of IMessageHandler to handle the incoming message
        /// </summary>
        /// <param name="propertyTypes">The types that are passed to the function</param>
        /// <param name="classname">The dynamic's class name</param>
        /// <param name="functionName">The function of the object thats being handled</param>
        /// <returns>A runtime type</returns>
        Type Build(List<string> propertyTypes, string classname, string functionName);
    }
}
