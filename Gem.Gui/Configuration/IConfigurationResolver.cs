using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Factories;
using System.Collections.Generic;

namespace Gem.Gui.Configuration
{
    public interface IConfigurationResolver
    {
        IList<IAggregator> GetAggregators(Aggregation.AggregationTarget target);
        IControlFactory GetControlFactory(ControlTarget target);
    }
}