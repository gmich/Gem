using Gem.Infrastructure.Functional;

namespace Gem.Console.Commands
{    
    public interface ICommandExecutioner
    {
        Result<bool> ExecuteCommand(string command);
    }

}
