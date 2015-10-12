using NullGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{
    public class CommandTable
    {
        private readonly List<CommandTable> subCommands = new List<CommandTable>();
        private readonly CommandCallback callback;
        private readonly string command;
        private readonly string description;
        private readonly bool requiresAuthorization;

        public CommandTable(CommandCallback callback, string command, string description, bool requiresAuthorization)
        {
            this.command = command;
            this.description = description;
            this.callback = callback;
            this.requiresAuthorization = requiresAuthorization;
            Rollback = null;
        }

        public bool AddSubCommand(CommandTable subCommand)
        {
            if (!subCommands.Any(arg => arg.command == subCommand.Command))
            {
                subCommands.Add(subCommand);
                return true;
            }
            return false;
        }

        public IEnumerable<CommandTable> SubCommand { get { return subCommands; } }
        public CommandCallback Callback { get { return callback; } }
        public CommandCallback Rollback {[return: AllowNull] get;[param: AllowNull] set; }
        public string Command { get { return command; } }
        public string Description { get { return description; } }
        public bool RequiresAuthorization { get { return requiresAuthorization; } }

    }

}
