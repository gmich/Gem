using Gem.Gui.Controls;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Aggregation
{
    public class AggregationContext
    {
        private readonly List<IAggregator> aggregators = new List<IAggregator>();

        public AControl FocusedControl { get; set; }

        private readonly List<GuiEntry> entries = new List<GuiEntry>();

        public IDisposable AddAggregator(IAggregator aggregator)
        {
            aggregators.Add(aggregator);
            return Gem.Infrastructure.Disposable.Create(aggregators, aggregator);
        }

        public void AddControlRange(params AControl[] controls)
        {
            foreach (var control in controls)
            {
                entries.Add(new GuiEntry(control));
            }
        }

        public void Aggregate()
        {
            foreach (var aggregator in aggregators)
            {
                entries.ForEach(entry => aggregator.Aggregate(entry, this));
            }
        }
    }
}
