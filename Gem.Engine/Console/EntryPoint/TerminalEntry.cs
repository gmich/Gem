using Gem.Infrastructure.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using Gem.Infrastructure.Events;
using Gem.Console.EntryPoint;

namespace Gem.Console
{

    public class TerminalEntry
    {

        #region Fields

        private readonly List<IEntryRule> entryRules = new List<IEntryRule>();
        private readonly Cursor cursor;
        private readonly CellAppender appender;
        private readonly CellAligner aligner;
        private readonly CommandHistory history;

        #endregion

        #region Events

        public event EventHandler<string> OnFlushedEntry;


        #endregion

        #region Ctor

        public TerminalEntry(CellAppender appender,CellAligner aligner, Func<int> spacing, Func<float> rowSize, int historyEntries = 40)
        {
            this.cursor = new Cursor();

            if (appender == null)
            {
                throw new ArgumentNullException("appender");
            }
            this.appender = appender;

            history = new CommandHistory(historyEntries);

            appender.OnCellAppend((sender, args) =>
            {
                aligner.AlignToRows(appender.GetCells(), spacing(), rowSize());
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

        #region History

        public void PeekNext()
        {
            appender.Clear();
            appender.AddCellRange(history.PeekNext().Cells);
        }

        public void PeekPrevious()
        {
            appender.Clear();
            appender.AddCellRange(history.PeekPrevious().Cells);
        }

        #endregion

        #region Entries

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
            appender.RemoveCellAt(cursor.Head + 1);
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

        public Result<FlushedEntry> Flush()
        {
            if (appender.Count > 0)
            {
                var entry = new FlushedEntry(appender.GetCells(), appender.ToString());
                var result = Result.Ok(entry);
                appender.Clear();
                history.Add(entry);
                OnFlushedEntry.RaiseEvent(this, entry.StringRepresentation);
                return result;
            }
            return Result.Fail<FlushedEntry>("No entries");
        }

        #endregion

    }
}
