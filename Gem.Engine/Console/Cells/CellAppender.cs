using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Gem.Console.Animations;
using Gem.Infrastructure.Functional;

namespace Gem.Console
{

    public class CellAppender
    {
        #region Fields

        private readonly Func<char, ICell> cellGenerator;
        private readonly ObservableCollection<ICell> cells = new ObservableCollection<ICell>();
        private readonly ICellBehavior cellBehavior;

        #endregion

        #region Ctor

        public CellAppender(Func<char, ICell> cellGenerator, ICellBehavior cellBehavior)
        {
            this.cellGenerator = cellGenerator;
            this.cellBehavior = cellBehavior;
        }

        #endregion

        #region Properties

        public ICellBehavior CellBehavior { get { return cellBehavior; } }

        #endregion

        #region Add / Remove

        public void AddCellRange(IEnumerable<ICell> cellRange)
        {
            foreach (var cell in cellRange)
            {
                cells.Add(cell);
            }
        }

        public void AddCell(char cell)
        {
            ICell generatedCell = cellGenerator(cell);
            cells.Add(generatedCell);
        }

        public bool AddCellAt(int index, char cell)
        {
            if (cells.Count > index)
            {
                ICell generatedCell = cellGenerator(cell);
                cells.Insert(index, generatedCell);
                return true;
            }
            return false;
        }

        public bool RemoveCellAt(int index)
        {
            if (IndexExists(index))
            {
                cells.RemoveAt(index);
                return true;
            }
            return false;
        }

        #endregion

        #region Collection

        public IEnumerable<ICell> GetCells()
        {
            return cells;
        }

        public void Clear()
        {
            cells.Clear();
        }

        public int Count
        {
            get { return cells.Count; }
        }

        public override string ToString()
        {
            return String.Concat(cells);
        }

        #endregion

        #region Guard

        private bool IndexExists(int index)
        {
            return (cells.Count > index);
        }

        #endregion

        #region Events

        public void SubscribeObserver(IObserver<ICell> observer)
        {
            cells.Subscribe(observer);
        }

        public void OnCellAppend(NotifyCollectionChangedEventHandler collectionChangedEvent)
        {
            cells.CollectionChanged += collectionChangedEvent;
        }

        #endregion

    }
}
