﻿using Gem.Infrastructure.Functional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{
    public delegate Result<object> CommandCallback(ICommandHost host,
                                                   IList<string> arguments, 
                                                   object executionResult);

}
