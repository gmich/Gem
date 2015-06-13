namespace Gem.Console
{
    internal class CommandHistory
    {
        private readonly FixedSizeList<string> history;
        private int peekedEntry;

        public CommandHistory(int maxHistoryEntries)
        {
            history = new FixedSizeList<string>(maxHistoryEntries+1);
            history.Add(string.Empty);
        }

        public void Add(string entry)
        {
            history.Add(entry);
            peekedEntry = history.Count - 1;
        }

        public string PeekNext()
        {
            peekedEntry = GuardEntry(peekedEntry - 1);

            return history [peekedEntry];
        }

        public string PeekPrevious()
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
