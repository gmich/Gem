using Gem.Infrastructure.Functional;
using System.Collections.Generic;

namespace Gem.Console.Commands
{
    public delegate Result CommandCallback(ICommandHost host, IList<string> arguments, object executionResult);

}
