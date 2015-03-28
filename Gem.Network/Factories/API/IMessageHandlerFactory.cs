using System;
using System.Collections.Generic;

namespace Gem.Network.Factories
{
   public  interface IMessageHandlerFactory
    {
       /// <summary>
       /// Creates a type that's aligned to IMessageHandler and is later used 
       /// via activator to create instances
       /// </summary>
       /// <param name="propertyTypeNames">The type's properties</param>
       /// <param name="classname">The type's name</param>
       /// <param name="functionName">The function the IMessageHandler invokes</param>
       /// <returns>A type that's aligned to IMessageHandler</returns>
       Type Create(List<string> propertyTypeNames, string classname, string functionName);
    }
}
