using System;

namespace Gem.Gui.Aggregation
{
    public interface IAggregator
    {
        bool IsEnabled { get; set; }

        void Aggregate(GuiEntry entry, AggregationContext context);
    }
}
