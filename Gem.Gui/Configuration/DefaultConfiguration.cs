using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Factories;
using Gem.Gui.Input;
using System.Collections.Generic;

namespace Gem.Gui.Configuration
{
    public class DefaultConfiguration : IConfigurationResolver
    {

        public IEnumerable<IAggregator> GetAggregators(AggregationTarget target)
        {
            var aggegators = new List<IAggregator>();

            if (target.HasTargetFlag(AggregationTarget.Mouse))
            {
                yield return new MouseControlAggregator(InputManager.Mouse);
            }
            if (target.HasTargetFlag(AggregationTarget.Keyboard))
            {
                yield return Script.ForKeyboard(InputManager.KeyboardInputKeys);
            }
            if (target.HasTargetFlag(AggregationTarget.GamePad))
            {
                yield return Script.ForGamePad(InputManager.GamePadInputKeys);
            }
            if (target.HasTargetFlag(AggregationTarget.Touch))
            {
                //not implemented yet
            }
        }

        public IControlFactory GetControlFactory(TargetPlatform target)
        {
            return new GeneralControlFactory();
        }
    }
}