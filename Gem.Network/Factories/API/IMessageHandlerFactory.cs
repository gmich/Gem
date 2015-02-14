using Gem.Network.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Factories
{
    interface IMessageHandlerFactory
    {
        IMessageHandler Create(List<string> propertyNames, string classname, string functionName);
    }
}
