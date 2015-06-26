using Gem.Infrastructure.Functional;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{    
    public interface ICommandExecutioner
    {
        Task<Result<object>> ExecuteCommand(string command);
    }

}
