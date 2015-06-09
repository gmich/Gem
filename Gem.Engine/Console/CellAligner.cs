using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace Gem.Console
{
    /// <summary>
    /// Alligns ICell instances into rows according to the specified buffer axis x-size
    /// </summary>
    public class CellAligner
    {

        #region Fields

        private readonly Func<IEnumerable<ICell>> cellsGetter;
        private readonly List<Row> rows = new List<Row>();
        private readonly Func<float> rowWidthGetter;

        #endregion

        #region Internal Row Class

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

        #endregion

        #region Ctor

        public CellAligner(Func<float> rowWidthGetter, Func<IEnumerable<ICell>> cellsGetter)
        {
            this.rowWidthGetter = rowWidthGetter;
            this.cellsGetter = cellsGetter;
        }

        #endregion

        #region Arrange Rows Algorithm

        public void ArrangeRows()
        {
            rows.Clear();

            int currentRowSize = 0;
            int skippedEntries = 0;
            int cellsCounter = 0;
            float rowWidth = rowWidthGetter();
            var cells = cellsGetter();

            foreach (var cell in cells)
            {
                currentRowSize += cell.SizeX;
                if (currentRowSize > rowWidth)
                {
                    rows.Add(new Row(rows.Count, cells.Skip(skippedEntries).Take(cellsCounter - skippedEntries)));
                    skippedEntries = cellsCounter;
                    currentRowSize = 0;
                }
                cellsCounter++;
            }

            //add the remaining cells
            if (cellsCounter - skippedEntries > 0)
            {
                rows.Add(new Row(rows.Count, cells.Skip(skippedEntries).Take(cellsCounter - skippedEntries)));
            }
        }

        #endregion

        #region Get Rows

        public IEnumerable<Row> Rows()
        {
            return rows.AsEnumerable();
        }

        #endregion

    }
}
