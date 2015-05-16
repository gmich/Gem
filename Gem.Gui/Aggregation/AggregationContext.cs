using Gem.Gui.Controls;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Aggregation
{
    public class AggregationContext
    {
        private readonly IList<IAggregator> aggregators;
        private readonly List<GuiEntry> entries = new List<GuiEntry>();
        private readonly Indexer indexer;


        public AggregationContext(IList<IAggregator> aggregators, IEnumerable<AControl> controls)
        {
            this.aggregators = aggregators;
            int controlCount = 0;
            foreach (var control in controls)
            {
                if (control.Options.IsFocusEnabled)
                {
                    entries.Add(new GuiEntry(control, controlCount++));
                }
            }
            indexer = new Indexer(0, entries.Count - 1);

            if (entries.Count > 0)
            {
                FocusControlAt(0);
            }
        }

        public Indexer Indexer { get { return indexer; } }

        public bool FirstEntry { get; set; }

        public IDisposable AddAggregator(IAggregator aggregator)
        {
            aggregators.Add(aggregator);
            return Gem.Infrastructure.Disposable.Create(aggregators, aggregator);
        }

        public void Reset()
        {
            foreach (var entry in entries)
            {
                entry.Control.HasFocus = false;
            }
        }

        public void TriggerFocusedControl()
        {
            foreach (var entry in entries)
            {
                if (entry.Control.HasFocus)
                {
                    entry.Control.Events.OnClicked();
                }
            }
        }
        
        public AControl this[int entryId]
        {
            get { return entries[entryId].Control; }
        }

        public void FocusControlAt(int controlIndex)
        {
            entries[indexer.Current].Control.HasFocus = false;

            indexer.Current = controlIndex;
            entries[indexer.Current].Control.HasFocus = true;
        }

        public void Aggregate()
        {
            FirstEntry = true;
            foreach (var aggregator in aggregators)
            {
                entries.ForEach(entry => aggregator.Aggregate(entry, this));
            }
        }
    }
}
