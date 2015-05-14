using Gem.Gui.Controls;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Aggregation
{
    public class AggregationContext
    {
        private readonly IList<IAggregator> aggregators;
        private readonly List<GuiEntry> entries = new List<GuiEntry>();

        public AControl FocusedControl { get; set; }


        public AggregationContext(IList<IAggregator> aggregators, IEnumerable<AControl> controls)
        {
            this.aggregators = aggregators;
            foreach (var control in controls)
            {
                entries.Add(new GuiEntry(control));
            }
        }

        private IDisposable AddAggregator(IAggregator aggregator)
        {
            aggregators.Add(aggregator);
            return Gem.Infrastructure.Disposable.Create(aggregators, aggregator);
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
