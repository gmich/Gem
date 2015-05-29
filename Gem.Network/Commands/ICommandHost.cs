using Gem.Network.Utilities.Loggers;

namespace Gem.Network.Commands
{
    internal interface ICommandHost : ICommandExecutioner , IDebugListener
    {
        /// <summary>
        /// Register new command
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="description">Description of command</param>
        /// <param name="callback">Invoke delegation</param>
        void RegisterCommand(string command, bool requiresAuthorization, string description,
                                                        ExecuteCommand callback);

        void SetPassword(string newPassword);

        /// <summary>
        /// Unregister command
        /// </summary>
        /// <param name="command">command name</param>
        void DeregisterCommand(string command);
        
        /// <summary>
        /// Add Command executioner
        /// </summary>
        void PushExecutioner(ICommandExecutioner executioner);

        /// <summary>
        /// Remote Command executioner
        /// </summary>
        void PopExecutioner();
    }
}
