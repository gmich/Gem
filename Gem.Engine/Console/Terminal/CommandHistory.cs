using Gem.Console.EntryPoint;
using System.Collections.Generic;
namespace Gem.Console
{
    internal class CommandHistory
    {
        private readonly FixedSizeList<FlushedEntry> history;
        private int peekedEntry;

        public CommandHistory(int maxHistoryEntries)
        {
            history = new FixedSizeList<FlushedEntry>(maxHistoryEntries + 1);
            history.Add(new FlushedEntry(new List<ICell>(), string.Empty));
        }

        public void Add(FlushedEntry entry)
        {
            history.Add(entry);
            peekedEntry = history.Count - 1;
        }

        public FlushedEntry PeekNext()
        {
            peekedEntry = GuardEntry(peekedEntry - 1);

            return history [peekedEntry];
        }

        public FlushedEntry PeekPrevious()
        {
            peekedEntry = GuardEntry(peekedEntry + 1);

            return history[peekedEntry];
        }

        private int GuardEntry(int peekedEntry)
        {
            return  Microsoft.Xna.Framework.MathHelper.Clamp(peekedEntry, 0, history.Count - 1);
        }
    }
}
