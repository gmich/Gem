using System.Collections.Generic;

namespace Gem.Engine.Console.Commands
{
    internal class ExecutionGraphEntry
    {
        private readonly IList<string> arguments = new List<string>();
        private readonly CommandCallback cmd;
        private readonly CommandCallback rollback;

        public ExecutionGraphEntry(CommandCallback cmd, CommandCallback rollback, IList<string> arguments)
        {
            this.arguments = arguments;
            this.cmd = cmd;
            this.rollback = rollback;
        }

        public IList<string> Arguments { get { return arguments; } }
        public CommandCallback Callback { get { return cmd; } }
        public CommandCallback Rollback { get { return rollback; } }
    }

}
