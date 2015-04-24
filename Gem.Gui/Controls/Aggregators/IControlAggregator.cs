using Gem.Gui.Elements;

namespace Gem.Gui.Controls.Aggregators
{
    public interface IControlAggregator
    {
        void Aggregate(IGuiComponent element, AggregationToken context);
    }
}
