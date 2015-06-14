using System.Collections.Generic;

namespace Gem.Console.EntryPoint
{

    public class FlushedEntry
    {
        private readonly string stringRepresentation;
        private readonly IEnumerable<ICell> cells;

        public FlushedEntry(IEnumerable<ICell> cells, string stringRepresentation)
        {
            this.cells = cells;
            this.stringRepresentation = stringRepresentation;
        }

        public string StringRepresentation
        {
            get { return stringRepresentation; }
        }

        public IEnumerable<ICell> Cells
        {
            get { return cells; }
        }
    }

}
