using Gem.Infrastructure.Functional;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{
    public class CommonCommands
    {

        [Command(command: "echo",
                 description: "append a message")]
        public Result<object> FirstCommandCallback(ICommandHost host,
                                                    IList<string> arguments,
                                                    object executionResult)
        {

            host.Message(string.Concat(arguments));
            return Result.Successful(null);
        }

    }
}
