using System;
using System.Collections.Generic;
using System.Linq;
using Gem.Infrastructure.Events;
using Microsoft.Xna.Framework;
using Gem.Infrastructure.Functional;
using System.Timers;
using Gem.Engine.Console.Rendering.Animations;
using Gem.Engine.Console.Cells;

namespace Gem.Engine.Console.EntryPoint
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

        private readonly Timer activeCursorTimer = new Timer();
        private readonly Timer showCursorTimer = new Timer();

        private List<int> rows = new List<int>();
        private int head;
        private int cellSum;
        private bool showCursor;
        private float alpha = 1.0f;

        #endregion

        #region Ctor

        public Cursor()
        {
            showCursorTimer.Elapsed += new ElapsedEventHandler((sender, args) =>
                {
                    alpha = (alpha == 0.0f || showCursor) ? 1.0f : 0.0f;
                    CreateEffect(alpha);
                });
            showCursorTimer.Interval = 500d;
            showCursorTimer.Enabled = true;

            activeCursorTimer.Elapsed += new ElapsedEventHandler((sender, args) =>
            {
                showCursor = false;
            });
            showCursorTimer.Interval = 800d;
            showCursorTimer.Enabled = true;
            CreateEffect(alpha);
        }

        #endregion

        private void CreateEffect(float alpha)
        {
            this.Effect = Animate.Cell(Behavior.Create(ctx => "|"), Behavior.Create(ctx => Color.Gray), alpha.Forever(), 0.0f.Forever());
        }
        #region Properties

        public Behavior<IEffect> Effect
        {
            get;
            set;
        }

        #endregion

        #region Private Helpers

        private int GetCursorRow()
        {
            int sum = 0;
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                sum += rows[rowIndex];
                if (Head <= sum)
                {
                    return rowIndex;
                }
            }
            return 0;
        }

        #endregion

        #region ICursor Members

        public event EventHandler<CursorEventArgs> CursorChanged;

        public void Left()
        {
            Head--;
        }

        public void Right()
        {
            Head++;
        }

        public int Head
        {
            get { return head; }
            set
            {
                head = MathHelper.Clamp(value, 0, cellSum);
                CursorChanged.RaiseEvent(this, new CursorEventArgs(head, Row));

                showCursor = true;
                activeCursorTimer.Stop();
                activeCursorTimer.Start();
            }
        }

        public int HeadInRow
        {
            get
            {
                return (Row > 0) ?
                    head - this.rows.Take(Row).Sum() : head;
            }
        }

        public int Row
        {
            get { return GetCursorRow(); }
        }

        public void Update(IEnumerable<Row> rows)
        {
            this.rows = rows.Select(row => row.Entries.Count()).ToList();
            this.cellSum = this.rows.Sum();
        }

        #endregion


    }
}
