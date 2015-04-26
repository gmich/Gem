using Gem.Gui.Elements;
using Gem.Gui.Elements.Areas;
using System;

namespace Gem.Gui.Aggregation
{
    public interface IAggregator
    {
        bool IsActive { get; set; }
        void Aggregate(IGuiArea element, AggregationContext context);
    }
}
