using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Console
{

    public class CommandEntry
    {

        #region Fields

        private readonly List<IEntryRule> entryRules = new List<IEntryRule>();
        private readonly Cursor cursor;
        private readonly CellAppender appender;
        private readonly CellAligner aligner;

        #endregion

        #region Ctor

        public CommandEntry(CellAppender appender, Func<float> entryWidth)
        {
            this.appender = appender;
            cursor = new Cursor();
            aligner = new CellAligner(entryWidth, appender.GetCells);

            appender.OnCellAppend((sender, args) => aligner.ArrangeRows());
            appender.OnCellAppend((sender, args) =>
            {
                cursor.Update(aligner.Rows());
                cursor.Right();
            });
        }

        #endregion

        #region Public Properties

        public Cursor Cursor { get { return cursor; } }
        public CellAligner Aligner { get { return aligner; } }
        public CellAppender Appender { get { return appender; } }

        #endregion

        #region Public Methods

        public void AddEntryRule(IEntryRule entryRule)
        {
            entryRules.Add(entryRule);
        }

        public bool RemoveEntryRule(IEntryRule entryRule)
        {
            return entryRules.Remove(entryRule);
        }

        public void DeleteEntryAfterCursor()
        {
            appender.RemoveCellAt(cursor.Head+1);
        }

        public void DeleteEntry()
        {
            appender.RemoveCellAt(cursor.Head + 1);
            Cursor.Left();
        }

        public void AddEntry(char ch)
        {
            if (!entryRules
                .Select(rule => rule.Apply(ch))
                .Any(result => result == false))
            {
                appender.AddCellAt(cursor.Head, ch);
            }
        }

        #endregion

    }
}
