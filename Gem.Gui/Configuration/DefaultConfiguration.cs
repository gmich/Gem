using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Factories;
using Gem.Gui.Input;
using System.Collections.Generic;

namespace Gem.Gui.Configuration
{
    public class DefaultConfiguration : IConfigurationResolver
    {
        public IList<IAggregator> Aggregators(AggregationTarget target)
        {
            var aggegators = new List<IAggregator>();

            if(target.HasTargetFlag(AggregationTarget.Mouse))
            {
                aggegators.Add(new MouseControlAggregator(InputManager.Mouse));
            }
            //TODO: add the rest

            return aggegators;
        }
        
        public IControlFactory GetControlFactory(ControlTarget target)
        {
            return new GeneralControlFactory();
        }
    }
}