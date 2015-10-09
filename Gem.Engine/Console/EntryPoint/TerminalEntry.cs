using Gem.Infrastructure.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using Gem.Infrastructure.Events;
using Gem.Engine.Console.Commands;
using Gem.Engine.Console.Cells;

namespace Gem.Engine.Console.EntryPoint
{

    public class TerminalEntry
    {

        #region Fields

        private readonly List<IEntryRule> entryRules = new List<IEntryRule>();
        private readonly Cursor cursor;
        private readonly CellAppender appender;
        private readonly CommandHistory history;

        #endregion

        #region Events

        public event EventHandler<string> OnFlushedEntry;


        #endregion

        #region Ctor

        public TerminalEntry(CellAppender appender, CellRowAligner aligner, Func<int> spacing, Func<float> rowSize, int historyEntries = 40)
        {
            this.cursor = new Cursor();

            if (appender == null)
            {
                throw new ArgumentNullException("appender");
            }
            this.appender = appender;

            AddEntryRule(new EntryRule(ch => appender.Count < 100));

            history = new CommandHistory(historyEntries);

            appender.OnCellAppend((sender, args) =>
            {
                aligner.AlignToRows(appender.Cells(), spacing(), rowSize());
                cursor.Update(aligner.Rows());
            });
        }

        #endregion

        #region Public Properties

        public Cursor Cursor { get { return cursor; } }
        public CellAppender Appender { get { return appender; } }

        #endregion

        #region History

        public void PeekNext()
        {
            appender.Clear();
            appender.AddCellRange(history.PeekNext().Cells);
            cursor.Head = appender.Count;
        }

        public void PeekPrevious()
        {
            appender.Clear();
            appender.AddCellRange(history.PeekPrevious().Cells);
            cursor.Head = appender.Count;
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
            appender.RemoveCellAt(cursor.Head);
        }

        public void DeleteEntry()
        {
            appender.RemoveCellAt(cursor.Head - 1);
            Cursor.Left();
        }

        public void AddEntry(char ch)
        {
            if (!entryRules
                .Select(rule => rule.Apply(ch))
                .Any(result => result == false))
            {
                appender.AddCellAt(cursor.Head, ch);
                cursor.Right();
            }
        }

        public void AppendString(string entryString)
        {
            InsertString(entryString);
            Flush();
        }

        public void InsertString(string entryString)
        {
            foreach (var character in entryString)
            {
                AddEntry(character);
            }
        }

        public Result<FlushedEntry> Flush()
        {
            if (appender.Count > 0)
            {
                var entry = new FlushedEntry(appender.Cells(), appender.ToString());
                var result = Result.Ok(entry);
                OnFlushedEntry.RaiseEvent(this, entry.StringRepresentation);
                appender.Clear();
                history.Add(entry);
                return result;
            }
            return Result.Fail<FlushedEntry>("No entries");
        }

        #endregion

    }
}
