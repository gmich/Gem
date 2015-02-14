using Gem.Network.Handlers;
using System;
using System.Collections.Generic;

namespace Gem.Network.Builders
{
    public interface IMessageHandlerBuilder
    {
        IMessageHandler Build(List<string> propertyNames, string classname, string functionName);
    }
}
