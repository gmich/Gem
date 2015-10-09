using Gem.Engine.Logging;
using System;

namespace Gem.Engine.Console.Commands
{
    public interface ICommandHost : IDebugHost, ICommandExecutioner
    {
        void RegisterCommand(Type typeOfObjectWithCommand);

        void RegisterCommand(CommandCallback callback, string command, string description, bool requiresAuthorization);

        void UnregisterCommand(string command);

        void PushExecutioner(ICommandExecutioner executioner);

        void PopExecutioner();
    }
}
