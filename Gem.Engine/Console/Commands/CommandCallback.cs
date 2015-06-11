using Gem.Infrastructure.Functional;
using System.Collections.Generic;

namespace Gem.Console.Commands
{
    public delegate Result<object> CommandCallback(ICommandHost host,
                                                   string command,
                                                   IList<string> arguments, 
                                                   object executionResult);

}
