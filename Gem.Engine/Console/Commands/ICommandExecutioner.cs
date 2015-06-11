using Gem.Infrastructure.Functional;

namespace Gem.Console.Commands
{    
    public interface ICommandExecutioner
    {
        Result ExecuteCommand(string command);
    }

}
