using System.Collections.Generic;

namespace Gem.Console
{
    public class Row
    {
        private readonly int row;
        private readonly IEnumerable<ICell> entries;

        public Row(int row, IEnumerable<ICell> entries)
        {
            this.row = row;
            this.entries = entries;
        }

        public int RowIndex { get { return row; } }
        public IEnumerable<ICell> Entries { get { return entries; } }
    }

}
