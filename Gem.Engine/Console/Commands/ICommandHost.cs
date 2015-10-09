using Gem.Engine.Logging;

namespace Gem.Engine.Console.Commands
{
    public interface ICommandHost : IDebugHost, ICommandExecutioner
    {
        void RegisterCommand<TObject>(TObject objectWithCommand)
            where TObject : class;

        void RegisterCommand(CommandCallback callback, string command, string description, bool requiresAuthorization);

        void UnregisterCommand(string command);

        void PushExecutioner(ICommandExecutioner executioner);

        void PopExecutioner();
    }
}
