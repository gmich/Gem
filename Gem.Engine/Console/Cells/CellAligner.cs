using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gem.Infrastructure.Events;
using Microsoft.Xna.Framework;
using Gem.Console.Animations;
using Gem.Infrastructure.Functional;

namespace Gem.Console
{

    public class CellAlignerEventArgs : EventArgs
    {
        private readonly Row row;
        private readonly int rowIndex;

        public CellAlignerEventArgs(Row row, int rowIndex)
        {
            this.rowIndex = rowIndex;
            this.row = row;
        }

        public int RowIndex { get { return rowIndex; } }
        public Row Row { get { return row; } }
    }

    public class CellAlignmentOptions
    {
        public event EventHandler<EventArgs> OnOptionChanged;

        public CellAlignmentOptions(int cellSpacing)
        {
            this.spacing = cellSpacing;
        }

        private int spacing;
        public int CellSpacing
        {
            get { return spacing; }
            set
            {
                spacing = value;
                OnOptionChanged.RaiseEvent(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Alligns ICell instances into rows according to the specified buffer axis x-size
    /// </summary>
    public class CellAligner
    {

        #region Fields

        private readonly List<Row> rows;

        #endregion

        #region Ctor

        public CellAligner()
        {
            this.rows = new List<Row>();
        }

        #endregion

        #region Events

        public event EventHandler<CellAlignerEventArgs> RowAdded;

        #endregion

        #region Arrange Rows Algorithm

        public void Reset()
        {
            rows.Clear();
        }

        public void AlignToRows(IEnumerable<ICell> cells, int spacing, float rowSize)
        {
            int currentRowSize = 0;
            int skippedEntries = 0;
            int cellsCounter = 0;

            foreach (var cell in cells)
            {
                currentRowSize += (cell.SizeX + spacing);
                if (currentRowSize > rowSize)
                {
                    var row = new Row(rows.Count, cells.Skip(skippedEntries).Take(cellsCounter - skippedEntries));
                    rows.Add(row);
                    skippedEntries = cellsCounter;
                    currentRowSize = 0;
                    RowAdded.RaiseEvent(this, new CellAlignerEventArgs(row, rows.Count));
                }
                cellsCounter++;
            }

            //add the remaining cells
            if (cellsCounter - skippedEntries > 0)
            {
                var row = new Row(rows.Count,cells.Skip(skippedEntries).Take(cellsCounter - skippedEntries));
                rows.Add(row);
                RowAdded.RaiseEvent(this, new CellAlignerEventArgs(row, rows.Count));
            }
        }

        #endregion

        #region Get Rows

        public IEnumerable<Row> Rows()
        {
            return rows;
        }

        #endregion

    }
}
