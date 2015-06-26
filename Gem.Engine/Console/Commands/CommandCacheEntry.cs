using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{

    internal class CommandCacheEntry<TEntry>
    {
        private readonly string command;
        private readonly List<TEntry> cachedCommands = new List<TEntry>();

        public CommandCacheEntry(string command, TEntry cachedCommand)
        {
            this.command = command;
            cachedCommands.Add(cachedCommand);
        }

        public void AddEntry(TEntry cachedCommand)
        {
            cachedCommands.Add(cachedCommand);
        }

        public string Command { get { return command; } }
        public IEnumerable<TEntry> Entries { get { return cachedCommands; } }
    }

}
