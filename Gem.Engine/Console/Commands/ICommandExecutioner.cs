using Gem.Infrastructure.Functional;

namespace Gem.Console.Commands
{    
    public interface ICommandExecutioner
    {
        Result<object> ExecuteCommand(string command);
    }

}
