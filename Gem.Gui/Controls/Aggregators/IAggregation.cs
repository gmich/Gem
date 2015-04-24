using Gem.Gui.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Controls.Aggregators
{
    interface IAggregation
    {
        void AddAggregator();

        void RemoveAggregator();

        void Aggregate(IGuiArea area);
    }

    public class AggregationContext
    {
        List<IAggregation> aggregations;
        AggregationToken token;

    }

}
