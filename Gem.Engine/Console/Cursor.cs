using System;
using System.Collections.Generic;
using System.Linq;
using Gem.Infrastructure.Events;
using Microsoft.Xna.Framework;

namespace Gem.Console
{

    public class CursorEventArgs : EventArgs
    {
        private readonly int head;
        private readonly int row;

        public CursorEventArgs(int head, int row)
        {
            this.head = head;
            this.row = row;
        }

        public int Head { get { return head; } }
        public int Row { get { return row; } }
    }

    public class Cursor : ICursor
    {

        #region Fields

        private List<int> rows = new List<int>();
        private int cursorRow;
        private int head;
        private int cellSum;

        #endregion

        #region Private Helpers

        private int GetCursorRow()
        {
            int sum = 0;
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                sum += rows[rowIndex];
                if (Head < sum)
                {
                    return rowIndex;
                }
            }
            return 0;
        }

        #endregion

        #region ICursor Members

        public event EventHandler<CursorEventArgs> CursorChanged;

        public void Up()
        {
            if (cursorRow > 0)
            {
                Head = rows.Take(cursorRow - 1).Sum() + rows[cursorRow];
            }
        }

        public void Down()
        {
            if (cursorRow < rows.Count)
            {
                Head = rows.Take(cursorRow).Sum() + rows[cursorRow];
            }
        }

        public void Left()
        {
            if (Head >= 0)
            {
                Head--;
            }
        }

        public void Right()
        {
            if (Head < cellSum)
            {
                Head++;
            }
        }

        public int Head
        {
            get { return head; }
            set
            {
                head = MathHelper.Clamp(value, 0, cellSum);
                GetCursorRow();
                CursorChanged.RaiseEvent(this, new CursorEventArgs(head, cursorRow));
            }
        }

        public int Row
        {
            get { return cursorRow; }
        }

        public void Update(IEnumerable<Row> rows)
        {
            this.rows = rows.Select(row => row.Entries.Count()).ToList();
            this.cursorRow = GetCursorRow();
            this.cellSum = this.rows.Sum();
        }

        #endregion
    }
}
